'use strict';

angular.module('mobireward.servies', [])

	.service('Promotions', function($http) {

	var baseUrl = "http://182.18.157.106:8076/api/MobiRewardServices/";
	
	this.searchPromotion = function(countryId, keyword, dateValue ) {	    
	    return $http({
	        method: 'GET',
	        url: baseUrl + "PromotionSearchCountryCode?countryCode=" + countryId + "&promotionName="+ keyword +"&endDate="+ dateValue + "&browseBy="
	     });
	}

	this.browseAll = function(countryCode) {	    
	    return $http({
	        method: 'GET',
	        url:  baseUrl + "BrowserAllPromotionsByCountryCode?countryCode=" + countryCode
	        //url: baseUrl + "BrowserAllPromotions?countryId=" + countryId ,                
	     });
	}

	this.coupons=function(promotionId){
		return $http({
		        method: 'GET',
		        url: baseUrl + "GetCouponsByPromotionId?id=" + promotionId ,                
		     });
	}

	this.getCouponDetail=function(couponId){
		return $http({
		        method: 'GET',
		        url: baseUrl + "GetCouponByIdV2?id=" + couponId ,                
		     });
	}

	this.isValidCoupon=function(qrCode){
		return $http({
			method: 'GET',
			url: baseUrl + "IsValidCoupon?qrcode=" + qrCode 
		});		
	}
});