





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
   

});