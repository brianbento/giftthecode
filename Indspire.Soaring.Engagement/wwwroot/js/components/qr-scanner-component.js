ko.components.register('qr-scanner', {
    viewModel: function (params) {
        var self = this;

        self.types = {
            Award: {
                Name: 'Award',
                Endpoint: '/json/PointsAwardedResponse.json'
            },
            Redemption: {
                Name: 'Redemption',
                Endpoint: '/json/RedemptionResponse.json'
            }
        }

        self.state = {
            type: null,
            error: ko.observable(null),
            success: ko.observable(null),
            currentCamera: ko.observable(null),
            scannerActive: ko.observable(false),
            loading: ko.observable(false)
        }

        self.data = {
            scanner: ko.observable(null),
            cameras: ko.observableArray([]),
            code: ko.observable(null),
            awardNumber: null,
            redemptionNumber: null
        };

        self.events = {
            onScan: function (content) {
                console.log('Scan found:' + content)
                if (content) {
                    self.data.code(content)
                    self.methods.submit();
                }
            },
            onActive: function () {
                self.state.scannerActive(true);
            },
            onInActive: function () {
                self.state.scannerActive(false);
            },
            onCamerasLoaded: self.data.cameras.subscribe(function (cameras) {
                if (cameras.length > 0) {
                    self.state.currentCamera(cameras[0]);
                    
                } else {
                    self.state.error('No cameras found.');
                }
            }),
            onCameraSelected: self.state.currentCamera.subscribe(function (camera) {
                self.data.scanner().start(camera);
            })
        }

        self.methods = new function (params) {
            var m = this;

            m.init = function (params) {
                if (params.redemptionNumber) {
                    self.state.type = self.types.Redemption;
                    self.data.redemptionNumber = params.redemptionNumber;
                }
                if (params.awardNumber) {
                    self.state.type = self.types.Award;
                    self.data.awardNumber = params.awardNumber;
                }
            };

            m.changeCamera = function (camera) {
                self.state.currentCamera(camera);
            }


            m.submit = function () {
                var postData = {
                    UserNumber: self.data.code()
                };

                if (self.state.type === self.types.Award) {
                    postData.AwardNumber = self.data.awardNumber;
                }

                if (self.state.type === self.types.Redemption) {
                    postData.RedemptionNumber = self.data.redemptionNumber;
                }

                self.state.loading(true);

                $.get(self.state.type.Endpoint, postData)
                    .done(function (response) {
                        //success
                        self.state.success("Success!")
                        self.data.code(null);
                    }).fail(function (error) {
                        //error
                        self.state.error("Error!")
                    }).always(function () {
                        //always
                        self.state.loading(false);
                    });
                
            }
        };

        self.methods.init(params);
    },
    template: {
        element: 'qr-scanner-tmpl'
    }
});