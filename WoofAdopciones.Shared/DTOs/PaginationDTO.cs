namespace WoofAdopciones.Shared.DTOs
{
    public class PaginationDTO
    {
        public int Id { get; set; }

        public int Page { get; set; } = 1;

        public int RecordsNumber { get; set; } = 10;

        public string? Filter { get; set; }

        public string? AdoptionCenterFilter { get; set; }

        public string? StateFilter { get; set; }

    }
}
