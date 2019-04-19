sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/core/routing/History"
], function (Controller, History) {
    "use strict";

        return Controller.extend("ui5.controller.Overview", {

        onInit: function () {
            this.getView().addStyleClass("sapUiSizeCompact"); // make everything inside this View appear in Compact mode

            //this.getView().bindElement("/Books");

        },

        onErrorCall: function (oError) {
            if (oError.statusCode === 500 || oError.statusCode === 400 || oError.statusCode === "500" || oError.statusCode === "400") {
                var errorRes = JSON.parse(oError.responseText);
                if (!errorRes.error.innererror) {
                    sap.m.MessageBox.alert(errorRes.error.message.value);
                } else {
                    if (!errorRes.error.innererror.message) {
                        sap.m.MessageBox.alert(errorRes.error.innererror.toString());
                    } else {
                        sap.m.MessageBox.alert(errorRes.error.innererror.message);
                    }
                }
                return;
            } else {
                sap.m.MessageBox.alert(oError.response.statusText);
                return;
            }

        },
        /**The following methods could be added to a reusable base controller
                 * Convenience method for accessing the router in every controller of the application.
                 * @public
                 * @returns {sap.ui.core.routing.Router} the router for this component
                 */
        getRouter: function () {
            return this.getOwnerComponent().getRouter();
        },

        /**
         * Convenience method for getting the view model by name in every controller of the application.
         * @public
         * @param {string} sName the model name
         * @returns {sap.ui.model.Model} the model instance
         */
        getModel: function (sName) {
            return this.getView().getModel(sName);
        },

        /**
         * Convenience method for setting the view model in every controller of the application.
         * @public
         * @param {sap.ui.model.Model} oModel the model instance
         * @param {string} sName the model name
         * @returns {sap.ui.mvc.View} the view instance
         */
        setModel: function (oModel, sName) {
            return this.getView().setModel(oModel, sName);
        },


        /**
         * Event handler for navigating back.
         * It there is a history entry we go one step back in the browser history
         * If not, it will replace the current entry of the browser history with the master route.
         * @public
         */
        onNavBack: function () {
            var sPreviousHash = History.getInstance().getPreviousHash();

            if (sPreviousHash !== undefined) {
                history.go(-1);
            } else {
                this.getRouter().navTo("master", {}, true);
            }
        }

    });

       
});