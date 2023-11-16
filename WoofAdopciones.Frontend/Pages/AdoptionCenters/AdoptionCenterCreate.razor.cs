﻿using Blazored.Modal.Services;
using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages.AdoptionCenters
{
    [Authorize(Roles = "Admin")]
    public partial class AdoptionCenterCreate
    { 
  [Inject] private NavigationManager navigationManager { get; set; } = null!;

    [Inject] private IRepository repository { get; set; } = null!;

    [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

    private AdoptionCenter adoptionCenter = new();
    private AdoptionCenterForm? adoptionCenterForm;

    [CascadingParameter]
    private BlazoredModalInstance BlazoredModal { get; set; } = default!;

    private async Task CreateAsync()
    {
        var httpResponse = await repository.PostAsync("api/AdoptionCenter", adoptionCenter);

        if (httpResponse.Error)
        {
            var mensajeError = await httpResponse.GetErrorMessageAsync();
            await sweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
            return;
        }

        await BlazoredModal.CloseAsync(ModalResult.Ok());
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
        adoptionCenterForm!.FormPostedSuccessfully = true;
        navigationManager.NavigateTo("adoptioncenters");
    }
}
}