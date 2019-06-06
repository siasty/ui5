sap.ui.define([
    "sap/ui/core/UIComponent",
    "sap/ui/model/json/JSONModel",
    "sap/ui/Device",
    "ui5/model/models",
    "./model/errorHandling"
], function (UIComponent, JSONModel, Device,models, errorHandling) {
        "use strict";

        var navigationWithContext = {

        };

    return UIComponent.extend("ui5.Component", {

        metadata: {
            manifest: "json"
        },
  

        init: function () {

            this.setModel(models.createDeviceModel(), "device");


            var oApplicationModel = new JSONModel({});
            this.setModel(oApplicationModel, "applicationModel");

            UIComponent.prototype.init.apply(this, arguments);

            errorHandling.register(this);


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