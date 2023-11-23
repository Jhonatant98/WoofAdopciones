using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.Entites;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages.volunteer
{
    [Authorize(Roles = "Admin, User")]
    public partial class RequestsVolunteringIndex
    {
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        [Inject] private IRepository repository { get; set; } = null!;

        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

        private int currentPage = 1;
        private int totalPages;

        public List<RequestVolunteering>? RequestsVoluntering { get; set; }

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

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync();
        }

        private async Task<bool> LoadListAsync()
        {
            var url = string.Empty;
            if (string.IsNullOrEmpty(Filter))
            {
                url = $"api/Volunteerings/my";
            }
            else
            {
                url = $"api/Volunteerings/my";
            }

            var response = await repository.GetAsync<List<RequestVolunteering>>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            RequestsVoluntering = response.Response;
            return true;
        }
    }
}
