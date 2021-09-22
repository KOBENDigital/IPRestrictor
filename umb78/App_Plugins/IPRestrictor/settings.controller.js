angular.module("umbraco").controller("koben.ipRestrictor.settingsController", ['$scope', 'ipRestrictorDataService', 'umbRequestHelper', 'notificationsService', function ($scope, ipRestrictorDataService, umbRequestHelper,  notificationsService) {
    $scope.savingState = 'init';
    $scope.newip = new ipConfigElement();
    $scope.required = true;

    $scope.addIp = function (ip) {
        $scope.list.push(angular.copy(ip));
        $scope.newip = new ipConfigElement();
    }

    $scope.removeIp = function (index) {
        $scope.list.splice(index, 1);
    }

    $scope.matchToIp = function () {
        $scope.newip.toValue = $scope.newip.fromValue;
    }
    

    $scope.save = function () {
        $scope.savingState = 'busy';
        umbRequestHelper.resourcePromise(ipRestrictorDataService.saveData($scope.list), "Error saving data. Check Umbraco logs for more information.")
            .then(function (response) {
                $scope.savingState = 'success'; 
                notificationsService.success("Saved", "Configuration saved.");
                $scope.addnewipform.$setPristine();
                $scope.addnewipform.alias.$setPristine();
            }, function () {
                $scope.savingState = 'error';
                notificationsService.error("Error", "There was an error saving the configuration.");
            }

        );
    }

    function ipConfigElement() {
        this.alias;
        this.fromIp;
        this.toIp;
    }

    function getConfig() {
        umbRequestHelper.resourcePromise(ipRestrictorDataService.loadData(), "Error retrieving data.")
            .then(function (response) {
                $scope.list = response;                
            });
    }

    getConfig();

}]);