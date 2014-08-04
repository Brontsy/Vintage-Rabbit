





    $(window).on('lightbox.ajax-content-loaded', function () {

        $("#product-search").autocomplete({
            source: function (request, response) {

                $('.input-icon i').removeClass('hidden');

                $.ajax({
                    url: $("#product-search").data('search-url'),
                    dataType: "json",
                    data: {
                        term: request.term
                    },
                    success: function (data) {

                        $('.input-icon i').addClass('hidden');
                        response(data);
                    }
                });
            },
            minLength: 3,
            select: function (event, ui) {
                event.preventDefault();
                $('#ProductGuid').val(ui.item.value);
                $("#product-search").val(ui.item.label);
            }
        });


        $('img.main-image').on('click', function (e) {

            var parentOffset = $(this).offset();
            //or $(this).offset(); if you really just want the current element's offset
            var x = e.pageX - parentOffset.left;
            var y = e.pageY - parentOffset.top;
            

            //var posX = $(this).offset().left;
            //var posY = $(this).offset().top;

            //var x = e.pageX - posX + 20;
            //var y = e.pageY - posY + 10;

            var percentX = x * 100 / $(this).width();
            var percentY = y * 100 / $(this).height();

            $('#X').val(percentX);
            $('#Y').val(percentY);

            $(this).parent().find('.marker').remove();
            $(this).parent().append('<div class="marker selected" style="left: ' + percentX + '%; top: ' + percentY + '%;"><span class="inner-circle"></span></div>');
        });

    });
