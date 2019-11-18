using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Application.Users.Models
{
    public abstract class UserManipulationDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "The resource's first name exceeds the 20 character limit")]
       public string FirstName { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "The resource's last name exceeds the 20 character limit")]
       public string LastName { get; set; }
    }
}
