

function AddThemeImageEvents() {
 
    $('.marker').on('click', function (e) {

        e.preventDefault();

        var themeImage = $(this).parents('.theme-image');
        var guid = $(this).data('product-guid');
        var productId = $(this).data('product-id');

        themeImage.removeClass('show-left');
        themeImage.removeClass('show-right');
        themeImage.removeClass('show-product-left');
        themeImage.removeClass('show-product-right');

        //if (themeImage.hasClass('animate')) {

        //    themeImage.removeClass('animate');
        //}
        //else {
            var parentOffset = themeImage.parent().offset();
            //or $(this).offset(); if you really just want the current element's offset
            var relX = e.pageX - parentOffset.left;
            var relY = e.pageY - parentOffset.top;

            var width = $(themeImage).parent().width();

            var pos = 'left';
            if (relX < width / 2) {
                pos = 'right';
            }

            themeImage.toggleClass('show-' + pos);
            themeImage.toggleClass('animate');
            
            setTimeout(function () {

                themeImage.toggleClass('show-product-' + pos);
            }, 100);
            $('.side-panel').html('').append($('.product-' + productId).clone(true, true));
        //}
    });

    $('.close-product-info').on('click', function () {

        var themeImage = $('.theme-image');

        themeImage.removeClass('animate');
        themeImage.removeClass('show-left');
        themeImage.removeClass('show-right');
        themeImage.removeClass('show-product-left');
        themeImage.removeClass('show-product-right');
    });
}

$(window).on('lightbox.ajax-content-loaded', function () {

    AddThemeImageEvents();

    if($('.theme-added-to-cart').length > 0)
    {
        $(window).trigger('AddToCart');
    }
});


