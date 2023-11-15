using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages.Pets
{
    [Authorize(Roles = "Admin")]
    public partial class PetCreate
    {

        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private PetDTO petDTO = new()
        {
            PetImages = new List<string>()
        };

        private bool loading = true;
        private PetForm? petForm;
        protected override async Task OnInitializedAsync()
        {
            
            loading = false;

        }

        private async Task CreateAsync()
        {
            var response = await repository.PostAsync("/api/Pets", petDTO);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");

        }

        private void Return()
        {
            petForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/Pets");
        }
    }
}
