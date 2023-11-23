using WoofAdopciones.Shared.Enums;

namespace WoofAdopciones.Shared.DTOs
{
    public class VolunteringDTO
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Description { get; set; } = null!;
        public RequestStatusVolunteering RequestStauts { get; set; }
    }
}