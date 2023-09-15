using Microsoft.EntityFrameworkCore;

namespace WoofAdopciones.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
