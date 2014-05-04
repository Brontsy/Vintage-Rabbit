





$(document).ready(function () {

    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    
    $('.datepicker-trigger').datepicker({
        format: 'dd MM yyyy',
        autoclose: true
    }).on('changeDate', function (ev) {

        $('.check-availability-form').attr('action', $('.check-availability-form').data('checkAvailability'));
        $('.check-availability').removeClass('unavailable').removeClass('available').removeClass('in-cart');
    });


    //$.each($('#StartDate'), function(index, element)
    //{
    //    var checkin = element.datepicker({
    //        format: 'dd MM yyyy',
    //        autoclose: true,
    //        startDate: now,
    //        onRender: function (date) {
    //            return date.valueOf() < now.valueOf() ? 'disabled' : '';
    //        }
    //    }).on('changeDate', function (ev) {

    //        var newDate = new Date(ev.date)
    //        newDate.setDate(newDate.getDate() + 1);
    //        checkout.setValue(newDate);

    //        $('#EndDate')[0].focus();
    //    }).data('datepicker');
    //    var checkout = $('#EndDate').datepicker({
    //        format: 'dd MM yyyy',
    //        autoclose: true,
    //        onRender: function (date) {
    //            return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
    //        }
    //    }).on('changeDate', function (ev) {
    //    }).data('datepicker');
    //})


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
            dataType: 'json',
            success: function (json) {
                
                if (json.Available) {

                    $this.attr('action', $this.data('addToCart'));
                    $('.check-availability').addClass('available').removeClass('unavailable').removeClass('in-cart');
                }
                else
                {
                    $(this).attr('action', $this.data('checkAvailability'));
                    $('.check-availability').addClass('unavailable').removeClass('available').removeClass('in-cart');
                }

                button.html(button.data('button-text'));
            }
        })
    });

    
    $(window).on('AddToCart', function (json) {
        $('.add-hire-product-to-cart').find('button').addClass('hidden');
        $('.add-hire-product-to-cart').find('.available').addClass('hidden');
        $('.add-hire-product-to-cart').find('.added-to-cart').removeClass('hidden');
        $('.check-availability').addClass('in-cart');
    });

});