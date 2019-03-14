angular.module('mobireward', [

	'ionic',         
    'ngCordova',
	'mobireward.factory',	
	'mobireward.controllers',
    'mobireward.servies',    
    
])

/*.run(function($ionicPlatform){
    $ionicPlatform.ready(function() {              
            //alert("Device ready...")      
            dbFactory.PrepareDB(); 
    });
})*/

.config(function($stateProvider, $urlRouterProvider) {

  $stateProvider
    .state('home', {
      url: "/home",
      templateUrl: "partial/home.html",
      controller: 'HomeCtrl'
    })

    .state('promotions', {
        url: "/promotions/:searchType/:countryId/:keyword/:availability",
        templateUrl: "partial/promotions.html",
        controller: 'PromotionsCtrl'
    })

    .state('coupons', {
        url: "/coupons/:promotionId",
        templateUrl: "partial/coupons.html",
        controller: 'CouponsCtrl'
    })

    .state('coupons-detail', {
        url: "/coupons-detail/:couponId",
        templateUrl: "partial/coupons-detail.html",
        controller: 'CouponsDetailCtrl'
    })
    
    .state('location', {
        url: "/location",
        templateUrl: "partial/location.html",
        controller: 'LocationCtrl'
    })

    .state('my-coupons', {
        url: "/my-coupons",
        templateUrl: "partial/my-coupons.html",
        controller: 'MyCouponsCtrl'
    })

    .state('my-coupons-detail', {
        url: "/my-coupons-detail/:couponId",
        templateUrl: "partial/my-coupons-detail.html",
        controller: 'MyCouponDetailCtrl'
    })

    .state('redeem-coupon', {
        url: "/redeem-coupon/:couponId",
        templateUrl: "partial/redeem-coupon.html",
        controller: 'RedeemCtrl'
    })

    ;

   $urlRouterProvider.otherwise("/home");

})

 
