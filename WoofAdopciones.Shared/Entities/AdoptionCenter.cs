using System.ComponentModel.DataAnnotations;

namespace WoofAdopciones.Shared.Entities
{
    public class AdoptionCenter
    {
        public int Id { get; set; }

        [Display(Name = "Nit")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombre")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Sede")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NameCampus { get; set; } = null!;

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Logo")]
        public string? Photo { get; set; }

        public City City { get; set; } = null!;

        [Display(Name = "Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int CityId { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName => $"{Name} {NameCampus}";

        [Display(Name = "Dirección")]
        public string FullAddress
        {
            get
            {
                var fullAddress = Address;
                if (City != null && City!.Name != null) fullAddress += $", {City.Name}";
                if (City != null && City!.State != null && City!.State!.Name != null) fullAddress += $", {City.State.Name}";
                if (City != null && City!.State != null && City!.State!.Country != null && City!.State!.Country!.Name != null) fullAddress += $", {City.State.Country.Name}";
                return fullAddress;
            }
        }
    }
}
