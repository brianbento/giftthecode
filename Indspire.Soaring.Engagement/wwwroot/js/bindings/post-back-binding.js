(function (window) {

    "use strict";

    var ko = window.ko;

    var $ = window.jQuery;

    ko.bindingHandlers.postBack = {

        init: function (element) {

            $(element).on("change", function (evt) {
                var form = $(element).closest("form");

                if (form.length > 0) {

                    evt.preventDefault();

                    form.submit();
                }
            });
        }
    };

}(this));