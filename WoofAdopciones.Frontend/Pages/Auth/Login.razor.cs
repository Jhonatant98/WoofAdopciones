﻿using Blazored.Modal.Services;
using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Frontend.Services;
using WoofAdopciones.Shared.DTOs;

namespace WoofAdopciones.Frontend.Pages.Auth
{
    public partial class Login
    {
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        [Inject] private ILoginService loginService { get; set; } = null!;

        private LoginDTO loginDTO = new();
        private bool wasClose;

        [CascadingParameter]
        private BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private async Task CloseModalAsync()
        {
            wasClose = true;
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }

        private async Task LoginAsync()
        {
            if (wasClose)
            {
                navigationManager.NavigateTo("/");
                return;
            }

            var responseHttp = await repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await loginService.LoginAsync(responseHttp.Response!.Token);
            navigationManager.NavigateTo("/");
        }
    }
}
