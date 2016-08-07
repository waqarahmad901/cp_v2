/// <reference path="../angular.min.js" />

angular.module('carApp', [])
  .controller('checkincontroller', function ($scope, $http) {


      $scope.range = function (min, max, step) {
          step = step || 1;
          var input = [];
          for (var i = min; i <= max; i += step) {
              input.push(i);
          }
          return input;
      };

      $scope.pageClick = function (page) {
          var data = {
              currentPage: page - 1,
              veh_no: $scope.veh_no,
              token_no: $scope.token_no,
          };

          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get("/Home/GetParkedCars", config)
               .then(function (response) {
                   $scope.carTable = response.data;
               });

      }
      $scope.clearControls = function () {
          $scope.veh_no = $scope.token_no = "";
          $scope.pageClick(1);

      }
      $scope.pageClick(1);


  });