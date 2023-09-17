using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckOrderTypeAsync();
        }

        private async Task CheckOrderTypeAsync()
        {
            if (!_context.OrderTypes.Any())
            {
                _context.OrderTypes.Add(new OrderType { Name = "Voluntario" });
                _context.OrderTypes.Add(new OrderType { Name = "Apadrinar" });
                _context.OrderTypes.Add(new OrderType { Name = "Odopción" });
                await _context.SaveChangesAsync();
            }
        }

    }
}
