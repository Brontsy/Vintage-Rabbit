





$(document).ready(function () {


    $('.ajax-lightbox').featherlight(
        {
            targetAttr: 'href',
            type: 'ajax',
            variant: 'lightbox-content',
            afterOpen: function()
            {

                AddClickEvents();
                
                $(window).trigger('lightbox.ajax-content-loaded');
            }
        });

    //$('.ajax-lightbox').magnificPopup({
    //    type: 'ajax',
    //    closeBtnInside: true,
    //    mainClass: 'lightbox-content',
    //    parseAjax: function (mfpResponse) {
    //        alert('s23');
    //    },
    //    ajaxContentAdded: function () {
    //        alert('s');
    //    }
    //});
   

});