/*!
 * OpenUI5
 * (c) Copyright 2009-2019 SAP SE or an SAP affiliate company.
 * Licensed under the Apache License, Version 2.0 - see LICENSE.txt.
 */
sap.ui.define(['sap/ui/core/Renderer'],function(R){"use strict";return{render:function(r,c){r.write("<div");r.addClass("sapFShellBar");r.writeControlData(c);r.writeClasses();r.write(">");r.renderControl(c._getOverflowToolbar());r.write("</div>");}};},true);
