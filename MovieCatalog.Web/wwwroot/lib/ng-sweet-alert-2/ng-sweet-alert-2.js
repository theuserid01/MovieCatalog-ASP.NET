/**
 * @description directive for sweet alert
* @author Karl Vaillant
* @createDate 08/07/2016
* @version 0.0.1
*/

(function () {
    'use strict';

    // Check we have sweet alert js included
    if (angular.isUndefined(window.swal)) {
        throw "Please inlcude sweet alert 2 js and css from https://github.com/limonte/sweetalert2";
    }

    angular
        .module('ng-sweet-alert-2', [])
        .directive('sweetalert2', sweetalert2);

    sweetalert2.$inject = ['$parse'];

    /* @ngInject */
    function sweetalert2($parse) {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            link: link
        };
        return directive;

        function link(scope, element, attrs, controller) {
            var sweetElement = angular.element(element);
            sweetElement.click(function () {
                var sweetOptions = scope.$eval(attrs.sweetOptions);
                var sweetSuccessOption = scope.$eval(attrs.sweetSuccessOption);
                var sweetDismissOption = scope.$eval(attrs.sweetDismissOption);

                swal(sweetOptions).then(function (result) {
                    if (sweetSuccessOption) swal(sweetSuccessOption);
                    if (attrs.sweetOnConfirm) scope.$evalAsync(attrs.sweetOnConfirm);
                    // Add support for result
                }, function (dismiss) {
                    // dismiss can be 'cancel', 'overlay', 'close', 'timer'
                    if (sweetDismissOption) swal(sweetDismissOption);
                    if (dismiss === 'cancel' && attrs.sweetOnCancel) scope.$evalAsync(attrs.sweetOnCancel);
                    if (dismiss === 'overlay' && attrs.sweetOnOverlay) scope.$evalAsync(attrs.sweetOnOverlay);
                    if (dismiss === 'close' && attrs.sweetOnClose) scope.$evalAsync(attrs.sweetOnClose);
                    if (dismiss === 'timer' && attrs.sweetOnTimer) scope.$evalAsync(attrs.sweetOnTimer);
                }).finally(function () {
                });
            });
        }
    }
})();
