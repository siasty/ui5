sap.ui.controller("view.App", {

    onPress: function (evt) {
        jQuery.sap.require("sap.m.MessageToast");
        sap.m.MessageToast.show(evt.getSource().getId() + " Pressed");
    }
});