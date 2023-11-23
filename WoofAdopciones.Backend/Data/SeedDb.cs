using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;
using WoofAdopciones.Backend.Services;
using WoofAdopciones.Shared.Enums;
using WoofAdopciones.Backend.Helpers;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;

namespace WoofAdopciones.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;
        private readonly IFileStorage _fileStorage;
        private readonly IRuntimeInformationWrapper _runtimeInformationWrapper;

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper, IFileStorage fileStorage, IRuntimeInformationWrapper runtimeInformationWrapper)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
            _fileStorage = fileStorage;
            _runtimeInformationWrapper = runtimeInformationWrapper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            //await CheckCountriesAsync();
            await CheckCountriesAsync2();
            await CheckAdoptionCenterAsync();
            await CheckPetsAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Jhonatan", "Wirbiezcas", "jhonatan@yopmail.com", "300 594 3458", "Bello", UserType.Admin);
            await CheckUserAsync("1011", "Juan Diego", "Gil", "JuanDiego@yopmail.com", "313 548 2252", "La Raya", UserType.Admin);
            await CheckUserAsync("1011", "Juan Diego", "Gil", "JuanDiego@yopmail.com", "313 548 2252", "La Raya", UserType.Admin);

        }


        private async Task CheckCountriesAsync2()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Antioquia",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Name = "Medellín"
                                }
                            }
                        }
                    }
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetsAsync()
        {
            var adoptionCenter = await _context.AdoptionCenters.FirstOrDefaultAsync();


            if (!_context.Pets.Any())
            {
                await AddPetAsync("Polo", "Blanco", 13, new List<string>() { "polo.png" },adoptionCenter);
                await AddPetAsync("Nemo", "Negro", 13, new List<string>() { "nemo.png" }, adoptionCenter);
                await AddPetAsync("Matias", "cade", 13, new List<string>() { "matias.png" }, adoptionCenter);
                await AddPetAsync("Betoben", "cade", 13, new List<string>() { "betoben.png" }, adoptionCenter);

                await _context.SaveChangesAsync();
            }
        }

        private async Task AddPetAsync(string name, string color, int age, List<string> images,AdoptionCenter adoptionCenter )
        {
            Pet pet = new()
            {
                Description = name,
                Name = name,
                Color = color,
                Age = age,
                PetImages = new List<PetImage>(),
                AdoptionCenterId = adoptionCenter.Id,
            };


            foreach (string? image in images)
            {
                string filePath;
                if (_runtimeInformationWrapper.IsOSPlatform(OSPlatform.Windows))
                {
                    filePath = $"{Environment.CurrentDirectory}\\Images\\pets\\{image}";
                }
                else
                {
                    filePath = $"{Environment.CurrentDirectory}/Images/pets/{image}";
                }
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "pets");
                pet.PetImages.Add(new PetImage { Image = imagePath });
            }

            _context.Pets.Add(pet);
        }
        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                if (city == null)
                {
                    city = await _context.Cities.FirstOrDefaultAsync();
                }
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckAdoptionCenterAsync()
        {
            var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
            if (city == null)
            {
                city = await _context.Cities.FirstOrDefaultAsync();
            }
            if (!_context.AdoptionCenters.Any())
            {
                _context.AdoptionCenters.Add(new AdoptionCenter {
                    Name = "Huellas limpias",
                    Document = "87612777",
                    NameCampus = "Sabaneta",
                    Address = "CL 45 34 - 34",
                    City = city

                });
               
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
        {
           if (!_context.Countries.Any())
            {
                var responseCountries = await _apiService.GetAsync<List<CountryResponse>>("/v1", "/countries");
                if (responseCountries.WasSuccess)
                {
                    var countries = responseCountries.Result!;
                    foreach (var countryResponse in countries)
                    {
                        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            var responseStates = await _apiService.GetAsync<List<StateResponse>>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.WasSuccess)
                            {
                                var states = responseStates.Result!;
                                foreach (var stateResponse in states!)
                                {
                                    var state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        var responseCities = await _apiService.GetAsync<List<CityResponse>>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.WasSuccess)
                                        {
                                            var cities = responseCities.Result!;
                                            foreach (var cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                var city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();

                            }
                      }
                    }
                }
            }
        }
    }
}
