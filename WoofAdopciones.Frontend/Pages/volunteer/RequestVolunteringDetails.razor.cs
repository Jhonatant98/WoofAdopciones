using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.Entites;
using WoofAdopciones.Shared.Enums;

namespace WoofAdopciones.Frontend.Pages.volunteer
{
    public partial class RequestVolunteringDetails
    {
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private RequestVolunteering? requestVolunteering;


        [Parameter]
        public int volunteringId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHppt = await repository.GetAsync<RequestVolunteering>($"api/Volunteerings/{volunteringId}");
            if (responseHppt.Error)
            {
                if (responseHppt.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/RequestsVolunteering");
                    return;
                }
                var messageError = await responseHppt.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            requestVolunteering = responseHppt.Response;


        }

        private async Task ReviewVolunteringAsync()
        {
            await ModifyStatus("Revisar", RequestStatusVolunteering.Review);
        }

        private async Task ApprovedVolunteringSync()
        {
            await ModifyStatus("Aprobar el voluntariado", RequestStatusVolunteering.Approved);
        }

        private async Task RejectVolunteringAsync()
        {
            await ModifyStatus("Rechazada", RequestStatusVolunteering.Rejected);
        }


        private async Task ModifyStatus(string message, RequestStatusVolunteering status)
        {
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Esta seguro que quieres {message} el voluntariado?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            requestVolunteering!.RequestStauts = status;

            var responseHTTP = await repository.PutAsync("api/Volunteerings", requestVolunteering);
            if (responseHTTP.Error)
            {
                var mensajeError = await responseHTTP.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
                return;
            }

            navigationManager.NavigateTo("/RequestsVolunteering");
        }
    }
}
