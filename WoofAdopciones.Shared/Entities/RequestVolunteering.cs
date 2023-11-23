
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Enums;

namespace WoofAdopciones.Shared.Entites
{
    public class RequestVolunteering
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public string? UserId { get; set; }
        public City City { get; set; } = null!;
        public int? CityId { get; set; }
        public string Description { get; set; } = null!;
        public RequestStatusVolunteering RequestStauts { get; set; }
    }
}