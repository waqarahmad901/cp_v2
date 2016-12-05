/// <reference path="../angular.min.js" />


angular.module('carApp', [])
  .controller('monthlySubscriptionController', function ($scope, $http) {
      $scope.currentEdit = "";
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

      $scope.edit = function (id)
      {
          $scope.currentEdit = id;
          var data = {
              id : id
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "MonthlySubscription/GetSubcriptionbyId", config)
            .then(function (response) {
                $scope.carbike = response.data.carno;
                $scope.cnic = response.data.cninc;
                $scope.mobilenumber = response.data.mobileno;
                $scope.amount = response.data.amount;
                $scope.month = response.data.month;
                $scope.ownername = response.data.ownername;
            });
      }
      $scope.clear = function ()
      {
          $scope.carnosearch = "";
          $scope.ownernamesearch = "";
          $scope.getSubscriptions("", "");
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
              id :   $scope.currentEdit,
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
                  if (response.data == "added") {
                      $scope.getSubscriptions("", "");
                      $scope.carbike = "";
                      $scope.cnic = "";
                      $scope.amount= "",
                      $scope.mobilenumber = "";
                      $scope.month = "";
                      $scope.ownername = "";
                  }
                  else {
                      alert("Error on addding payment");
                  }
                  $scope.currentEdit = "";
              });
      }

      $scope.search = function ()
      {
          $scope.getSubscriptions($scope.carnosearch, $scope.ownernamesearch);
      }

      $scope.getSubscriptions("","");

  });