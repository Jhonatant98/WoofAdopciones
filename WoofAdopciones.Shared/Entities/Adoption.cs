using System.ComponentModel.DataAnnotations;
using WoofAdopciones.Shared.Enums;

namespace WoofAdopciones.Shared.Entities
{
    public class Adoption
    {
        public int Id { get; set; }

        public User User { get; set; } = null!;

        public Pet Pet { get; set; } = null!;

        public AdoptionStatus AdoptionStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha de creacion")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }
    }
}
