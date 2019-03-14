'use strict';

angular.module('mobireward.controllers', [])

    .controller('MainCtrl', function ($scope, $rootScope, $location, $window, $ionicPopup, $ionicLoading, $ionicPlatform,dbFactory, $cordovaBarcodeScanner, Promotions, $cordovaNetwork) {		
		
		$rootScope.domain="http://182.18.157.106:8076/";		
		dbFactory.PrepareDB(); 
		/*$ionicPlatform.ready(function() {				 
			//alert("Device ready...")	    
		    dbFactory.PrepareDB(); 
  		});*/

		$rootScope.scan = function () {        	
			$cordovaBarcodeScanner.scan().then(function(result) {				
		    	Promotions.isValidCoupon(result.text).then(function(response) { 
		    		//alert(result.text);
			        if( response.data == true){
			        	$rootScope.alert("Info", "Coupon is valid");			        	
			        }			       
			        else{
			        	$rootScope.alert("Info", "Invalid coupon");
			        }

		    	});
		  }, function(err) {
		  });	
		}

		 $rootScope.showLoading = function() {
		    $ionicLoading.show({
		      template: 'Loading...'
		    });
		  };
		  $rootScope.hideLoading = function(){
		    $ionicLoading.hide();
		  };

        $rootScope.back = function () { 
        	          
            $window.history.back();
        }

        $rootScope.go = function (path) {            
            $location.url(path);
        }
        
       	$rootScope.location= "";		       
        
        $rootScope.showTab = true;
        
        //Show alert
        $rootScope.alert= function(title, content){
        	$ionicPopup.alert({
			          title: title,
			          content: content
			});
        }

	})
	
	.controller('HomeCtrl', function ($scope, $rootScope, dbFactory) {	
		
		//$scope.keyword="";
		//$scope.date ="";

		//$rootScope.showLoading();
		//Check if current location is not set then redirect to location page
		$rootScope.location=dbFactory.GetLocation();
	 	if($rootScope.location == "" || $rootScope.location == undefined){
		 	$rootScope.go("/location");
		 	/*dbFactory.GetLocation().then(function(result) {
				    $rootScope.location=result
				    //$rootScope.hideLoading();
				    if($rootScope.location == "" || $rootScope.location == undefined)
				    	$rootScope.go("/location");
				    
			  }, function(reason) {
			  		//$rootScope.hideLoading();
			  		$rootScope.go("/location");
			    	//$rootScope.alert("Error", reason);			    				    		  
			  });*/

	 	}	    
	    
	 	
	    $scope.browserAll = function () {	    	
	    	var url = "b/" + $rootScope.location + "/na/na";
	        $rootScope.go("/promotions/" + url);
	    }
	    
	    $scope.searchData={};

	    $scope.searchData.keyword="";
	    $scope.searchData.date="0";

	    $scope.search= function() {
	    	var url = "s/" + $rootScope.location + "/" + $scope.searchData.keyword + "/" + $scope.searchData.date;
	    	 $rootScope.go("/promotions/" + url);
	    }
	})

    .controller('PromotionsCtrl', function ($scope, $rootScope, $stateParams, Promotions) {	    
    	
    	$rootScope.showLoading();
    	if($stateParams.searchType == "b"){ //browser All    		
    		Promotions.browseAll($rootScope.location).then(function(response) {    			
		        $scope.promotions = response.data;
		        $rootScope.hideLoading();
		    });
    	}
    	else{
    		//Search by keyword and data
    		var keyword = ($stateParams.keyword == undefined ? "" : $stateParams.keyword);
    		var avDate = ($stateParams.availability == "0" ? "" : $stateParams.availability);

    		Promotions.searchPromotion($rootScope.location, keyword, avDate).then(function(response) {    			
		        $scope.promotions = response.data;
		        $rootScope.hideLoading();
		    });
    	}
		
    })

    .controller('CouponsCtrl', function ($scope, $stateParams, $rootScope, Promotions, dbFactory) {    	
    	$rootScope.showLoading();
    	var couponsIds=[];
    	dbFactory.GetCouponIds().then(function(result) {
				couponsIds= result;				    
			  }, function(reason) {
			  		
		});

    	Promotions.coupons($stateParams.promotionId).then(function(response) {      		  			
		    $scope.coupons = response.data;
		    $rootScope.hideLoading();
		});

		$scope.isGrabbed=function(id){
			debugger
			var cls="";

			angular.forEach(couponsIds, function(item){   
				if (item.id == id) {cls="grabbed"};
			});
			return cls;
		}
    	
    })

    .controller('CouponsDetailCtrl', function ($scope, $stateParams, $rootScope, Promotions, dbFactory) {
    	$rootScope.showLoading();
    	
    	Promotions.getCouponDetail($stateParams.couponId).then(function(response) {    			
		    $scope.coupon = response.data;
		    $rootScope.hideLoading();

		    var today = new Date();
			var stratDate = new Date($scope.coupon.DateStart);
			$scope.notStarted=stratDate > today;
		});

		dbFactory.IsGrabbed($stateParams.couponId).then(function(result) {		            				
		    $scope.grabbed = (result == 1);		  		    	
		  }, function(reason) {
		    $rootScope.alert("", reason);		  
		  });
    	

		$scope.grabCoupon=function(){

			//Check if valid coupons

			dbFactory.SaveCoupon($scope.coupon);
			$rootScope.alert("Info", "Coupon grabbed successfully!");		
			$rootScope.go('/my-coupons')
		}
    })

    .controller('LocationCtrl', function ($scope, $rootScope, dbFactory) {
        $rootScope.showTab = false;
    	//$rootScope.showLoading();		
		
        /*dbFactory.GetCountryList().then(function(result) {		            	
		    $scope.countryList= result;
		  }, function(reason) {
		    $rootScope.alert("Error", reason);		  
		  });
		  
		 dbFactory.GetLocation().then(function(result) {		 	
		    $rootScope.location=result;
		    $rootScope.hideLoading();
		  }, function(reason) {
		    $rootScope.alert("Error", reason);		  
		    $rootScope.hideLoading();		    
		  });*/
				        		
        $scope.saveLocation = function () {        	
        	if(this.location == undefined || this.location == ""){
        		$rootScope.alert("", "Please select country");		  	
        		return false;
        	}

        	dbFactory.SetLocation(this.location);
        	$rootScope.location = this.location;
            $rootScope.showTab = true;
            $rootScope.go("/home");
        }
    })

    .controller('MyCouponsCtrl', function ($scope, $rootScope, dbFactory) {
    	$rootScope.showLoading();    	
    	dbFactory.GetMyCoupons().then(function(result) {		 	
		    $rootScope.coupons=result;
		    $rootScope.hideLoading();		    
		  }, function(reason) {
		  	$rootScope.hideLoading();		    
		    $rootScope.alert("Error", reason);		  		    
		  });
    })

    .controller('MyCouponDetailCtrl', function ($scope, $rootScope, $stateParams, dbFactory) {
    	$rootScope.showLoading();     			
    	dbFactory.GetMyCouponById($stateParams.couponId).then(function(result) {		 	
		    $rootScope.hideLoading();	
		    $rootScope.coupon=result;	    
		  }, function(reason) {
		  	$rootScope.hideLoading();
			$rootScope.alert("Error", reason);		  			    
		  });

    	$scope.redeemCoupon=function(){
    	   $rootScope.go("/redeem-coupon/" + $stateParams.couponId);		
    	}
    })

    .controller('RedeemCtrl', function ($scope, $rootScope, $stateParams, dbFactory) {
    	$rootScope.showLoading();    	
    	dbFactory.GetMyCouponById($stateParams.couponId).then(function(result) {		 	
		    $rootScope.coupon=result;
		    $rootScope.hideLoading();		    
		  }, function(reason) {
		  	$rootScope.hideLoading();		    
		    $rootScope.alert("Error", reason);	
		  });

    	//Remove from local saved list
    		dbFactory.Redeem($stateParams.couponId).then(function(result) {		 	
		
		  }, function(reason) {
		    $rootScope.alert("Error", reason);				      
		  });
    })

	;
    