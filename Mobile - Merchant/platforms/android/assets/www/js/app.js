angular.module('mobireward', [

	'ionic',         
    'ngCordova',
    'mobireward.servies',
    'mobireward.controllers'    
])
.config(function($stateProvider, $urlRouterProvider) {

  $stateProvider
    .state('home', {
      url: "/home",
      templateUrl: "partial/home.html",
      controller: 'HomeCtrl'
    })

    .state('scan', {
        url: "/scan",
        templateUrl: "partial/scan.html",
        controller: 'ScanCtrl'
    })

    ;

   $urlRouterProvider.otherwise("/home");

})

 
