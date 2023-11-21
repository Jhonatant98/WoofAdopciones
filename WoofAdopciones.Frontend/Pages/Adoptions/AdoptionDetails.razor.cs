using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Enums;

namespace WoofAdopciones.Frontend.Pages.Adoptions
{
    public partial class AdoptionDetails
    {
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private Adoption? adoption;

        [Parameter]
        public int adoptionId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHppt = await repository.GetAsync<Adoption>($"api/Adoptions/{adoptionId}");
            if (responseHppt.Error)
            {
                if (responseHppt.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/Adoptions");
                    return;
                }
                var messageError = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            adoption = responseHppt.Response;
        }

        private async Task CancelOrderAsync()
        {
            await ModifyTemporalOrder("Rechazar", AdoptionStatus.Rejected);
        }

        private async Task DispatchOrderAsync()
        {
            await ModifyTemporalOrder("Visitar el Hogar de", AdoptionStatus.GoHome);
        }

        private async Task SendOrderAsync()
        {
            await ModifyTemporalOrder("Aprobar", AdoptionStatus.Approved);
        }


        private async Task ModifyTemporalOrder(string message, AdoptionStatus status)
        {
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Esta seguro que quieres {message} la adopción?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var adoptionDTO = new AdoptionDTO
            {
                Id = adoptionId,
                AdoptionStatus = status
            };

            var responseHTTP = await repository.PutAsync("api/Adoptions", adoptionDTO);
            if (responseHTTP.Error)
            {
                var mensajeError = await responseHTTP.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
                return;
            }

            navigationManager.NavigateTo("/Adoptions");
        }
    }
}
