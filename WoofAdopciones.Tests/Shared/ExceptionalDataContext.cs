using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;

namespace WoofAdopciones.Tests.Shared
{
    public class ExceptionalDataContext : DataContext
    {
        public ExceptionalDataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Test Exception");
        }
    }
}
