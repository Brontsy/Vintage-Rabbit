
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

//var componentForm = {
//    street_number: 'short_name',
//    route: 'long_name',
//    locality: 'long_name',
//    administrative_area_level_1: 'short_name',
//    country: 'long_name',
//    postal_code: 'short_name'
//};

//function initialize() {
//    // Create the autocomplete object, restricting the search
//    // to geographical location types.
//    autocomplete = new google.maps.places.Autocomplete(document.getElementById('autocomplete'), { types: ['geocode'] });
//    // When the user selects an address from the dropdown,
//    // populate the address fields in the form.
//    google.maps.event.addListener(autocomplete, 'place_changed', function () {
//        //fillInAddress();

//        var place = autocomplete.getPlace();
        

//        var streetNumber;
//        var street;
//        var city;
//        var postcode;
//        var state;
//        var country;

//        // Get each component of the address from the place details
//        // and fill the corresponding field on the form.
//        for (var i = 0; i < place.address_components.length; i++)
//        {
//            var addressType = place.address_components[i].types[0];

//            if (addressType == 'street_number') {
//                streetNumber = place.address_components[i]['short_name'];
//            }

//            if (addressType == 'route') {
//                street = place.address_components[i]['long_name'];
//            }
//            if (addressType == 'locality') {
//                city = place.address_components[i]['long_name'];
//            }
//            if (addressType == 'administrative_area_level_1') {
//                state = place.address_components[i]['long_name'];
//            }
//            if (addressType == 'country') {
//                country = place.address_components[i]['long_name'];
//            }
//            if (addressType == 'postal_code') {
//                postcode = place.address_components[i]['short_name'];
//            }
//        }

//        $('#Address').val(streetNumber + ' ' + street);
//        $('#SuburbCity').val(city);
//        $('#Postcode').val(postcode);
//        $('#State').val(state);
//    });
//}



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



    $('#IsDelivery').on('change', function () {
        
        if ($('#IsDelivery').is(':checked')) {
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


    // This example displays an address form, using the autocomplete feature
    // of the Google Places API to help users fill in the information.

    //var placeSearch, autocomplete;
    //var componentForm = {
    //    street_number: 'short_name',
    //    route: 'long_name',
    //    locality: 'long_name',
    //    administrative_area_level_1: 'short_name',
    //    country: 'long_name',
    //    postal_code: 'short_name'
    //};

    //initialize();

});
