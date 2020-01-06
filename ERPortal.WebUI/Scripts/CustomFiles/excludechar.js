// <reference path="jquery.validate.js" />  
// <reference path="jquery.validate.unobtrusive.js" />  
$.validator.unobtrusive.adapters.addSingleVal("customvalidationclass", "chars");
$.validator.addMethod("customvalidationclass", function (value, element, exclude) {
    if (value) {
        for (var i = 0; i < exclude.length; i++) {
            if (jQuery.inArray(exclude[i], value) != -1) {
                return false;
            }
        }
    }
    return true;
});  