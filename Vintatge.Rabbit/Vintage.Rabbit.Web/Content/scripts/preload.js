




function AddThumbnailClickEvents() {
    $('.thumbnails img').on('click', function (event) {
        var url = $(this).data('full-source');

        $('.large-image').css('height', $('.large-image').height());

        $('.large-image img').fadeOut(500, function () {

            var img = new Image();

            $(img).load(function () {

                $('.large-image img').replaceWith($(this));
                $('.large-image').animate({
                    height: $(this).height()
                }, 400, function () { });

            })
            .attr('src', url).addClass('large-image');

        })
    });


    $('.change-party-date').on('click', function (event) {
        event.preventDefault();
        $('.choose-party-date').removeClass('hidden');
        $('.available-text').addClass('hidden');
    });

    $('.change-party-date-form').on('submit', function () {
        $(this).find('.btn').html('Changing');
        $(this).css('opacity', '0.5');
    });
}

$(document).ready(function () {

    $('.product-list-item').on('mouseover', function (event) {


    });

    $('.product-list .product-list-item').on('click', function (e) {

        if ($(e.target).prop("tagName") != 'A' && $(this).find('a').length > 0) {
            window.location = $(this).find('a').attr('href');
        }
    });

    $('.product-list .product-list-item').on('click', function (event) {

        event.stopPropagation();
    });


    AddThumbnailClickEvents();

});



$(window).on('lightbox.ajax-content-loaded', function () {

    AddThumbnailClickEvents();
});
