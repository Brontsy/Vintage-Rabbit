





$(document).ready(function () {

    $('.edit-product #Type').on('change', function()
    {
        $('.edit-product form').attr('class', $(this).val().toLowerCase());
    });

});