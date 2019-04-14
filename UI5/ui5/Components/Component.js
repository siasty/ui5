sap.ui.define([
    "sap/ui/core/UIComponent",
    "sap/ui/model/json/JSONModel",
    "sap/ui/Device"
], function (UIComponent, JSONModel, Device) {
    "use strict";

    return UIComponent.extend("host.ui5.Components.Component", {

        metadata: {
            manifest: "json"
        },
  

        init: function () {
            jQuery.sap.require("sap.m.MessageBox");
            jQuery.sap.require("sap.m.MessageToast");

            var oModel = new JSONModel(Device);
            oModel.setDefaultBindingMode("OneWay");

            this.setModel(oModel, "device");

            sap.ui.core.UIComponent.prototype.init.apply(this, arguments);


        },
      
        destroy: function () {
            UIComponent.prototype.destroy.apply(this, arguments);
        }

     
    });

});