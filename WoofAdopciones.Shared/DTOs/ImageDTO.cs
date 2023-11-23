using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoofAdopciones.Shared.DTOs
{
    public class ImageDTO
    {
        [Required]
        public int PetId { get; set; }

        [Required]
        public List<string> Images { get; set; } = null!;
    }
}
