



function AddClickEvents()
{
    $('a.remove-from-cart').off('click');
    $('a.remove-from-cart').on('click', function (event) {

        $(this).parents('li').css('opacity', 0.3).find('.col-5').html('removing');
        event.preventDefault();
        $.ajax({
            url: $(this).attr('href'),
            type: 'POST',
            complete: function (json) {
                $(window).trigger('RemovedFromCart', json)
            }
        })
    });
    
    $('.cart-content').off('click');
    $('.cart-content').on('click', function (event) {

        event.stopPropagation();
    });

    $('header .shopping-cart .container').off('click');
    $('header .shopping-cart .container').on('click', function (event) {

        event.stopPropagation();
        $(this).toggleClass('active');
        $('.cart-content').toggleClass('hidden');
    });
    
    $('.shopping-cart').off('mouseover');
    $('.shopping-cart').on('mouseover', function (event) {

        $(this).parents('.row').addClass('shopping-cart-hover');
    });

    $('.shopping-cart').off('mouseleave');
    $('.shopping-cart').on('mouseleave', function (event) {
        $(this).parents('.row').removeClass('shopping-cart-hover');
    });

    $('#qty').off('change');
    $('#qty').on('change', function () {
        
        if(parseInt($(this).val()))
        {
            if(parseInt($(this).val()) > parseInt($(this).data('max-value')))
            {
                $(this).val($(this).data('max-value'));
                $(this).removeClass('valid');
            }
        }
        else
        {
            //alert('no number');
            $(this).removeClass('valid');
        }
    });


    $('a.add-to-cart').off('click');
    $('a.add-to-cart').on('click', function (event) {

        event.preventDefault();
        var $this = $(this);
        $.ajax({
            url: $(this).attr('href'),
            type: 'POST',
            complete: function (json) {
                $(window).trigger('AddToCart', $this)
            }
        })
    });

    $('button.add-to-cart').off('click');
    $('button.add-to-cart').on('click', function (event) {

        event.preventDefault();
        var $this = $(this);

        var form = $(this).parents('form');

        $(this).data('button-text', $(this).html());
        $(this).html('Adding');

        $.ajax({
            url: form.attr('action'),
            data: form.serialize(),
            type: 'POST',
            complete: function (json) {
                $this.html($this.data('button-text'));
                $(window).trigger('AddToCart', $this)
            }
        })
    });

    $('.shopping-cart .qty').on('change', function () {

        var alertDiv = $(this).parents('li').find('.error');
        var productName = $(this).parents('li').find('.product-name a');

        alertDiv.addClass('hidden');

        var qty = $(this).val();
        if (qty > $(this).data('max')) {
            alertDiv.find('.alert').html('Sorry we only have <strong>' + $(this).data('max') + '</strong> ' + productName.html() + ' left');
            alertDiv.removeClass('hidden');

            // Sorry we only have 5 
        }
        else {
            $(this).parents('li').css('opacity', 0.3).find('.col-5').html('Updating');

            var url = $(this).parents('li').data('update-qty-url') + '?qty=' + qty;
            $.ajax({
                url: url,
                type: 'POST',
                success: function (response) {

                    $(window).trigger('QuantityUpdated')
                }
            });
        }
    });
}




$(document).ready(function () {

    AddClickEvents();

});


$(window).on('lightbox.ajax-content-loaded availability-check', function () {

    AddClickEvents();
});


$(window).on('AddToCart RemovedFromCart QuantityUpdated', function (json) {

    //$('.added-to-cart').removeClas
    $.each($('.shopping-cart'), function (index, element) {

        var open = $(this).find('.container').hasClass('active');

        $.ajax({
            url: $(element).data('reload'),
            type: 'GET',
            data: { isOpen: open },
            dataType: 'html',
            success: function (html) {

                $(element).replaceWith($(html));
                AddClickEvents()
                if (open)
                {
                    $('header .shopping-cart .container').addClass('active');
                }
            }
        })
    });
});


$(window).on('AddToCart', function (element) {

    $(element).parents('form').find('.added-to-cart').removeClass('hidden');
});

$(window).on('RemovedFromCart', function (json) {

    $('.added-to-cart').addClass('hidden');
});

$('body').on('click', function () {
    $('header .shopping-cart .container').removeClass('active');
    $('header .cart-content').addClass('hidden');
});
