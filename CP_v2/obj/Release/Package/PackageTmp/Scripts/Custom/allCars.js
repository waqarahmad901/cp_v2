/// <reference path="../angular.min.js" />
/// <reference path="util.js" />

angular.module('carApp', [])
  .controller('allCars', function ($scope, $http) {
      $scope.selected = [];
      $scope.pageClick = function (page) {

          var data = {
              currentPage: page - 1,
              recordPerPage: 100,
              veh_no: $scope.veh_no,
              parked: $scope.parked,
              token_no: $scope.token_no,
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "Home/GetParkedCars", config)
               .then(function (response) {
                   $scope.carTable = response.data;
                   if (page == 1)
                       $scope.token_no_current = $scope.carTable.token_no_current;
                   TotalParkedCars = $scope.carTable.TotalParkedCars;
               }); 
      }

      $scope.CheckoutCars = function () {
         
          var selected = $scope.selected;
          $scope.selectedIds = "";
         
          for (var key in selected) {
              if (selected.hasOwnProperty(key) && selected[key]) {
                  $scope.selectedIds += "," + key
              }
          }

          var data = {
              selectedIds : $scope.selectedIds
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "Home/CheckoutCars", config)
              .then(function (response) {
                  $scope.pageClick(1);
              });
      }

      $scope.range = function (min, max, step) {
          step = step || 1;
          var input = [];
          for (var i = min; i <= max; i += step) {
              input.push(i);
          }
          return input;
      };

      $scope.pageClick(1);


  });