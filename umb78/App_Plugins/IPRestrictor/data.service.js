angular.module('umbraco.resources').factory('ipRestrictorDataService', ['$http', function ($http) {
    var controllerBase = "/umbraco/backoffice/KobenIPRestrictor/IpRestrictor/";
    var loadData = function () {
        return $http.get(controllerBase + "loadData");
    };

    var saveData = function (data) {
        return $http.post(controllerBase + "saveData", JSON.stringify(data));
    };

    return {
        loadData: loadData,
        saveData: saveData
    }
}])