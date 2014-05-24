





$(document).ready(function () {

    $('.shopping-cart').on('mouseover', function (event) {

        $(this).parents('.row').addClass('shopping-cart-hover');
    });
    $('.shopping-cart').on('mouseleave', function (event) {
        $(this).parents('.row').removeClass('shopping-cart-hover');
    });

    $('.product-list-item').on('mouseover', function (event) {


    });

});