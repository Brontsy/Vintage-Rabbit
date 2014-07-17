
function ReloadOrderSummary() {
    var url = $('.order-summary').data('reload-url');
    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {
            $('.order-summary').replaceWith(response);
            $(window).trigger('order-summary-loaded');
        }
    });
}

function AddApplyDiscountEvents() {
    $('.apply-discount-form').on('submit', function (event) {

        event.preventDefault();
        var $this = $(this);
        $(this).find('button').html('Applying');
        $.ajax({
            url: $(this).attr('action'),
            data: $(this).serialize(),
            type: 'POST',
            dataType: 'json',
            success: function (response) {

                if (response.Status == 'Not Found') {
                    $this.find('.error').html('Sorry we were unable to find any loyalty card matching your discount number.').removeClass('hidden');
                    $(this).find('button').html('Apply');
                }

                if (response.Status == 'Expired') {
                    $this.find('.error').html('Sorry the loyalty card has expired.').removeClass('hidden');
                    $(this).find('button').html('Apply');
                }

                if (response.Status == 'Used') {
                    $this.find('.error').html('Sorry the loyalty card has already been used.').removeClass('hidden');
                    $(this).find('button').html('Apply');
                }

                if (response.Status == 'Applied') {
                    $(window).trigger('discount-applied');
                }
            }
        });
    });
}





$(document).ready(function () {

    $('.guest-form').on('submit', function (event) {

        var selected = $(this).find('input[name=register]:checked').val();

        if(selected == 'register')
        {
            event.preventDefault();
            $('.checkout-control-container').addClass('register');
        }
        else
        {
            $('.checkout-control-container').removeClass('register');
        }
    });

    $('#credit-card').on('change', function () {

        if ($(this).is(':checked')) {
            $('.credit-card-form').removeClass('hidden');
            $('.paypal-form').addClass('hidden');
        }
    });


    $('#paypal').on('change', function () {

        if ($(this).is(':checked')) {
            $('.credit-card-form').addClass('hidden');
            $('.paypal-form').removeClass('hidden');
        }
    });



    $('#IsDropoff, #IsPickup').on('change', function () {
        
        if ($('#IsDropoff').is(':checked') || $('#IsPickup').is(':checked')) {
            $('.deliver-field').removeClass('hidden');
            $('.pickup-dropoff-yourself').addClass('hidden');
        }
        else {
            $('.deliver-field').addClass('hidden');
            $('.pickup-dropoff-yourself').removeClass('hidden');
        }
    });

    $(window).on('discount-applied', function () {

        ReloadOrderSummary();
    });

    $(window).on('order-summary-loaded', function () {

        AddApplyDiscountEvents();
    });

    AddApplyDiscountEvents();
});
