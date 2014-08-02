





$(document).ready(function () {

    $('.selectable-theme-image').on('click', function()
    {
        var guid = $(this).data('guid');

        $('.selectable-theme-image').removeClass('selected');
        $(this).addClass('selected');

        $('.theme-products').addClass('hidden');
        $('.' + guid).removeClass('hidden');
    })

});