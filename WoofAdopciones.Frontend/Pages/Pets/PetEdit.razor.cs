using System.Data;
using System.Net;
using System.Net.Cache;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages.Pets
{
    [Authorize(Roles = "Admin")]
    public partial class PetEdit
    {
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private PetDTO petDTO = new()
        {
            PetImages = new List<string>()
        };

        private PetForm? petForm;
        private bool loading = true;
        private Pet? pet;

        [Parameter]
        public int PetId { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await LoadProductAsync();
        }

        private async Task AddImageAsync()
        {
            if (petDTO.PetImages is null || petDTO.PetImages.Count == 0)
            {
                return;
            }

            var imageDTO = new ImageDTO
            {
                PetId = PetId,
                Images = petDTO.PetImages!
            };

            var httpResponse = await repository.PostAsync<ImageDTO, ImageDTO>("/api/Pets/addImages", imageDTO);
            if (httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            petDTO.PetImages = httpResponse.Response!.Images;
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Imagenes agregadas con éxito.");
        }

        private async Task RemoveImageAsyc()
        {
            if (petDTO.PetImages is null || petDTO.PetImages.Count == 0)
            {
                return;
            }

            var imageDTO = new ImageDTO
            {
                PetId = PetId,
                Images = petDTO.PetImages!
            };

            var httpResponse = await repository.PostAsync<ImageDTO, ImageDTO>("/api/Pets/removeLastImage", imageDTO);
            if (httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            petDTO.PetImages = httpResponse.Response!.Images;
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Imagén eliminada con éxito.");
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
            petDTO = ToPetDTO(pet);
            loading = false;
        }

        private PetDTO ToPetDTO(Pet pet)
        {
            return new PetDTO
            {
                Description = pet.Description,
                Id = pet.Id,
                Name = pet.Name,
                Color = pet.Color,
                Stock = pet.Stock,
                CreatedOn = pet.CreatedOn,
                Age = pet.Age,
                AdoptionCenterId = pet.AdoptionCenterId,
                PetImages = pet.PetImages!.Select(x => x.Image).ToList()
            };
        }

        private async Task SaveChangesAsync()
        {
            var httpResponse = await repository.PutAsync("/api/Pets/full", petDTO);
            if (httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();
        }
        
        private void Return()
        {
            petForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/Pets");
        }
    }
}
