ko.bindingHandlers.qrScanner = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called when the binding is first applied to an element
        // Set up any initial state, event handlers, etc. here

        var scanner = valueAccessor().scanner;
        var camerasObs = valueAccessor().cameras;
        var onScan = valueAccessor().onScan;
        var onActive = valueAccessor().onActive;

        scanner(new Instascan.Scanner({ video: element }));

        scanner().addListener('scan', function (content) {
            onScan(content);
        });

        scanner().addListener('active', function () {
            onActive();
        });

        scanner().addListener('inactive', function () {
            onInActive();
        });

        Instascan.Camera.getCameras().then(function (cameras) {
            camerasObs(cameras);
        }).catch(function (e) {
            console.error(e);
        });

    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever any observables/computeds that are accessed change
        // Update the DOM element based on the supplied values here.
    }
};