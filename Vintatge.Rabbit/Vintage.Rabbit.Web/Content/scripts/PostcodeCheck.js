




function PostcodeCheckEvents() {
    
    $('.postcode-check-form').on('submit', function (event) {

        event.preventDefault();
        var $this = $(this);
        
        if ($this.valid()) {
            var button = $(this).find('.btn.check-availability-button');

            $(this).data('check-availability', $(this).attr('action'));

            button.data('button-text', $(button).html());
            $(button).html('Checking');

            $.ajax({
                url: $(this).attr('action'),
                data: $(this).serialize(),
                type: 'GET',
                success: function (response) {

                    $('.availability-check').html(response);

                    $(window).trigger('availability-check');
                }
            });
        }
    });
}

$(document).ready(function () {

    PostcodeCheckEvents();

});



$(window).on('lightbox.ajax-content-loaded', function () {

    PostcodeCheckEvents();
});
