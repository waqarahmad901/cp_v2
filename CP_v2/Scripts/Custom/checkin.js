/// <reference path="../angular.min.js" />
/// <reference path="util.js" />

angular.module('carApp', [])
  .controller('checkincontroller', function ($scope, $http) {
     
      $scope.night = false;
      $scope.veh_type = "89793349-9168-4246-96C8-7E35E99CBCEE";
      $scope.checkout = new Object();
      $scope.checkout.time = "00 Hours 00 Minutes";
      $scope.checkout.totalAmount = "0.00";
      $scope.checkout.isMonthly = true;
      $scope.checkout.isPaid = true;

      $scope.range = function (min, max, step) {
          step = step || 1;
          var input = [];
          for (var i = min; i <= max; i += step) {
              input.push(i);
          }
          return input;
      };
      var isCarFound = false;
      var oldToken = '';

      function clearToken()
      {
          $scope.out_token_no = "";
          clearTimeout(clearToken,0);
      }

      $scope.checkoutTokenInfo = function (keyEvent) {
        //  debugger;
          //if (keyEvent.which === 27 || keyEvent.keyCode === 27)
          //{ 
          //    $scope.checkout = new Object();
          //    $scope.checkout.time = "00 Hours 00 Minutes";
          //    $scope.checkout.totalAmount = "0.00";
          //    $scope.checkout.isPaid = true;
          //    $scope.checkout.isMonthly = false;

          //    setTimeout(clearToken, 0);
           
          //}
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
                       else {
                           isCarFound = true;
                           $scope.checkout = response.data;
                           oldToken = $scope.out_token_no;
                           if ($scope.checkout.isPaid) {
                               $scope.checkout.time = "00 Hours 00 Minutes";
                               $scope.checkout.totalAmount = "0.00";
                           }
                           if ($scope.checkout.isMonthly)
                           {
                               $scope.checkout.totalAmount = "0.00";
                           }
                       }
                   });

              if (isCarFound && oldToken == $scope.out_token_no)
              {
                  $scope.checkoutClick($scope.checkout);
                  isCarFound = false;
              }

             
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
                   $scope.checkout = new Object()
                   $scope.out_token_no = "";
                   $scope.checkout.time = "00 Hours 00 Minutes";
                   $scope.checkout.totalAmount = "0.00";
                   $scope.checkout.isPaid = true;
                   $scope.checkout.isMonthly = false;
                   $scope.TotalParkedCars = response.parkinCars;
                     $scope.pageClick(1);

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
                   TotalParkedCars = $scope.carTable.TotalParkedCars;
               });
          

      }

      function GetParkedCarCount()
      {
          console.log("tedst");
          $http.get(rootUrl + "Home/GetTotalCarCount")
             .then(function (response) {
                
                 $scope.carTable.TotalParkedCars = response.data;
             });

      }

      setInterval(GetParkedCarCount, 10000);


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
                   $scope.token_no_current = response.data.token;
                   $("#carimg").attr("src", rootUrl +"images/car-img.jpg");
                   $scope.veh_no_token = "";
                   $('#loaderFrame').attr('src', 'print.html?car=' + response.data.car_no + "&token=" + response.data.token + "&night=" + response.data.nightly + "&date=" + response.data.parkin_date + ' ' + response.data.parkin_time);
                   $('#loaderFrame').load(function () {
                       var w = (this.contentWindow || this.contentDocument.defaultView);
                       w.print();
                   });

                  // $("#recipt").printThis();
                 //  makePDF(response.data);
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
                   if (response.data) {
                       $('#loaderFrame').attr('src', 'print.html?car=' + response.data.car_no + "&token=" + response.data.token + "&night=" + response.data.nightly + "&date=" + response.data.parkin_date + ' ' + response.data.parkin_time);
                       $('#loaderFrame').load(function () {
                           var w = (this.contentWindow || this.contentDocument.defaultView);
                           w.print();
                       });
                   }
               });
      }
      $scope.pageClick(1);

  });