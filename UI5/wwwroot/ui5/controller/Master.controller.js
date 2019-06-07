sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageBox",
    "./utilities",
    "sap/ui/core/routing/History"
], function (Controller, MessageBox, Utilities, History) {
   "use strict";
    
        return Controller.extend("ui5.controller.Master", {
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
            _onGenericTilePress: function (oEvent) {

                var oBindingContext = oEvent.getSource().getBindingContext();

                return new Promise(function (fnResolve) {

                    this.doNavigate("TestTable", oBindingContext, fnResolve, "");
                }.bind(this)).catch(function (err) {
                    if (err !== undefined) {
                        MessageBox.error(err.message);
                    }
                });

            },
            doNavigate: function (sRouteName, oBindingContext, fnPromiseResolve, sViaRelation) {
                var sPath = (oBindingContext) ? oBindingContext.getPath() : null;
                var oModel = (oBindingContext) ? oBindingContext.getModel() : null;

                console.log('oBindingContext: ' + oBindingContext);
                console.log('sPatch: ' + sPath);


                var sEntityNameSet;
                if (sPath === null && oBindingContext === "undefined") {
                    this.oRouter.navTo(sRouteName);
                } else {
                    if (sPath !== null && sPath !== "") {
                        if (sPath.substring(0, 1) === "/") {
                            sPath = sPath.substring(1);
                        }
                        sEntityNameSet = sPath.split("(")[0];
                    }
                    var sNavigationPropertyName;
                    var sMasterContext = this.sMasterContext ? this.sMasterContext : sPath;

                    if (sEntityNameSet !== null) {
                        sNavigationPropertyName = sViaRelation || this.getOwnerComponent().getNavigationPropertyForNavigationWithContext(sEntityNameSet, sRouteName);
                    }
                    if (sNavigationPropertyName !== null && sNavigationPropertyName !== undefined) {
                        if (sNavigationPropertyName === "") {
                            this.oRouter.navTo(sRouteName, {
                                context: sPath,
                                masterContext: sMasterContext
                            }, false);
                        } else {
                            oModel.createBindingContext(sNavigationPropertyName, oBindingContext, null, function (bindingContext) {
                                if (bindingContext) {
                                    sPath = bindingContext.getPath();
                                    if (sPath.substring(0, 1) === "/") {
                                        sPath = sPath.substring(1);
                                    }
                                } else {
                                    sPath = "undefined";
                                }

                                // If the navigation is a 1-n, sPath would be "undefined" as this is not supported in Build
                                if (sPath === "undefined") {
                                    this.oRouter.navTo(sRouteName);
                                } else {
                                    this.oRouter.navTo(sRouteName, {
                                        context: sPath,
                                        masterContext: sMasterContext
                                    }, false);
                                }
                            }.bind(this));
                        }
                    } else {
                        this.oRouter.navTo(sRouteName);
                    }
                }
                if (typeof fnPromiseResolve === "function") {
                    fnPromiseResolve();
                }

            },
            _onOverflowToolbarButtonPress: function (oEvent) {

                var oBindingContext = oEvent.getSource().getBindingContext();

                return new Promise(function (fnResolve) {

                    this.doNavigate("Logout", oBindingContext, fnResolve, "");
                }.bind(this)).catch(function (err) {
                    if (err !== undefined) {
                        MessageBox.error(err.message);
                    }
                });

            },
            onInit: function () {
                this.oRouter = sap.ui.core.UIComponent.getRouterFor(this);
                this.oRouter.getTarget("Master").attachDisplay(jQuery.proxy(this.handleRouteMatched, this));

            }
        });
    }, true);