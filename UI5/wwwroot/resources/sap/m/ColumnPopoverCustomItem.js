/*
 * ! OpenUI5
 * (c) Copyright 2009-2019 SAP SE or an SAP affiliate company.
 * Licensed under the Apache License, Version 2.0 - see LICENSE.txt.
 */
sap.ui.define(['./ColumnPopoverItem'],function(C){"use strict";var a=C.extend("sap.m.ColumnPopoverCustomItem",{library:"sap.m",metadata:{properties:{icon:{type:"sap.ui.core.URI",group:"Misc",defaultValue:null},text:{type:"string",group:"Misc",defaultValue:null}},aggregations:{content:{type:"sap.ui.core.Control",multiple:false,singularName:"content"}},events:{beforeShowContent:{}}}});return a;});
