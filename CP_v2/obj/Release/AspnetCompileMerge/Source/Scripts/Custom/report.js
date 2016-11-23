/// <reference path="../angular.min.js" />

angular.module('carApp', [])
  .controller('reportController', function ($scope, $http) {


      $scope.searchReport = function () {
          var data = {
              from: $scope.from,
              to : $scope.to

          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "UserReport/searchReport", config)
            .then(function (response) {
                $scope.paymentTable = response.data;
            });
      }
     
      $scope.searchReport();

  });