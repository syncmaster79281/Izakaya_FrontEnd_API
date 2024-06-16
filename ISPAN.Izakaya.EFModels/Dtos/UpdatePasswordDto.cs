using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class UpdatePasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
