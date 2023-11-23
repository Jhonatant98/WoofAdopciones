using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
namespace WoofAdopciones.Frontend.Pages.Pets
{
    public partial class PetDetails
    {
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private List<string>? images;
        private bool loading = true;
        private Pet? pet;
        private bool isAuthenticated;

        [Parameter]
        public int PetId { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            await CheckIsAuthenticatedAsync();
        }
        private async Task CheckIsAuthenticatedAsync()
        {
            var authenticationState = await authenticationStateTask;
            isAuthenticated = authenticationState.User.Identity!.IsAuthenticated;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadProductAsync();
        }

        private async Task LoadProductAsync()
        {
            loading = true;
            var httpResponse = await repository.GetAsync<Pet>($"/api/Pets/{PetId}");

            if (httpResponse.Error)
            {
                loading = false;
                var message = await httpResponse.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            pet = httpResponse.Response!;
            images = pet.PetImages!.Select(x => x.Image).ToList();
            loading = false;
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

            Return();
        }
        private void Return()
        {
            navigationManager.NavigateTo("/");
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
