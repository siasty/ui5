sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/odata/v4/ODataModel",
    "sap/ui/core/routing/History",
    "sap/m/MessageBox",
    "./utilities"
], function (Controller, ODataModel, History, MessageBox, Utilities) {
    "use strict";

        return Controller.extend("ui5.controller.TestTable", {
            handleRouteMatched: function (oEvent) {
                var sAppId = "App5cf6cff4687d54010939e1c2";

                var oParams = {};

                if (oEvent.mParameters.data.context) {
                    this.sContext = oEvent.mParameters.data.context;

                } else {
                    if (this.getOwnerComponent().getComponentData()) {
                        var patternConvert = function (oParam) {
                            if (Object.keys(oParam).length !== 0) {
                                for (var prop in oParam) {
                                    if (prop !== "sourcePrototype" && prop.includes("Set")) {
                                        return prop + "(" + oParam[prop][0] + ")";
                                    }
                                }
                            }
                        };

                        this.sContext = patternConvert(this.getOwnerComponent().getComponentData().startupParameters);

                    }
                }

                var oPath;

                if (this.sContext) {
                    oPath = {
                        path: "/" + this.sContext,
                        parameters: oParams
                    };
                    this.getView().bindObject(oPath);
                }

            },
            _onPageNavButtonPress: function () {
                var oHistory = History.getInstance();
                var sPreviousHash = oHistory.getPreviousHash();
                var oQueryParams = this.getQueryParameters(window.location);

                if (sPreviousHash !== undefined || oQueryParams.navBackToLaunchpad) {
                    window.history.go(-1);
                } else {
                    var oRouter = sap.ui.core.UIComponent.getRouterFor(this);
                    oRouter.navTo("default", true);
                }

            },
            getQueryParameters: function (oLocation) {
                var oQuery = {};
                var aParams = oLocation.search.substring(1).split("&");
                for (var i = 0; i < aParams.length; i++) {
                    var aPair = aParams[i].split("=");
                    oQuery[aPair[0]] = decodeURIComponent(aPair[1]);
                }
                return oQuery;

            },
            onInit: function () {

                this.oRouter = sap.ui.core.UIComponent.getRouterFor(this);
                this.oRouter.getTarget("TestTable").attachDisplay(jQuery.proxy(this.handleRouteMatched, this));

                this.oModel = new ODataModel({
                    groupId: "$direct",
                    synchronizationMode: "None",
                    serviceUrl: 'https://localhost:5001/odata/'
                });

                this.getView().setModel(this.oModel, 'items');

                this.getView().bindElement({ path: "/Books" });

            }
        });


    }, true);