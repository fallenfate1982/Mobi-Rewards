'use strict';

angular.module('mobireward.servies', [])

	.service('Promotions', function($http) {

		var baseUrl = "http://182.18.157.106:8076/api/MobiRewardServices/";	
		
		this.isValidCoupon=function(qrCode){
			return $http({
				method: 'GET',
				url: baseUrl + "IsValidCoupon?qrcode=" + qrCode 
			});		
		}

		this.isValidCouponByMerchant=function(qrCode, userName){
			return $http({
				method: 'GET',
				url: baseUrl + "IsValidCouponByMerchant?qrcode=" + qrCode + "&merchantUserName=" + userName
			});		
		}

		this.login=function(userName, password){
			return $http({
				method: 'GET',
				url: baseUrl + "Login?userName=" + userName + "&password="  + password
			});		
		}


});