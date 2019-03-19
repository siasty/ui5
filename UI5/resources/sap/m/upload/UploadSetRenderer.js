/*!
 * OpenUI5
 * (c) Copyright 2009-2019 SAP SE or an SAP affiliate company.
 * Licensed under the Apache License, Version 2.0 - see LICENSE.txt.
 */
sap.ui.define([],function(){"use strict";var U={};U.render=function(r,c){r.write("<div");r.writeControlData(c);r.addClass("sapMUC");r.writeClasses();r.write(">");this.renderDragDropOverlay(r,c);this.renderList(r,c);r.write("</div>");};U.renderDragDropOverlay=function(r,c){r.write("<div");r.writeAttribute("id",c.getId()+"-drag-drop-area");r.addClass("sapMUCDragDropOverlay");r.addClass("sapMUCDragDropOverlayHide");r.writeClasses();r.write(">");r.write("<div");r.addClass("sapMUCDragDropIndicator");r.writeClasses();r.write(">");r.write("</div>");r.write("</div>");};U.renderList=function(r,c){r.renderControl(c.getList());};return U;});
