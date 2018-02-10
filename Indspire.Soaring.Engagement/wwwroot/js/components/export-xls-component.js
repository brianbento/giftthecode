ko.components.register('export-xls', {
    viewModel: function (params) {
        var self = this;

        self.data = {
            fileName: ko.observable(),
            tableId: ko.observable()
        };

        self.css = {
            anchorCSS: null,
            iconCSS: null
        }



        self.methods = new function (params) {
            var m = this;
            m.init = function (params) {
                self.data.fileName(params.fileName + '.xls');
                self.data.tableId(params.tableId)
                self.css.anchorCSS = params.anchorCSS;
                self.css.iconCSS = params.iconCSS;
            };

            m.export = function (v, e) {
                return ExcellentExport.excel(e.target, self.data.tableId(), self.data.tableId())
            };
        };

        self.methods.init(params);
    },
    template: {
        element: 'export-xls-tmpl'
    }
});