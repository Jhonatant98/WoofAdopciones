
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.Entites;

namespace WoofAdopciones.Frontend.Pages.volunteer
{
    [Authorize(Roles = "Admin, User")]
    public partial class RequestVolunteringCreate
    { 
  [Inject] private NavigationManager navigationManager { get; set; } = null!;

    [Inject] private IRepository repository { get; set; } = null!;

    [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

    private RequestVolunteering requestVoluntering = new();
    private RequestVolunteringForm? requestVolunteringForm;


    private async Task CreateAsync()
    {
        var httpResponse = await repository.PostAsync("api/Volunteerings/full", requestVoluntering);

        if (httpResponse.Error)
        {
            var mensajeError = await httpResponse.GetErrorMessageAsync();
            await sweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
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
            requestVolunteringForm!.FormPostedSuccessfully = true;
        navigationManager.NavigateTo("/RequestsVolunteering");
    }
}
}