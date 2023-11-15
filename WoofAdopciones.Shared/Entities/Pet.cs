using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoofAdopciones.Shared.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Edad")]
        [Range(0, 100, ErrorMessage = "La {0} debe estar entre {1} y {2} años.")]
        public int Age { get; set; }

        public string Color { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Stock { get; set; }

        [Display(Name = "Fecha de Publicación")]
        public DateTime CreatedOn { get; set; }

        public ICollection<PetImage>? PetImages { get; set; }

        [Display(Name = "Imágenes")]
        public int ProductImagesNumber => PetImages == null || PetImages.Count == 0 ? 0 : PetImages.Count;

        [Display(Name = "Imagén")]
        public string MainImage => PetImages == null || PetImages.Count == 0 ? string.Empty : PetImages.FirstOrDefault()!.Image;

        public int AdoptionCenterId { get; set; }
    }
}
