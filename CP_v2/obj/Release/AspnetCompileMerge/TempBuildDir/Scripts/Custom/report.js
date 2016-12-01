/// <reference path="../angular.min.js" />

angular.module('carApp', [])
  .controller('reportController', function ($scope, $http) {
      var today = new Date();
      var dd = today.getDate();
      var mm = today.getMonth() + 1; //January is 0!

      var yyyy = today.getFullYear();
      if (dd < 10) {
          dd = '0' + dd
      }
      if (mm < 10) {
          mm = '0' + mm
      }
      var today = dd + '/' + mm + '/' + yyyy;

      $scope.from = today;

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
                $scope.shifts = response.data;
            });
      }
     
      $scope.searchReport();

  });