/// <reference path="../angular.min.js" />


angular.module('carApp', [])
  .controller('monthlySubscriptionController', function ($scope, $http) {
      $scope.currentEdit = "";
      var date = new Date();
      $scope.monthsearch = date.getMonth() + 1;
      $scope.getSubscriptions = function (page) {
          var data = {
              carno: $scope.carnosearch,
              name: $scope.ownernamesearch,
              month: $scope.monthsearch,
              page: page
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

      $scope.add = function () {
          $scope.carbike = "";
          $scope.cnic = "";
          $scope.amount = "",
          $scope.mobilenumber = "";
          $scope.month = "";
          $scope.ownername = "";

          $scope.currentEdit = "";
      }
      $scope.delete = function (id) {
          if (!confirm("Are you sured you want top delete this car"))
              return;
          var data = {
              id: id
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "MonthlySubscription/DeleteSubcriptionbyId", config)
            .then(function (response) {
                $scope.getSubscriptions(1);


            });

      }

      $scope.pageClick = function (page) {
          $scope.getSubscriptions(page);
      }
      $scope.edit = function (id) {
          $scope.currentEdit = id;
          var data = {
              id: id
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
      $scope.clear = function () {
          $scope.carnosearch = "";
          $scope.ownernamesearch = "";
          $scope.getSubscriptions(1);
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
              id: $scope.currentEdit,
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
                      $scope.getSubscriptions(1);
                      $scope.carbike = "";
                      $scope.cnic = "";
                      $scope.amount = "",
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

      $scope.search = function () {
          $scope.getSubscriptions(1);
      }

      $scope.getSubscriptions(1);

  });