using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Url { get; set; }
    }
}
