using System.ComponentModel.DataAnnotations;

namespace WoofAdopciones.Shared.Entities
{
    public class PetImage
    {
        public int Id { get; set; }

        public Pet Pet { get; set; } = null!;

        public int PetId { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; } = null!;
    }
}
