/// <reference path="../angular.min.js" />

angular.module('carApp', [])
  .controller('paymentcontroller', function ($scope, $http) { 
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
                $scope.payments = response.data;
            });
      }

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
                   else
                   {
                       alert("Error on addding payment");
                   }
               });
      }
      $scope.getPayments();

  });