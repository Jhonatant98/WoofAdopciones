using System.ComponentModel.DataAnnotations;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Shared.DTOs
{
    public class PetDTO
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

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha de Publicación")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public bool state { get; set; } = true;

        public List<string>? PetImages { get; set; }

        public AdoptionCenter? AdoptionCenter { get; set; }

        [Display(Name = "Centro de adopción")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un {0}.")]
        public int AdoptionCenterId { get; set; }
    }
}
