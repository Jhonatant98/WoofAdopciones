using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Helpers;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public class PetsRepository : GenericRepository<Pet>, IPetsRepository
    {
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;

        public PetsRepository(DataContext context, IFileStorage fileStorage) : base(context)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public override async Task<Response<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Pets
                .Include(x => x.PetImages)
                .AsQueryable();



            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(pagination.StateFilter))
            {
                queryable = queryable.Where(x => x.state.Equals(bool.Parse(pagination.StateFilter)));
            }

            return new Response<IEnumerable<Pet>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public override async Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Pets.AsQueryable();

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

        public override async Task<Response<Pet>> GetAsync(int id)
        {
            var pet = await _context.Pets
                .Include(x => x.PetImages)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pet == null)
            {
                return new Response<Pet>
                {
                    WasSuccess = false,
                    Message = "Mascota no existe"
                };
            }

            return new Response<Pet>
            {
                WasSuccess = true,
                Result = pet
            };
        }

        public async Task<Response<Pet>> AddFullAsync(PetDTO petDTO)
        {
            try
            {
                var newPet = new Pet
                {
                    Name = petDTO.Name,
                    Age = petDTO.Age,
                    Color = petDTO.Color,
                    CreatedOn = petDTO.CreatedOn,
                    Description = petDTO.Description,
                    state = petDTO.state,
                    PetImages = new List<PetImage>()
                };

                foreach (var petImage in petDTO.PetImages!)
                {
                    var photoPet = Convert.FromBase64String(petImage);
                    newPet.PetImages.Add(new PetImage { Image = await _fileStorage.SaveFileAsync(photoPet, ".jpg", "users") });
                }

                _context.Add(newPet);
                await _context.SaveChangesAsync();
                return new Response<Pet>
                {
                    WasSuccess = true,
                    Result = newPet
                };
            }
            catch (DbUpdateException)
            {
                return new Response<Pet>
                {
                    WasSuccess = false,
                    Message = "Ya existe una mascota con el mismo nombre."
                };
            }
            catch (Exception exception)
            {
                return new Response<Pet>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };
            }
        }

        public async Task<Response<Pet>> UpdateFullAsync(PetDTO petDTO)
        {
            try
            {
                var pet = await _context.Pets
                    .FirstOrDefaultAsync(x => x.Id == petDTO.Id);
                if (pet == null)
                {
                    return new Response<Pet>
                    {
                        WasSuccess = false,
                        Message = "Producto no existe"
                    };
                }

                pet.Name = petDTO.Name;
                pet.Age = petDTO.Age;
                pet.Color = petDTO.Color;
                pet.CreatedOn = petDTO.CreatedOn;
                pet.Description = petDTO.Description;
                pet.state = petDTO.state;

                _context.Update(pet);
                await _context.SaveChangesAsync();
                return new Response<Pet>
                {
                    WasSuccess = true,
                    Result = pet
                };
            }
            catch (DbUpdateException)
            {
                return new Response<Pet>
                {
                    WasSuccess = false,
                    Message = "Ya existe una mascota con el mismo nombre."
                };
            }
            catch (Exception exception)
            {
                return new Response<Pet>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };
            }
        }

        public async Task<Response<ImageDTO>> AddImageAsync(ImageDTO imageDTO)
        {
            var pet = await _context.Pets
                .Include(x => x.PetImages)
                .FirstOrDefaultAsync(x => x.Id == imageDTO.PetId);
            if (pet == null)
            {
                return new Response<ImageDTO>
                {
                    WasSuccess = false,
                    Message = "Mascota no existe"
                };
            }

            for (int i = 0; i < imageDTO.Images.Count; i++)
            {
                if (!imageDTO.Images[i].StartsWith("https://"))
                {
                    var photoPet = Convert.FromBase64String(imageDTO.Images[i]);
                    imageDTO.Images[i] = await _fileStorage.SaveFileAsync(photoPet, ".jpg", "users");
                    pet.PetImages!.Add(new PetImage { Image = imageDTO.Images[i] });
                }
            }

            _context.Update(pet);
            await _context.SaveChangesAsync();
            return new Response<ImageDTO>
            {
                WasSuccess = true,
                Result = imageDTO
            };
        }

        public async Task<Response<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO)
        {
            var product = await _context.Pets
                .Include(x => x.PetImages)
                .FirstOrDefaultAsync(x => x.Id == imageDTO.PetId);
            if (product == null)
            {
                return new Response<ImageDTO>
                {
                    WasSuccess = false,
                    Message = "Mascota no existe"
                };
            }

            if (product.PetImages is null || product.PetImages.Count == 0)
            {
                return new Response<ImageDTO>
                {
                    WasSuccess = true,
                    Result = imageDTO
                };
            }

            var lastImage = product.PetImages.LastOrDefault();
            await _fileStorage.RemoveFileAsync(lastImage!.Image, "pets");
            product.PetImages.Remove(lastImage);
            _context.Update(product);
            await _context.SaveChangesAsync();
            imageDTO.Images = product.PetImages.Select(x => x.Image).ToList();
            return new Response<ImageDTO>
            {
                WasSuccess = true,
                Result = imageDTO
            };
        }
    }
}
