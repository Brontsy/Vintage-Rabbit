﻿


function AddDatePickerEvents()
{
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

    $('.datepicker-trigger').datepicker({
        format: 'dd MM yyyy',
        daysOfWeekDisabled: '1,2,3,4,5',
        startDate: new Date(),
        autoclose: true
    }).on('changeDate', function (ev) {

        $('.check-availability-form').attr('action', $('.check-availability-form').data('checkAvailability'));
        $('.check-availability').removeClass('unavailable').removeClass('available').removeClass('in-cart');
    });

    $('.check-availability-form').on('submit', function (event) {

        event.preventDefault();
        var $this = $(this);
            var button = $(this).find('.btn.check-availability-button');

            $(this).data('check-availability', $(this).attr('action'));

            button.data('button-text', $(button).html());
            $(button).html('Checking');

            $.ajax({
                url: $(this).attr('action'),
                data: $(this).serialize(),
                type: 'GET',
                success: function (response) {

                    $('.availability-check').html(response);

                    $(window).trigger('availability-check');
                }
            });
      
    });


    $(window).on('AddToCart', function (json) {
        //$('.add-hire-product-to-cart').find('button').addClass('hidden');
        //$('.add-hire-product-to-cart').find('.available').addClass('hidden');
        //$('.add-hire-product-to-cart').find('.added-to-cart').removeClass('hidden');
        $('.add-to-cart-form').addClass('in-cart');
    });
}


$(document).ready(function () {

    AddDatePickerEvents();
});




$(window).on('lightbox.ajax-content-loaded, availability-check', function () {

    AddDatePickerEvents();
});
