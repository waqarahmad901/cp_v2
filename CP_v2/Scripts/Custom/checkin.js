/// <reference path="../angular.min.js" />

angular.module('carApp', [])
  .controller('checkincontroller', function ($scope, $http) {
     
      $scope.night = false;
      $scope.veh_type = "89793349-9168-4246-96C8-7E35E99CBCEE";
      $scope.range = function (min, max, step) {
          step = step || 1;
          var input = [];
          for (var i = min; i <= max; i += step) {
              input.push(i);
          }
          return input;
      };

      $scope.checkoutTokenInfo = function (keyEvent) {
          if (keyEvent.which === 13)
          {
              var data = {
                  token_no: $scope.out_token_no
                  
              };
              var config = {
                  params: data,
                  headers: { 'Accept': 'application/json' }
              };

              $http.get(rootUrl + "Home/GetCheckoutTokenInfo", config)
                   .then(function (response) {
                       if (response.data == "null")
                           alert("No car found. ");
                       else
                       $scope.checkout = response.data;
                   });
          }
      }

      $scope.checkoutClick = function (checkout)
      {
          if ($scope.checkout == null) {
              alert("Please enter valid car no. or token no.");
              return;
          }
          var data = {
              id: checkout.Id,
              vehimage: document.getElementById("vehimage") == undefined ? null : document.getElementById("vehimage").value
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "Home/CheckOutCar", config)
               .then(function (response) {
                   $scope.checkout = null;
                   $scope.pageClick(1);
                  // alert("car checkout successfully");
               });
      }
      

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

          $http.get(rootUrl + "Home/GetParkedCars", config)
               .then(function (response) {
                   $scope.carTable = response.data;
                   if(page == 1)
                        $scope.token_no_current = $scope.carTable.token_no_current;
               });

      }
      $scope.clearControls = function () {
          $scope.veh_no = $scope.token_no = "";
          $scope.pageClick(1);

      }
   
      $scope.printToken = function () {
          var data = {
              veh_no: $scope.veh_no_token,
              veh_type: $scope.veh_type,
              night: $scope.night,
              vehimage: document.getElementById("vehimage") == undefined ? null : document.getElementById("vehimage").value
          };

          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "Home/PrintTokenForCar", config)
               .then(function (response) {
                   $scope.token_no_current = response.data;
                   $("#carimg").attr("src", rootUrl +"images/car-img.jpg");
                   $scope.veh_no_token = "";
                   $scope.pageClick(1);
               });
      }
      $scope.reprintToken = function (carId)
      {
          var data = {
              id: carId
             
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "Home/RePrintTokenForCar", config)
               .then(function (response) {
                   if (response.data)
                       alert("ticket genrated successfully...");
               });
      }
      $scope.pageClick(1);

  });