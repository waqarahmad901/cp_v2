/// <reference path="../angular.min.js" />
/// <reference path="util.js" />

angular.module('carApp', [])
  .controller('manageUser', function ($scope, $http) {
     
      $scope.pageClick = function (page) {
          var data = {
             
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };

          $http.get(rootUrl + "Account/GetManageUsers", config)
               .then(function (response) {
                   $scope.userTabel = response.data;
               });
      }
      $scope.add = function () {
          window.location.href = rootUrl + 'Account/UserManagement/';
      }
      $scope.delete = function (id) {
          if (!confirm("Are you sured you want top delete this user"))
              return;
          var data = {
              id: id
          };
          var config = {
              params: data,
              headers: { 'Accept': 'application/json' }
          };
          $http.get(rootUrl + "Account/DeleteUser", config)
            .then(function (response) {
                $scope.pageClick(1);


            });

      }

      $scope.edit = function (id) {
          window.location.href = rootUrl +'Account/UserManagement/' + id;
      }

      $scope.pageClick(1);
  });