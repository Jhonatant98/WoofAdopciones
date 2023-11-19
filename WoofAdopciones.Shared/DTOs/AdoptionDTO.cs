using WoofAdopciones.Shared.Enums;

namespace WoofAdopciones.Shared.DTOs
{
    public class AdoptionDTO
    {
        public int Id { get; set; }

        public AdoptionStatus AdoptionStatus { get; set; }

        public string Remarks { get; set; } = string.Empty;

        public int PetId { get; set; }

    }
}
