using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages
{

    public partial class Index
    {
        private int currentPage = 1;
        private int totalPages;
        private bool isAuthenticated;
        private string allCategories = "all_categories_list";

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        public List<Pet>? Pets { get; set; }
        public List<AdoptionCenter>? AdoptionCenters { get; set; }
        public string AdoptionCenterFilter { get; set; } = string.Empty;
        public string StateFilter { get; set; } = "true";


        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; } = null!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string Page { get; set; } = "";

        [Parameter]
        [SupplyParameterFromQuery]
        public string Filter { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task CheckIsAuthenticatedAsync()
        {
            var authenticationState = await authenticationStateTask;
            isAuthenticated = authenticationState.User.Identity!.IsAuthenticated;
        }

        protected override async Task OnParametersSetAsync()
        {
            await CheckIsAuthenticatedAsync();
        }

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page, AdoptionCenterFilter);
        }

       
        private async Task LoadAsync(int page = 1, string adoptionCenter = "")
        {
            if (!string.IsNullOrWhiteSpace(adoptionCenter))
            {
                if (adoptionCenter == allCategories)
                {
                    AdoptionCenterFilter = string.Empty;
                }
                else
                {
                    AdoptionCenterFilter = adoptionCenter;
                }
            }

            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            var url = $"api/Pets?page={page}&RecordsNumber=8&filter={Filter}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }
            if (!string.IsNullOrEmpty(StateFilter))
            {
                url += $"&StateFilter={StateFilter}";
            }
            if (!string.IsNullOrEmpty(AdoptionCenterFilter))
            {
                url += $"&AdoptionCenterFilter={AdoptionCenterFilter}";
            }

            var response = await repository.GetAsync<List<Pet>>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Pets = response.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            var url = $"api/Pets/totalPages/?RecordsNumber=8";
            if (string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }
            if (!string.IsNullOrEmpty(AdoptionCenterFilter))
            {
                url += $"&AdoptionCenterFilter={AdoptionCenterFilter}";
            }

            var response = await repository.GetAsync<int>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = response.Response;
        }

        private async Task CleanFilterAsync()
        {
            Filter = string.Empty;
            await ApplyFilterAsync();
        }

        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        private async Task AddToCartAsync(Pet pet)
        {
            if (!isAuthenticated)
            {
                navigationManager.NavigateTo("/Login");
                var toast1 = sweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = false,
                    Timer = 5000
                });
                await toast1.FireAsync(icon: SweetAlertIcon.Error, message: "Debes haber iniciado sesión para poder agregar productos al carro de compras.");
                return;
            }

            var adoptionDTO = new AdoptionDTO
            {
                PetId = pet.Id
            };

            var httpResponse = await repository.PostAsync("/api/Adoptions", adoptionDTO);
            if (httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            PetDTO petDTO = ToPetDTO(pet);

            var httpResponsePet = await repository.PutAsync("/api/Pets/full", petDTO);
            if (httpResponsePet.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }


            //await LoadCounterAsync();

            var toast2 = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast2.FireAsync(icon: SweetAlertIcon.Success, message: "Solicitud realizada con exito.");

            await LoadAsync();
        }

        private PetDTO ToPetDTO(Pet pet)
        {
            return new PetDTO
            {
                Description = pet.Description,
                Id = pet.Id,
                Name = pet.Name,
                Color = pet.Color,
                state = false,
                CreatedOn = pet.CreatedOn,
                Age = pet.Age,
                AdoptionCenterId = pet.AdoptionCenterId,
                PetImages = pet.PetImages!.Select(x => x.Image).ToList()
            };
        }
    }
}
