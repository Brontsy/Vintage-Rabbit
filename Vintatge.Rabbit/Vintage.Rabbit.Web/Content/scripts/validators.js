// Expiry date validation.
// monthproperty: this is the dropdown box that contains the value for the month
// yearproperty: this is the dropdown that contains the year the user is selecting
// isrequired: is a validexpiry date required for the form to be submitted.
$.validator.addMethod("validexpiry", function (value, element, params) {

    var id = $(element).attr('id');

    var month = $('#' + params.monthproperty).val();
    var year = $('#' + params.yearproperty).val();

    // otherwise if its not required then we only need to make sure the date is greater than today
    if (month != '' && year != '') {
        //lol, javascript indexes months from 0
        var regoDate = new Date(year, month - 1, 1);
        var today = new Date();
        var compareDate = new Date(today.getFullYear(), today.getMonth(), 1);

        if (regoDate < compareDate) {
            return false;
        }
    }

    return true;
});

$.validator.unobtrusive.adapters.add("validexpiry", ['monthproperty', 'yearproperty', 'isrequired'], function (options) {
    options.rules["validexpiry"] = options.params;
    options.messages["validexpiry"] = options.message;
});


// CCV validation for client side. if its amex then it must have a length of 4
$.validator.addMethod("ccv", function (value, element, params) {

    if (value.length != 4 && value.length != 3) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.add("ccv", {}, function (options) {
    options.rules["ccv"] = options.params;
    options.messages["ccv"] = options.message;
});