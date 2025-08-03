(function ($) {
 "use strict";

  /*----------------------------
 TOP Menu Stick
------------------------------ */
$(window).on('scroll',function() {
if ($(this).scrollTop() > 120){  
    $('#header-sticky').addClass("sticky");
  }
  else{
    $('#header-sticky').removeClass("sticky");
  }
}); 

  /*----------------------------
 TOP Menu Stick
------------------------------ */
$(window).on('scroll',function() {
if ($(this).scrollTop() > 40){  
    $('#header-sticky-2').addClass("sticky");
  }
  else{
    $('#header-sticky-2').removeClass("sticky");
  }
}); 


/*----------------------------
 jQuery MeanMenu
------------------------------ */
	jQuery('#mobile-menu-active').meanmenu();	
	
	
/*----------------------------
 nivoSlider
------------------------------ */	
	$("#slider").nivoSlider({ 
    effect: 'fold',                 // Specify sets like: 'fold,fade,sliceDown' 
    slices: 15,                       // For slice animations 
    boxCols: 8,                       // For box animations 
    boxRows: 4,                       // For box animations 
    animSpeed: 500,                   // Slide transition speed 
    pauseTime: 3000,                  // How long each slide will show 
    startSlide: 0,                    // Set starting Slide (0 index) 
    directionNav: true,               // Next & Prev navigation 
    controlNav: false,                 // 1,2,3... navigation 
    controlNavThumbs: false,          // Use thumbnails for Control Nav 
    pauseOnHover: true,               // Stop animation while hovering 
    manualAdvance: true,             // Force manual transitions 
    prevText: '<i class="fa fa-angle-left"></i>',   // Prev directionNav text 
    nextText: '<i class="fa fa-angle-right"></i>',  // Next directionNav text 
    randomStart: true,               // Start on a random slide 
    beforeChange: function(){},       // Triggers before a slide transition 
    afterChange: function(){},        // Triggers after a slide transition 
    slideshowEnd: function(){},       // Triggers after all slides have been shown 
    lastSlide: function(){},          // Triggers when last slide is shown 
    afterLoad: function(){}           // Triggers when slider has loaded 
});		
/*----------------------------
 nivoSlider
------------------------------ */	
	$("#slider-2").nivoSlider({ 
    effect: 'fold',                 // Specify sets like: 'fold,fade,sliceDown' 
    slices: 15,                       // For slice animations 
    boxCols: 8,                       // For box animations 
    boxRows: 4,                       // For box animations 
    animSpeed: 500,                   // Slide transition speed 
    pauseTime: 3000,                  // How long each slide will show 
    startSlide: 0,                    // Set starting Slide (0 index) 
    directionNav: false,               // Next & Prev navigation 
    controlNav: true,                 // 1,2,3... navigation 
    controlNavThumbs: false,          // Use thumbnails for Control Nav 
    pauseOnHover: true,               // Stop animation while hovering 
    manualAdvance: true,             // Force manual transitions 
    prevText: '<i class="fa fa-angle-left"></i>',   // Prev directionNav text 
    nextText: '<i class="fa fa-angle-right"></i>',  // Next directionNav text 
    randomStart: true,               // Start on a random slide 
    beforeChange: function(){},       // Triggers before a slide transition 
    afterChange: function(){},        // Triggers after a slide transition 
    slideshowEnd: function(){},       // Triggers after all slides have been shown 
    lastSlide: function(){},          // Triggers when last slide is shown 
    afterLoad: function(){}           // Triggers when slider has loaded 
});	

/*----------------------------
 wow js active
------------------------------ */
 new WOW().init();
 
/*----------------------------
 product-active
------------------------------ */  
 $('.product-active').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:2
			},
			768:{
				items:3
			},
			992:{
				items:4
			},
			1000:{
				items:4
			}
		}
	}) 
	 
/*----------------------------
 product-active-3
------------------------------ */  
 $('.product-active-3').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:2
			},
			768:{
				items:3
			},
			992:{
				items:4
			},
			1000:{
				items:5
			}
		}
	}) 

	/*----------------------------
 product-active-9
------------------------------ */  
$('.product-active-9').owlCarousel({
	smartSpeed:1000,
	margin:0,
	autoplay:false,
	nav:true,
	dots:false,
	loop:true,
	navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
	responsive:{
		0:{
			items:1
		},
		480:{
			items:1
		},
		768:{
			items:3
		},
		992:{
			items:1
		},
		1000:{
			items:1
		}
	}
})
/*----------------------------
 blog-active
------------------------------ */  
 $('.blog-active').owlCarousel({
		smartSpeed:1000,
		margin: 30,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:1
			},
			768:{
				items:2
			},
			992:{
				items:3
			},
			1000:{
				items:3
			}
		}
	}) 		 
/*----------------------------
 blog-active-2
------------------------------ */  
 $('.blog-active-2').owlCarousel({
		smartSpeed:1000,
		margin: 30,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:1
			},
			768:{
				items:2
			},
			992:{
				items:3
			},
			1000:{
				items:3
			}
		}
	}) 
	
/*----------------------------
 brand-active
------------------------------ */  
 $('.brand-active').owlCarousel({
		smartSpeed:1000,
		margin: 30,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:2
			},
			480:{
				items:3
			},
			768:{
				items:4
			},
			992:{
				items:6
			},
			1000:{
				items:6
			}
		}
	}) 
		
/*----------------------------
 brand-active
------------------------------ */  
 $('.brand-active-2').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:false,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:2
			},
			480:{
				items:3
			},
			768:{
				items:4
			},
			992:{
				items:6
			},
			1000:{
				items:6
			}
		}
	}) 
	
