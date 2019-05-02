using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UI5.Models
{
    public class OData
    {
        public class Book
        {
            [Key]
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
        }

        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "MyBooks";
            //builder.ContainerName = "DefaultContainer";
            builder.EntitySet<Book>("Books");
            return builder.GetEdmModel();
        }
    }
}
