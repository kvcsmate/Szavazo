using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistence.DTO
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        
        public String Password { get; set; }
    }
}