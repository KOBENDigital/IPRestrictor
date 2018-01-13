angular.module("umbraco").controller("koben.ipRestrictor.settingsController", ['ipRestrictorDataService', 'umbRequestHelper', function (ipRestrictorDataService, umbRequestHelper) {
    var vm = this;
    vm.savingState = 'init';
    vm.newip = new ipConfigElement();
    vm.required = true;

    vm.addIp = function (ip) {
        vm.list.push(ip);
        vm.addnewipform.$setPristine(true)
    }

    vm.removeIp = function (index) {
        vm.list.splice(index, 1);
    }

    vm.matchToIp = function () {
        vm.newip.toValue = vm.newip.fromValue;
    }

    vm.save = function () {
        vm.savingState = 'busy';
        umbRequestHelper.resourcePromise(ipRestrictorDataService.saveData(vm.list), "Error saving data. Check Umbraco logs for more information.")
            .then(function (response) {
                vm.savingState = 'success';                
            }, function () {
                vm.savingState = 'error';
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
                vm.list = response;
            });
    }

    getConfig();

}]);