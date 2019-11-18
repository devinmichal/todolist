using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Models
{
   public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        
    }
}
