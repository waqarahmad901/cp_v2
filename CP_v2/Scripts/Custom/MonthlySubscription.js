/// <reference path="../angular.min.js" />


angular.module('carApp', [])
  .controller('monthlySubscriptionController', function ($scope, $http) {
      $scope.getSubscriptions = function (carno,name) {
          var data = {
              carno : carno,
              name : name
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "MonthlySubscription/GetAllSubscriptions", config)
            .then(function (response) {
                $scope.subscriptionTable = response.data;
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

      $scope.addSubscription = function () {
          var data = {
              carbike: $scope.carbike,
              cnic: $scope.cnic,
              mobilenumber: $scope.mobilenumber,
              amount: $scope.amount,
              month: $scope.month,
              ownername: $scope.ownername

          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "MonthlySubscription/AddSubscription", config)
              .then(function (response) {
                  if (response.data == "added")
                      $scope.getSubscriptions("", "");
                  else {
                      alert("Error on addding payment");
                  }
              });
      }

      $scope.search = function ()
      {
          $scope.getSubscriptions($scope.carnosearch, $scope.ownernamesearch);
      }

      $scope.getSubscriptions("","");

  });