/*----------------------------
 product-active-2
------------------------------ */  
$('.product-active-2').owlCarousel({
	smartSpeed: 1000,
	margin: 0,
	autoplay: false,
	nav: false,
	dots: false,
	loop: true,
	navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
	responsive: {
		0: {
			items: 1
		},
		480: {
			items: 1
		},
		768: {
			items: 1
		},
		992: {
			items: 1
		},
		1000: {
			items: 1
		}
	}
})
 	
/*----------------------------
 product-active-4
------------------------------ */  
 $('.product-active-4').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:1
			},
			768:{
				items:1
			},
			992:{
				items:1
			},
			1000:{
				items:1
			}
		}
})
/*----------------------------
 product-active-5
------------------------------ */  
 $('.product-active-5').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:1
			},
			768:{
				items:2
			},
			992:{
				items:3
			},
			1000:{
				items:3
			}
		}
	})

  	
/*----------------------------
 product-active-7
------------------------------ */  
 $('.product-active-7').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			768:{
				items:1
			},
			1000:{
				items:1
			}
		}
	})  	
	
/*----------------------------
 product-active-8
------------------------------ */  
 $('.product-active-8').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:false,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			768:{
				items:1
			},
			1000:{
				items:1
			}
		}
	})  	
		
/*----------------------------
 product-active-6
------------------------------ */  
 $('.product-active-6').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:2
			},
			768:{
				items:3
			},
			992:{
				items:4
			},
			1000:{
				items:4
			}
		}
	})  
	
	
/*----------------------------
 tab-active-3
------------------------------ */  
 $('.tab-active-3').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:2
			},
			768:{
				items:3
			},
			992:{
				items:3
			},
			1000:{
				items:3
			}
		}
	})
	
	
 /*----------------------------
 tab-active-4
------------------------------ */  
 $('.tab-active-4').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:false,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:2
			},
			768:{
				items:3
			},
			992:{
				items:4
			},
			1000:{
				items:4
			}
		}
	})
 
/*----------------------------
 tab-active
------------------------------ */  
 $('.tab-active').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:true,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:1
			},
			768:{
				items:1
			},
			992:{
				items:1
			},
			1000:{
				items:1
			}
		}
	})
/*----------------------------
 tab-active-2
------------------------------ */  
 $('.tab-active-2').owlCarousel({
		smartSpeed:1000,
		margin:0,
		autoplay:false,
		nav:true,
		dots:true,
		loop:true,
		navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		responsive:{
			0:{
				items:1
			},
			480:{
				items:1
			},
			768:{
				items:1
			},
			992:{
				items:1
			},
			1000:{
				items:1
			}
		}
	})

	
	
/*--------------------------
   chosen
---------------------------- */		
	var config = {
	'.chosen-select-deselect'  : {allow_single_deselect:true},
    }
    for (var selector in config) {
      $(selector).chosen(config[selector]);
    }

/*-------------------------
  showlogin toggle function
--------------------------*/
	 $( '#showlogin' ).on('click', function() {
        $( '#checkout-login' ).slideToggle(900);
     }); 
	
/*-------------------------
  showcoupon toggle function
--------------------------*/
	 $( '#showcoupon' ).on('click', function() {
        $( '#checkout_coupon' ).slideToggle(900);
     });
	
/*-------------------------
  Create an account toggle function
--------------------------*/
	$( '#cbox' ).on('click', function() {
        $( '#cbox_info' ).slideToggle(900);
     });
	 
/*-------------------------
  Create an account toggle function
--------------------------*/
	 $( '#ship-box' ).on('click', function() {
        $( '#ship-box-info' ).slideToggle(1000);
     });	
		 
	 	 	
/*-------------------------
  Create an account toggle function
--------------------------*/
	 $( '#side-show' ).on('click', function() {
        $( '#side-hide' ).slideToggle(1000);
     });

/*-------------------------
  showlogin toggle function
--------------------------*/
	 $( '#showcat' ).on('click', function() {
        $( '#hidecat' ).slideToggle(900);
     }); 
	
	
 /*--------------------------
   Countdown
---------------------------- */	
    $('[data-countdown]').each(function() {
        var $this = $(this), finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function(event) {
        $this.html(event.strftime('<div class="cdown days"><span class="counting counting-2">%-D</span>days</div><div class="cdown hours"><span class="counting counting-2">%-H</span>hrs</div><div class="cdown minutes"><span class="counting counting-2">%M</span>mins</div><div class="cdown seconds"><span class="counting">%S</span>secs</div>'));
        });
    });
	
/*---------------------
	counter
--------------------- */	  

$('.counter').counterUp({
    delay: 10,
    time: 1000
});

	
/*----------------------------
 flexslider-slider active
------------------------------ */
	  $('.flexslider').flexslider({
		animation: "slide",
		controlNav: "thumbnails"
	  });
	  
	
/*----------------------------
 price-slider active
------------------------------ */  
	  $( "#slider-range" ).slider({
	   range: true,
	   min: 0,
	   max: 500,
	   values: [ 30, 470 ],
	   slide: function( event, ui ) {
		$( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
	   }
	  });
	  $( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
	   " - $" + $( "#slider-range" ).slider( "values", 1 ) );  
	   
/*--------------------------
 scrollUp
---------------------------- */	
	$.scrollUp({
        scrollText: '<i class="fa fa-angle-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade'
    }); 	   
 
})(jQuery); 