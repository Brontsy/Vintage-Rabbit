





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



    $('#deliver').on('change', function () {

        if ($(this).is(':checked')) {
            $('.deliver-address-form').removeClass('hidden');
            $('.pickup-button').addClass('hidden');
        }
    });


    $('#pickup').on('change', function () {

        if ($(this).is(':checked')) {
            $('.deliver-address-form').addClass('hidden');
            $('.pickup-button').removeClass('hidden');
        }
    });


});