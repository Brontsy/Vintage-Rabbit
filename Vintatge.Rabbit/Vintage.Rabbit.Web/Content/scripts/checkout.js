﻿





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
        }
        else {
            $('.deliver-field').addClass('hidden');
        }
    });

});