ko.components.register('award-validate', {
    viewModel: function (params) {
        var self = this;

        self.data = {
            code: ko.observable(null)
        }

        self.methods = new function (params) {
            var m = this;

            m.init = function (params) {

            }

            m.submit = function () {
                $.get('/json/ValidateCodeResponse', { code: self.data.code() },
                    function (response) {
                        //success
                        debugger;

                    },
                    function (error) {
                        //error
                    },
                    function () {
                        //always
                    }
                );
            }
        };

        self.methods.init(params);
    },
    template: {
        element: 'award-validate-tmpl'
    }
});