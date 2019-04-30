sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/odata/v4/ODataModel",
    "sap/ui/core/routing/History"
], function (Controller, ODataModel,History) {
    "use strict";

        return Controller.extend("ui5.controller.Overview", {

            onInit: function () {
                this.oModel = new ODataModel({
                    synchronizationMode: "None",
                    serviceUrl: "/odata/"
                });

                this.getView().setModel(this.oModel, 'Items');
                this.getView().bindElement({ path: "/Books" });
            }


        });

       
});