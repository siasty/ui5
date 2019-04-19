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
      
        destroy: function () {
            UIComponent.prototype.destroy.apply(this, arguments);
        }

     
    });

});