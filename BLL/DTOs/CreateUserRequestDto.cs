using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class CreateUserRequestDto
    {
        public string PhoneNumber { get; set; }
        public List<int> CourseIds { get; set; }
    }
}
