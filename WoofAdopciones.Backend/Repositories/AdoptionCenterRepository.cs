using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Helpers;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public class AdoptionCenterRepository : GenericRepository<AdoptionCenter>, IAdoptionCenterRepository
    {
        private readonly DataContext _context;

        public AdoptionCenterRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Response<IEnumerable<AdoptionCenter>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.AdoptionCenters
                .Include(u => u.City!)
                .ThenInclude(c => c.State!)
                .ThenInclude(s => s.Country)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new Response<IEnumerable<AdoptionCenter>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<IEnumerable<AdoptionCenter>> GetComboAsync()
        {
            return await _context.AdoptionCenters
                .Include(u => u.City!)
                .ThenInclude(c => c.State!)
                .ThenInclude(s => s.Country)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public override async Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.AdoptionCenters.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new Response<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        public virtual async Task<Response<AdoptionCenter>> AddAsync(AdoptionCenter entity)
        {
            _context.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new Response<AdoptionCenter>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionResponse();
            }
            catch (Exception exception)
            {
                return ExceptionResponse(exception);
            }
        }
        private Response<AdoptionCenter> ExceptionResponse(Exception exception)
        {
            return new Response<AdoptionCenter>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }

        private Response<AdoptionCenter> DbUpdateExceptionResponse()
        {
            return new Response<AdoptionCenter>
            {
                WasSuccess = false,
                Message = "Ya existe el registro que estas intentando crear."
            };
        }
    }
}