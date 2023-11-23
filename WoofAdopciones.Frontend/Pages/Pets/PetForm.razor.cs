using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using WoofAdopciones.Frontend.Helpers;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages.Pets
{
    public partial class PetForm
    {

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private EditContext editContext = null!;
        private List<MultipleSelectorModel> selected { get; set; } = new();
        private List<MultipleSelectorModel> nonSelected { get; set; } = new();
        private string? imageUrl;

        private List<AdoptionCenter>? adoptionCenters;

        [Parameter]
        public bool IsEdit { get; set; } = false;

        [EditorRequired]
        [Parameter]
        public PetDTO PetDTO { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [EditorRequired]
        [Parameter]
        public EventCallback ReturnAction { get; set; }


        [Parameter]
        public EventCallback AddImageAction { get; set; }

        [Parameter]
        public EventCallback RemoveImageAction { get; set; }

        public bool FormPostedSuccessfully { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadAdoptionCenterAsync();

            editContext = new(PetDTO);

           
        }
        private async Task LoadAdoptionCenterAsync()
        {
            var responseHttp = await repository.GetAsync<List<AdoptionCenter>>("/api/AdoptionCenter/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            adoptionCenters = responseHttp.Response;
        }
        private void ImageSelected(string imagenBase64)
        {
            if (PetDTO.PetImages is null)
            {
                PetDTO.PetImages = new List<string>();
            }

            PetDTO.PetImages!.Add(imagenBase64);
            imageUrl = null;
        }

        private async Task OnDataAnnotationsValidatedAsync()
        {
            PetDTO.AdoptionCenterId = selected.Select(x => int.Parse(x.Key)).ToList().FirstOrDefault();
            await OnValidSubmit.InvokeAsync();
        }


        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasEdited = editContext.IsModified();

            if (!formWasEdited)
            {
                return;
            }

            if (FormPostedSuccessfully)
            {
                return;
            }

            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "¿Deseas abandonar la página y perder los cambios?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true
            });

            var confirm = !string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}
