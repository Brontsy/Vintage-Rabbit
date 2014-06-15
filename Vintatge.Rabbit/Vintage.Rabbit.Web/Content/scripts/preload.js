




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
}

$(document).ready(function () {

    $('.product-list-item').on('mouseover', function (event) {


    });


    AddThumbnailClickEvents();

});



$(window).on('lightbox.ajax-content-loaded', function () {

    AddThumbnailClickEvents();
});
