using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Application.Lists.Models
{
   public abstract class ListManipulationDto
    {
        [Required]
        [MaxLength(150,ErrorMessage ="Name of the list exceeds 150 characters")]
        public string Name { get; set; }
    }
}
