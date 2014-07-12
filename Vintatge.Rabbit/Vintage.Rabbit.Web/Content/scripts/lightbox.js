


function AddAjaxLighboxEvents()
{
    $('.ajax-lightbox').featherlight(
    {
        targetAttr: 'href',
        type: 'ajax',
        variant: 'lightbox-content',
        afterOpen: function () {

            AddClickEvents();

            $(window).trigger('lightbox.ajax-content-loaded');
        }
    });
}


$(document).ready(function () {

    AddAjaxLighboxEvents();
});

$(window).on('availability-check', function () {

    AddAjaxLighboxEvents();
});

$(window).on('lightbox.ajax-content-loaded', function ()
{
    $('.featherlight-content .ajax-lightbox').on('click', function (e) {

        e.preventDefault();
        var url = $(this).attr('href');
        var content = $('.featherlight-content');
        var width = content.outerWidth();
        var height = content.outerHeight();

        content.css('min-width', width);
        content.css('min-height', height);

        $('.featherlight-inner').fadeOut(500, function () {

            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'html',
                success: function (response) {

                    $('.featherlight-inner').html(response);
                    $('.featherlight-inner').fadeIn(500);

                    content.css('min-width', '0px');
                    content.css('min-height', '0px');

                    $(window).trigger('lightbox.ajax-content-loaded');
                }
            });
        });

    });
})