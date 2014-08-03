





$(document).ready(function () {

    $(window).on('lightbox.ajax-content-loaded', function()
    {
        $.each($('.featherlight-content form'), function (index, form) {
            
            $(form).removeData('validator');
            $(form).removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse($(form));
        });
    });

});