using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Helpers;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Enums;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public class AdoptionsRepository : GenericRepository<Adoption>, IAdoptionsRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IPetsUnitOfWork _petsUnitOfWork;

        public AdoptionsRepository(DataContext context, IUserHelper userHelper, IPetsUnitOfWork petsUnitOfWork) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _petsUnitOfWork = petsUnitOfWork;
        }

        public async Task<Response<IEnumerable<Adoption>>> GetAsync(string email, PaginationDTO pagination)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new Response<IEnumerable<Adoption>>
                {
                    WasSuccess = false,
                    Message = "Usuario no válido",
                };
            }

            var queryable = _context.Adoptions
                .Include(s => s.Pet)
                .Include(s => s.User!)
                .AsQueryable();

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if (!isAdmin)
            {
                queryable = queryable.Where(s => s.User!.Email == email);
            }

            return new Response<IEnumerable<Adoption>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderByDescending(x => x.Date)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<Response<int>> GetTotalPagesAsync(string email, PaginationDTO pagination)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new Response<int>
                {
                    WasSuccess = false,
                    Message = "Usuario no válido",
                };
            }

            var queryable = _context.Adoptions.AsQueryable();

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if (!isAdmin)
            {
                queryable = queryable.Where(s => s.User!.Email == email);
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return new Response<int>
            {
                WasSuccess = true,
                Result = (int)totalPages
            };
        }

        public override async Task<Response<Adoption>> GetAsync(int id)
        {
            var adoption = await _context.Adoptions
                .Include(s => s.Pet)
                .Include(s => s.User!)
                .ThenInclude(u => u.City!)
                .ThenInclude(c => c.State!)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == id);



            if (adoption == null)
            {
                return new Response<Adoption>
                {
                    WasSuccess = false,
                    Message = "Pedido no existe"
                };
            }

            return new Response<Adoption>
            {
                WasSuccess = true,
                Result = adoption
            };
        }

        public async Task<Response<Adoption>> UpdateFullAsync(string email, AdoptionDTO adoptionDTO)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new Response<Adoption>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Admin.ToString());
            if (!isAdmin && adoptionDTO.AdoptionStatus != AdoptionStatus.Rejected)
            {
                return new Response<Adoption>
                {
                    WasSuccess = false,
                    Message = "Solo permitido para administradores."
                };
            }

            var adoption = await _context.Adoptions
                .FirstOrDefaultAsync(s => s.Id == adoptionDTO.Id);
            if (adoption == null)
            {
                return new Response<Adoption>
                {
                    WasSuccess = false,
                    Message = "Adopción no existe"
                };
            }

            adoption.AdoptionStatus = adoptionDTO.AdoptionStatus;
            _context.Update(adoption);
            await _context.SaveChangesAsync();
            return new Response<Adoption>
            {
                WasSuccess = true,
                Result = adoption
            };
        }

        public async Task<Response<bool>> ProcessAdoptionAsync(string email, int petId)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new Response<bool>
                {
                    WasSuccess = false,
                    Message = "Usuario no válido"
                };
            }

            var pet = await _petsUnitOfWork.GetAsync(petId);
            if (pet.Result == null)
            {
                return new Response<bool>
                {
                    WasSuccess = false,
                    Message = "No se encontró la mascota"
                };
            }
            var adoption = new Adoption
            {
                Date = DateTime.UtcNow,
                User = user,
                Pet = pet.Result,
                AdoptionStatus = AdoptionStatus.InReview
            };

            _context.Add(adoption);
            await _context.SaveChangesAsync();

            return new Response<bool>
            {
                WasSuccess = true,
                Message = "Solicitud creada con exito"
            };
        }

    }
}
