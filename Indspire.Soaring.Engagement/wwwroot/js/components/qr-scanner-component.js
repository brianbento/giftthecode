ko.components.register('qr-scanner', {
    viewModel: function (params) {
        var self = this;

        self.data = {
            scanner: ko.observable(null)
        };

        self.methods = new function (params) {
            var m = this;

            m.init = function (params) {

            }
        };

        self.methods.init(params);
    },
    template: {
        element: 'qr-scanner-tmpl'
    }
});