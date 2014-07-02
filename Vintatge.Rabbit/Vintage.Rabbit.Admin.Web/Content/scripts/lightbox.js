





$(document).ready(function () {


    $('.ajax-lightbox').featherlight(
        {
            targetAttr: 'href',
            type: 'ajax',
            variant: 'lightbox-content',
            afterOpen: function()
            {
                $(window).trigger('lightbox.ajax-content-loaded');
            }
        });

});