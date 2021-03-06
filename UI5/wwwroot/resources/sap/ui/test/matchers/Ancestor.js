/*!
 * OpenUI5
 * (c) Copyright 2009-2019 SAP SE or an SAP affiliate company.
 * Licensed under the Apache License, Version 2.0 - see LICENSE.txt.
 */
sap.ui.define(["sap/ui/test/_LogCollector","sap/base/Log"],function(_,L){"use strict";var l=L.getLogger("sap.ui.test.matchers.Ancestor");function m(p,a){var M=typeof a==="string";return M?(p&&p.getId())===a:p===a;}return function(a,d){return function(c){if(!a){l.debug("No ancestor was defined so no controls will be filtered.");return true;}var p=c.getParent();while(!d&&p&&!m(p,a)){p=p.getParent();}var r=m(p,a);if(!r){l.debug("Control '"+c+"' does not have "+(d?"direct ":"")+"ancestor '"+a);}return r;};};},true);
