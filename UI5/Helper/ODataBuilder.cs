using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static UI5.Models.OData;

namespace UI5.Helper
{
    public class ODataBuilder
    {

        public static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "MyBooks";
            builder.ContainerName = "DefaultContainer";
            builder.EntitySet<Book>("Books")
                   .EntityType
                   .Filter()
                   .Count()
                   .Expand()
                   .OrderBy()
                   .Page()
                   .Select();

            builder.EntitySet<Press>("Presses")
                   .EntityType
                   .Filter()
                   .Count()
                   .Expand()
                   .OrderBy()
                   .Page()
                   .Select();

            return builder.GetEdmModel(); 
        }

    
    }
}
