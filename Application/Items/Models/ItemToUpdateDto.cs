using Application.Items.Models.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Items.Models
{
    public class ItemToUpdateDto : ItemManipulationDto
    {
        [Status( "completed","incomplete", ErrorMessage ="Input of the status attribute needs to be completed or incomplete")]
        public string Status { get; set; }
    }
}
