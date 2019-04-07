sap.ui.controller("view.App", {

    onPress: function (evt) {
        jQuery.sap.require("sap.m.MessageToast");
        sap.m.MessageToast.show(evt.getSource().getId() + " Pressed");
    },

    onInit: function () {
   
        var oModel = new sap.ui.model.odata.v4.ODataModel(
            {
                synchronizationMode: "None",
                serviceUrl: '/odata/'
            });
        this.getView().setModel(oModel);

    }
});