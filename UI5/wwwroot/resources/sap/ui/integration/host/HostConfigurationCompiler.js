/*!
 * OpenUI5
 * (c) Copyright 2009-2019 SAP SE or an SAP affiliate company.
 * Licensed under the Apache License, Version 2.0 - see LICENSE.txt.
 */
sap.ui.define(["sap/ui/thirdparty/less","sap/base/Log"],function(L,a){"use strict";var l=jQuery.sap.loadResource("sap/ui/integration/host/HostConfigurationMap.json",{dataType:"json"}),b=jQuery.sap.loadResource("sap/ui/integration/host/HostConfiguration.less",{dataType:"text"});function c(u,t){return new Promise(function(r,e){jQuery.ajax({url:u,async:true,dataType:t,success:function(j){r(j);},error:function(){e();}});});}function _(n,p){if(!p){return n;}var P=p.split("/"),i=0;if(!P[0]){n=n;i++;}while(n&&P[i]){n=n[P[i]];i++;}return n;}function g(C,s){var m=l,p=[];for(var n in m){var v=_(C,m[n].path),u=m[n].unit;if(v){p.push(n+":"+v+(u?u:""));}else{p.push(n+": /*null*/");}}var r=b.replace(/\#hostConfigName/g,"."+s);r=r.replace(/\/\* HOSTCONFIG PARAMETERS \*\//,p.join(";\n")+";");var P=new L.Parser(),S="";P.parse(r,function(e,R){try{S=R.toCSS();}catch(f){S=" ";}});return S;}function d(C,o){return c(C,"json").then(function(o){return g(o,o);});}return{loadResource:c,generateCssText:g,generateCssTextAsync:d};});
