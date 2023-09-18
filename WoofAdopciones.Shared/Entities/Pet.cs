using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
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

        public string Color { get; set; }

        [Display(Name = "Fecha de Publicación")]
        public DateTime CreatedOn { get; set; }
    }
}
