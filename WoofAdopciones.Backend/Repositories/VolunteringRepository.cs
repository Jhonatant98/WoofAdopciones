
using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Helpers;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entites;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Enums;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public class VolunteringRepository : GenericRepository<RequestVolunteering>, IAVolunteringsRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public VolunteringRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<Response<VolunteringDTO>> AddFullAsync(string email, VolunteringDTO volunteringDTO)
        {

            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new Response<VolunteringDTO>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var voluntering = new RequestVolunteering
            {
                UserId = user.Id,
                User = user,
                CityId = volunteringDTO.CityId,
                Description = volunteringDTO.Description,
                RequestStauts = RequestStatusVolunteering.Sent
            };

            try
            {
                _context.Add(voluntering);
                await _context.SaveChangesAsync();
                return new Response<VolunteringDTO>
                {
                    WasSuccess = true,
                    Result = volunteringDTO
                };
            }
            catch (Exception ex)
            {
                return new Response<VolunteringDTO>
                {
                    WasSuccess = false,
                    Message = ex.InnerException.Message
                };
            }
        }

        public async Task<Response<IEnumerable<RequestVolunteering>>> GetAsync(string email)
        {

            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new Response<IEnumerable<RequestVolunteering>>
                {
                    WasSuccess = false,
                    Message = "Usuario no válido",
                };
            }

            var queryable = _context.RequestVolunteering
                .Include(s => s.User!)
                .Include(s => s.City!)
                .AsQueryable();

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if (!isAdmin)
            {
                queryable = queryable.Where(s => s.User!.Email == email);
            }

            return new Response<IEnumerable<RequestVolunteering>>
            {
                WasSuccess = true,
                Result = await queryable.ToListAsync()
            };
        }

        public override async Task<Response<RequestVolunteering>> GetAsync(int id)
        {

            var temporalOrder = await _context.RequestVolunteering
                .Include(ts => ts.User!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (temporalOrder == null)
            {
                return new Response<RequestVolunteering>
                {
                    WasSuccess = false,
                    Message = "Registro no encontrado"
                };
            }

            return new Response<RequestVolunteering>
            {
                WasSuccess = true,
                Result = temporalOrder
            };
        }


    }
}