using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Data.DTOs
{
    public class LoginUserDto
    {
        [Required]
        public string UserName{ get; set; }
        [Required]
        public string Password { get; set; }
    }
}
