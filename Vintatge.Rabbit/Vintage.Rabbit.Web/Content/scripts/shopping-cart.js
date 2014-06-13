



function AddClickEvents()
{
    $('a.remove-from-cart').off('click');
    $('a.remove-from-cart').on('click', function (event) {

        event.preventDefault();
        $.ajax({
            url: $(this).attr('href'),
            type: 'POST',
            complete: function (json) {
                $(window).trigger('RemovedFromCart', json)
            }
        })
    });

    $('.cart-content').on('click', function (event) {

        event.stopPropagation();
    });

    $('header .shopping-cart .container').on('click', function (event) {

        event.stopPropagation();
        $(this).toggleClass('active');
        $('.cart-content').toggleClass('hidden');
    });

    $('.shopping-cart').on('mouseover', function (event) {

        $(this).parents('.row').addClass('shopping-cart-hover');
    });
    $('.shopping-cart').on('mouseleave', function (event) {
        $(this).parents('.row').removeClass('shopping-cart-hover');
    });

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
}



$('a.add-to-cart').on('click', function (event) {

    event.preventDefault();
    $.ajax({
        url: $(this).attr('href'),
        type: 'POST',
        complete: function (json) {
            $(window).trigger('AddToCart', json)
        }
    })
});

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
            
            $(window).trigger('AddToCart', json)
        }
    })
});

$(document).ready(function () {

AddClickEvents();

});

$(window).on('AddToCart RemovedFromCart', function (json) {

    $('.added-to-cart').removeClas
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
            }
        })
    });
});


$(window).on('AddToCart', function (json) {

    $('.added-to-cart').removeClass('hidden');
});

$(window).on('RemovedFromCart', function (json) {

    $('.added-to-cart').addClass('hidden');
});

$('body').on('click', function () {
    $('header .shopping-cart .container').removeClass('active');
    $('header .cart-content').addClass('hidden');
});
