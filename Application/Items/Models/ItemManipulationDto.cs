using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Application.Items.Models
{
   public abstract class ItemManipulationDto
    {
        [Required]
        [MaxLength(150,ErrorMessage ="Item name can't exceed 150 characters")]
        public string Name { get; set; }
    }
}
