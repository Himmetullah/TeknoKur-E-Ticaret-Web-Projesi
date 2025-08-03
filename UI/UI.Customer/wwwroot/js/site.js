$(document).ready(function () {

    const isUserLoggedIn = document.cookie.indexOf("UserSession=") !== -1;
    function getLocalCart() {

        const cart = localStorage.getItem('anon_cart');

        return cart ? JSON.parse(cart) : [];

    }
    function setLocalCart(cart) {

        localStorage.setItem('anon_cart', JSON.stringify(cart));

    }
    function getGuestFavorites() {

        const favorites = localStorage.getItem('guestFavorites');

        return favorites ? JSON.parse(favorites) : [];

    }
    function saveGuestFavorites(favorites) {

        localStorage.setItem('guestFavorites', JSON.stringify(favorites));

    }
    function updateCartNavbarCount() {

        if (isUserLoggedIn) {

            $.get("/Sepet/GetSepetItemCount", function (res) {

                $('#navbar-cart-count').text(res?.sepetAdet || 0);

            }).fail(() => $('#navbar-cart-count').text('0'));

        } else {
            localStorage.removeItem("cart");
            $('#navbar-cart-count').text('0');
        }
    }
    function updateFavoriteView(favoriteIds) {

        $('.favori-toggle i').removeClass('fa-heart').addClass('fa-heart-o');

        favoriteIds.forEach(id => {

            $(`.favori-toggle[data-id='${id}'] i`).removeClass('fa-heart-o').addClass('fa-heart');

        });

        $('#navbar-favori-count').text(favoriteIds.length);
    }
    if (isUserLoggedIn) {

        $.get("/Favori/UrunIdler", res => {
            if (res?.urunIdler) updateFavoriteView(res.urunIdler);
        });
        localStorage.removeItem('guestFavorites');
    } else {

        updateFavoriteView(getGuestFavorites());
    }
    $(document).on("click", ".favori-ekle-kaldir", function (e) {

        e.preventDefault();

        const btn = $(this);

        const urunId = parseInt(btn.data("id"));

        const icon = btn.find("i");

        const isFavorited = icon.hasClass('fa-heart');

        if (!urunId || isNaN(urunId)) {

            console.error("Geçersiz ürün ID:", urunId);
            return;
        }
        if (isFavorited) {

            $.get("/Favori/GetFavoriIdByUrunId", { urunId: urunId }, function (res) {

                if (res.basarili && res.favoriId) {

                    const favoriId = res.favoriId;

                    $.post("/Favori/FavoriSil", { favoriId: favoriId }, function (silmeRes) {

                        if (silmeRes.basarili) {

                            icon.removeClass("fa-heart").addClass("fa-heart-o"); 

                            alert("Favorilerden kaldırıldı.");

                            $.get("/Favori/UrunIdler", r => {

                                if (r?.urunIdler) updateFavoriteView(r.urunIdler);

                            });

                        } else {

                            alert("Hata: " + (silmeRes.mesaj || "Favori kaldırma işlemi başarısız."));
                        }
                    }).fail(() => alert("Favori kaldırma işlemi sırasında sunucu hatası."));

                } else {
                    alert("Hata: Bu ürün favorilerinizde bulunamadı veya zaten kaldırılmış. " + (res.mesaj || ""));

                    $.get("/Favori/UrunIdler", r => {

                        if (r?.urunIdler) updateFavoriteView(r.urunIdler);

                    });
                }
            }).fail(() => alert("Favori ID'si alınırken sunucu hatası."));

        } else {

            $.post("/Favori/Ekle", { urunId: urunId }, res => {

                if (res.basarili) {

                    icon.removeClass("fa-heart-o").addClass("fa-heart");

                    alert("Favorilere eklendi.");

                    $.get("/Favori/UrunIdler", r => {

                        if (r?.urunIdler) updateFavoriteView(r.urunIdler);

                    });
                } else {
                    alert("Hata: " + (res.mesaj || "Favori ekleme işlemi başarısız."));
                }
            }).fail(() => alert("Favori ekleme işlemi sırasında sunucu hatası."));
        }
    });

    $(document).on("click", ".favori-kaldir", function (e) {

        e.preventDefault();

        const favoriId = parseInt($(this).data("id"));

        if (!favoriId || isNaN(favoriId)) return;

        $.post("/Favori/FavoriSil", { favoriId }, res => {

            if (res.basarili) {
                $(this).closest("tr").remove(); 

                alert("Favori başarıyla kaldırıldı.");
                $.get("/Favori/Say", r => {
                    $('#navbar-favori-count').text(r?.say || 0);
                });
                $.get("/Favori/UrunIdler", r => {
                    if (r?.urunIdler) updateFavoriteView(r.urunIdler);
                });
            } else {
                alert("Hata: " + (res.mesaj || "Favori kaldırılamadı."));
            }
        }).fail(() => alert("Sunucu hatası oluştu."));
    });
    $(document).on("click", ".sepete-ekle-btn", function (e) {
        e.preventDefault();
        const btn = $(this);
        const urunId = parseInt(btn.data("id") || btn.data("product-id"));
        let adet = 1;
        const quantityInput = btn.closest('.product-info-main, .product-item').find('.qty, .adet-input-selector');

        if (quantityInput.length > 0) {
            const inputVal = parseInt(quantityInput.val());
            adet = (!isNaN(inputVal) && inputVal > 0) ? inputVal : 1;
        }

        if (!urunId || isNaN(urunId)) {
            alert("Ürün tanımlanamadı.");
            return;
        }
        if (!isUserLoggedIn) {
            alert("Sepete ürün eklemek için giriş yapmalısınız.");
            window.location.href = "/Uye/GirisYap";
            return;
        }
        $.post("/Sepet/Ekle", { urunId, adet }, res => {

            if (res.basarili) {
                alert(res.mesaj || "Ürün sepete eklendi.");
                updateCartNavbarCount();
            } else {
                if (res.yonlendir) {
                    window.location.href = res.yonlendir;
                } else {

                    alert(res.mesaj || "Sepete eklenirken hata oluştu.");
                }
            }
        }).fail(() => alert("Sunucuya bağlanırken hata oluştu."));
    });
    updateCartNavbarCount();

});