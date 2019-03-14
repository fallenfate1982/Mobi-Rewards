'use strict';

angular.module('mobireward.factory', [])

.service('dbFactory', function ($q, $http, $rootScope, $ionicPlatform) {
        var countries=[];
        var location="";
 		var deferred = $q.defer();        
        var dbFactory = {};       
		var baseUrl = "http://182.18.157.106:8076/api/MobiRewardServices/";
		var db = window.openDatabase("Mobireward", "1.0", "Mobireward", 200000);       
		
		//populateDB();
		 /*$ionicPlatform.ready(function() {		    
		    populateDB();
		  });*/
		
		dbFactory.PrepareDB = function(){			        	
           populateDB();
        };
		
		dbFactory.GetCountryList = function(){			        	
            return getCountries();
        };
        
        dbFactory.GetLocation = function(){        	       	
            return window.localStorage['loc']; //getLocation();
        };
        
        dbFactory.SetLocation = function(location){        	
        	 window.localStorage['loc'] = location;      
            //setCurrentLocation(location);
        };

		dbFactory.IsGrabbed = function(couponId){       			
			var sql = 'SELECT * FROM MYCOUPONS WHERE id=' + couponId;
		  return promisedQuery(sql, boolResultHandler, defaultErrorHandler);
        };        

        dbFactory.GetCouponIds = function(){       			
			var sql = 'SELECT id FROM MYCOUPONS';
		  return promisedQuery(sql, defaultResultHandler, defaultErrorHandler);
        };        

        dbFactory.Redeem = function(couponId){       			
			var sql = 'DELETE FROM MYCOUPONS WHERE id=' + couponId;
		  return promisedQuery(sql, boolResultHandler, defaultErrorHandler);
        };        

        dbFactory.GetMyCoupons = function(couponId){       			
			var sql = 'SELECT * FROM MYCOUPONS';
		  return promisedQuery(sql, defaultResultHandler, defaultErrorHandler);
        };        

        dbFactory.GetMyCouponById = function(couponId){       			
			var sql = 'SELECT * FROM MYCOUPONS WHERE id=' + couponId;
		  return promisedQuery(sql, singleResultHandler, defaultErrorHandler);
        };        

		/*dbFactory.SaveCoupon = function(couponId, coupontitle, couponImage, description, couponName, endDate, barCodeImage, qrCodeImage, termsAndCondition, qrCode){*/
		dbFactory.SaveCoupon = function(coupon){
			db.transaction(
		            function(tx) {
						        	
		            	var sql = 'INSERT OR REPLACE INTO MYCOUPONS (id , couponTitle, couponImage, description, couponName, dateEnd, barCodeImage, qrCodeImage, terms, qrCode)'
							+ 'VALUES(' + coupon.CouponId + ', "'+  coupon.CouponTitle + '","' + coupon.CouponImage +'","' + coupon.CouponDescription +'","' + 
								coupon.CouponName + '", "' +  coupon.DateEnd + '","'+ coupon.BarCodeImage +'","' + coupon.QRCodeImage + '","' + coupon.TermsAndCondition +'","' + coupon.QRCode + '")';

						//log(sql);
						tx.executeSql(sql);
		            },
		            txErrorHandler,
		            function() {
		                //log('Location set : ' + countryName);		               
		            }
		        );
		}

		function log(message){
			console.log(message);
		}
		
		function populateDB(){
			createTable();
            //synchCountry();	
		}
		
		function getLocation(){
		  return promisedQuery('SELECT * FROM LOCATION', locationResultHandler, defaultErrorHandler);
		}
		
		function getCountries(){
		  return promisedQuery('SELECT * FROM COUNTRY', defaultResultHandler, defaultErrorHandler);
		}
		
		function locationResultHandler(deferred) {
		  return function(tx, results) {
		    var len = results.rows.length;
		    var output_results;
		
		    for (var i=0; i<len; i++){		    	
		      output_results= results.rows.item(i).locationName;
		    }
		
		    deferred.resolve(output_results);  
		  }  
		}

		function boolResultHandler(deferred) {
		  return function(tx, results) {
		  	
		    var hasValue =1;
		    if(results == null  || results == undefined || results.rows == null || results.rows == undefined || results.rows.length == 0)
		    	hasValue=0;
		
		    deferred.resolve(hasValue);  
		  }  
		}
		
		function defaultResultHandler(deferred) {
		  return function(tx, results) {
		    var len = results.rows.length;
		    var output_results = [];
		
		    for (var i=0; i<len; i++){		    	
		      output_results.push(results.rows.item(i));
		    }
		
		    deferred.resolve(output_results);  
		  }  
		}

		function singleResultHandler(deferred) {
		  return function(tx, results) {
		    var len = results.rows.length;
		    var result;
		
		    for (var i=0; i<len; i++){		    	
		      result = results.rows.item(i);
		    }
		
		    deferred.resolve(result);  
		  }  
		}
		
		function defaultErrorHandler(err) {
			deferred.reject();
		  //alert("Error processing SQL: " + err.code);
		}
		
		function promisedQuery(query, successCB, errorCB) {
			if(db == null || db== undefined)
		  		db = window.openDatabase("Mobireward", "1.0", "Mobireward", 200000);

		  var deferred = $q.defer();
		  db.transaction(function(tx){
		    tx.executeSql(query, [], successCB(deferred), errorCB);      
		  }, errorCB);
		  return deferred.promise;  
		}
		
		
		function createTable () {			
			if(db == null || db== undefined)
				db = window.openDatabase("Mobireward", "1.0", "Mobireward", 200000);

	        db.transaction(
		            function(tx) {

		            	//tx.executeSql("DROP TABLE IF EXISTS COUNTRY");		                
		            	//tx.executeSql("DROP TABLE IF EXISTS LOCATION");	
		            	//tx.executeSql("DROP TABLE IF EXISTS MYCOUPONS");


		                //var sql ='CREATE TABLE IF NOT EXISTS COUNTRY (id, countryName, countryCode)';
		                //tx.executeSql(sql);		                

		                var sql = 'CREATE TABLE IF NOT EXISTS LOCATION (id, locationName)';		                
		                tx.executeSql(sql);		                

		                sql= 'CREATE TABLE IF NOT EXISTS MYCOUPONS (id , couponTitle, couponImage, description, couponName, dateEnd, barCodeImage, qrCodeImage, terms, qrCode)';
		                tx.executeSql(sql);
		                


		            },
		            txErrorHandler,
		            function() {
		                //log('Country and Location table created successfully');		               
		            }
		        );
		}

		function setCurrentLocation (countryName) {			
	        db.transaction(
		            function(tx) {
		            	var sql ="DELETE FROM LOCATION";
		            	tx.executeSql(sql);
		                sql='INSERT OR REPLACE INTO LOCATION (id, locationName) VALUES (1, "' +  countryName + '")';		                
		                tx.executeSql(sql);		                
		            },
		            txErrorHandler,
		            function() {
		                //log('Location set : ' + countryName);		               
		            }
		        );
		}
		
		function getCurrentLocation () {			
	        db.transaction(
	            function(tx) {
	            	location="";
	                var sql = "SELECT * FROM LOCATION";	                
	                tx.executeSql(sql, this.txErrorHandler,
	                    function(tx, results) {
	                    	log(results);
	                        var len = results.rows.length;	                                              
	                        for (var i=0; i < len; i = i + 1) {
	                        	//log(results.rows.item(i));
	                        	location = results.rows.item(i).locationName;
	                        }	         
	                        	                        
            				deferred.resolve(location);              
	                    }
		              );
		            }
		        );
		}
		
		function synchCountry(){
			//("synchCountry");
			var url = baseUrl + "GetAllCountry";	        	
			//log("calling server : "  + url);   	        	        	
			$http.get(url).success(function(data) {
				angular.forEach(data, function(item){
					//debugger
					db.transaction(
			            function(tx) {
			              //log(item.CountryId + " : " + item.CountryName);
			              tx.executeSql('INSERT OR REPLACE INTO COUNTRY (id, countryName, countryCode) VALUES ("' + item.CountryId + '","' + item.CountryName + '", "' + item.CountryCode + '")');		                
			            },
			            txErrorHandler,
			            function() {
			                //log('Record inserted successfully');		               		
			            }
			        );					
				});       		 
			    }).error(function(data, status) {						    				        
			});
		    
		}
			
		
		
		function txErrorHandler(tx) {
			deferred.reject();
        	//log(tx.message);
    	}
		
		
		return dbFactory; 
});
