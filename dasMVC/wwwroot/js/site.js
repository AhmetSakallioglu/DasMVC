toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

jQuery(document).ready(function ($) {

    "use strict";

    $(".loader").delay(1000).fadeOut("slow");
    $("#overlayer").delay(1000).fadeOut("slow");

    var fullHeight = function () {

        $('.js-fullheight').css('height', $(window).height());

        $(window).on('resize', function () {
            $('.js-fullheight').css('height', $(window).height());
        });

    };

    fullHeight();

});