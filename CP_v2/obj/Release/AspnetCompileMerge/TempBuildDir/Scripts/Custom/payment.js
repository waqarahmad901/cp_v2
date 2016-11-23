/// <reference path="../angular.min.js" />

angular.module('carApp', [])
  .controller('paymentcontroller', function ($scope, $http) {

      $http.get(rootUrl + "Account/GetAllUsers")
    .then(function (response) {
        $scope.users = response.data;
        $scope.username = response.data[0];
    });
      $scope.paymenttype = "check";

      $scope.getPayments = function () {
          var data = {
              username: $scope.filteruser

          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "Payment/GetAllPayments", config)
            .then(function (response) {
                $scope.paymentTable = response.data;
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


      $scope.addPayment = function () {
          var data = {
              username: $scope.username,
              amount: $scope.amount,
              paymenttype: $scope.paymenttype

          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "Payment/AddPayment", config)
               .then(function (response) {
                   if (response.data == "added")
                       $scope.getPayments();
                   else {
                       alert("Error on addding payment");
                   }
               });
      }
      $scope.getPayments();

  });