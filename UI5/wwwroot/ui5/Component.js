sap.ui.define([
    "sap/ui/core/UIComponent",
    "sap/ui/model/json/JSONModel",
    "sap/ui/Device"
], function (UIComponent, JSONModel, Device) {
    "use strict";

    return UIComponent.extend("ui5.Component", {

        metadata: {
            manifest: "json"
        },
  

        init: function () {
            jQuery.sap.require("sap.m.MessageBox");
            jQuery.sap.require("sap.m.MessageToast");

            var oModel = new JSONModel(Device);
            oModel.setDefaultBindingMode("OneWay");

            this.setModel(oModel, "device");

            UIComponent.prototype.init.apply(this, arguments);

            this.getRouter().initialize();

        },
        
        createContent: function () {
            var app = new sap.m.App({
                id: "App"
            });
            var appType = "App";
            var appBackgroundColor = "#FFFFFF";
            if (appType === "App" && appBackgroundColor) {
                app.setBackgroundColor(appBackgroundColor);
            }

            return app;
        },

        getNavigationPropertyForNavigationWithContext: function (sEntityNameSet, targetPageName) {
            var entityNavigations = navigationWithContext[sEntityNameSet];
            return entityNavigations === null ? null : entityNavigations[targetPageName];
        }

      
   
    });

});