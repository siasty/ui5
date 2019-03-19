/*!
 * OpenUI5
 * (c) Copyright 2009-2019 SAP SE or an SAP affiliate company.
 * Licensed under the Apache License, Version 2.0 - see LICENSE.txt.
 */

/**
 * Initialization Code and shared classes of library sap.f.
 */
sap.ui.define(["sap/ui/base/DataType",
		"sap/ui/Global",
		"sap/ui/core/library",
		"sap/f/library"
	], // library dependency
	function (DataType) {

		"use strict";

		// delegate further initialization of this library to the Core
		sap.ui.getCore().initLibrary({
			name: "sap.ui.integration",
			version: "1.63.1",
			dependencies: ["sap.ui.core", "sap.f"],
			types: [],
			controls: [
				"sap.ui.integration.widgets.Card",
				"sap.ui.integration.host.HostConfiguration"
			],
			elements: [],
			noLibraryCSS: true,

			//define the custom tags that can be used in this library
			customTags: {
				"card": "sap/ui/integration/widgets/Card",
				"host-configuration": "sap/ui/integration/host/HostConfiguration"
			},
			defaultTagPrefix: "sap-ui-integration"
		});

		/**
		 * SAPUI5 library with controls specialized for SAP Fiori apps.
		 *
		 * @namespace
		 * @alias sap.ui.integration
		 * @author SAP SE
		 * @version 1.63.1
		 * @public
		 */
		var thisLib = sap.ui.integration;

		return thisLib;

	});