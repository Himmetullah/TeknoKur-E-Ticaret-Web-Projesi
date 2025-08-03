using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Layer.Data;
using Layer.Entities;
using Microsoft.Extensions.Options;
using UI.Customer.Entities;

namespace UI.Customer.Models;

public class TPayment
{
    public async Task<TResult> Pay(List<TblUrun> Urunler, TCrediCart crediCart, TPersonContact personContact, string BaseUrl)
    {
        TResult result = new();

        Iyzipay.Options options = new();
        options.SecretKey = "SecretKey";
        options.ApiKey = "ApiKey";
        options.BaseUrl = BaseUrl;

        CreatePaymentRequest createPaymentRequest = new();
        createPaymentRequest.Price = Urunler.Sum(r => r.UrunFiyat).ToString();
        createPaymentRequest.PaidPrice = createPaymentRequest.Price;
        createPaymentRequest.Currency = Currency.TRY.ToString();
        createPaymentRequest.Installment = 1;
        createPaymentRequest.BasketId = Guid.NewGuid().ToString();
        createPaymentRequest.PaymentChannel = PaymentChannel.WEB.ToString();
        createPaymentRequest.PaymentChannel = PaymentGroup.LISTING.ToString();
        createPaymentRequest.PaymentCard = crediCart;
        createPaymentRequest.Buyer = personContact;
        createPaymentRequest.ShippingAddress = personContact.Address;

        List<BasketItem> basketItems = new();
        foreach (var Urun in Urunler)
        {
            basketItems.Add(new BasketItem()
            {
                Category1 = "Urunler",
                Id = Urun.UrunId.ToString(),
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Name = Urun.UrunAdi,
                Price = Urun.UrunFiyat.ToString(),
            });
        }

        createPaymentRequest.BasketItems.AddRange(basketItems);
        Payment PaymentResult = await Payment.Create(createPaymentRequest, options);

        return result;
    }
}
