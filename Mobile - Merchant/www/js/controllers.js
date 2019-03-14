'use strict';

angular.module('mobireward.controllers', [])

    .controller('MainCtrl', function ($scope, $rootScope, $location, $window, $ionicPopup, $ionicLoading, $ionicPlatform, $cordovaNetwork) {				
		//Show loading

		$rootScope.showLoading = function() {
		    $ionicLoading.show({
		      template: 'Loading...'
		    });
		};

		//Hide Loading
		$rootScope.hideLoading = function(){
		    $ionicLoading.hide();
		};

		//Back Page nav
        $rootScope.back = function () { 
        	          
            $window.history.back();
        }

        //Navigate page
        $rootScope.go = function (path) {            
            $location.url(path);
        }

        //Show alert
        $rootScope.alert= function(title, content){
        	$ionicPopup.alert({
			          title: title,
			          content: content
			});
        }

		//Check network connection
        $rootScope.isOnline = function (path) {            
            var type = $cordovaNetwork.getNetwork();
  			var hasConn = $cordovaNetwork.isOnline();
  			if(!hasConn)
  				$rootScope.alert("Error", "Network is not available");

  			return hasConn;
        }        

        $rootScope.userName ="";
	})
	
	.controller('HomeCtrl', function ($scope, $rootScope, Promotions) {	

	 	$scope.user={};

	    $scope.login = function () {	    		    	
	    	if($scope.user.userName == "" || $scope.user.userName == undefined)
	    	{
	    		$rootScope.alert("Info", "Please enter user name");	
	    		return false;
	    	}

	    	if($scope.user.password == "" || $scope.user.password == undefined)
	    	{
	    		$rootScope.alert("Info", "Please enter password");	
	    		return false;
	    	}

			$rootScope.showLoading();

	    	Promotions.login($scope.user.userName, $scope.user.password).then(function(response) { 		    		
	    			$rootScope.hideLoading();

			        if( response.data == "true"){
			        	$rootScope.userName =$scope.user.userName;
			        	$rootScope.go("/scan");
			        }			       
			        else{
			        	$rootScope.alert("Info", "Invalid user name or password");
			        }

		    });	        
	    }	    
	})

    .controller('ScanCtrl', function ($scope, $rootScope, $stateParams, $cordovaBarcodeScanner, $ionicPopup, Promotions) {	    
    	$scope.logout = function () {            		
    		$rootScope.go("/home");
    	}

    	$scope.scan = function () {            		
    		$cordovaBarcodeScanner.scan().then(function(result) {				    					
    					$ionicPopup.confirm({
				          title: 'Confirm',
				          content: 'Are you sure you want to redeem?'
				        }).then(function(res) {	        	
				          if(res) {	        								
							Promotions.isValidCouponByMerchant(result.text, $rootScope.userName ).then(function(response) { 
					    		
						        if( response.data + "" == "true"){
						        	//Grab the coupon
						        	$rootScope.alert("Info", "Coupon redeemed successfully!");			
						        }			       
						        else{
						        	$rootScope.alert("Info", "Invalid coupon");
						        }

					    	});
				          } 
				        });				    	
				  }, function(err) {
				  });	

    		
		}
    })
 
	;
    