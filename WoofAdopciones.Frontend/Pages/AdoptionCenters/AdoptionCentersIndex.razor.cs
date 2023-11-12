using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WoofAdopciones.Frontend.Repositories;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Frontend.Pages.AdoptionCenters
{
    [Authorize(Roles = "Admin")]
    public partial class AdoptionCentersIndex
    { 
 [Inject] private IRepository repository { get; set; } = null!;

    [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;

    public List<AdoptionCenter>? AdoptionCenters { get; set; }
    private int currentPage = 1;
    private int totalPages;

    [CascadingParameter]
    private IModalService Modal { get; set; } = default!;

    [Parameter]
    [SupplyParameterFromQuery]
    public string Page { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery]
    public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task ShowModal(int id = 0, bool isEdit = false)
    {
        IModalReference modalReference;

        if (isEdit)
        {
            modalReference = Modal.Show<AdoptionCenterEdit>(string.Empty, new ModalParameters().Add("Id", id));
        }
        else
        {
            modalReference = Modal.Show<AdoptionCenterCreate>();
        }

        var result = await modalReference.Result;
        if (result.Confirmed)
        {
            await LoadAsync();
        }
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

        var ok = await LoadListAsync(page);
        if (ok)
        {
            await LoadPagesAsync();
        }
    }

    private async Task<bool> LoadListAsync(int page)
    {
        var url = $"api/AdoptionCenter?page={page}";
        if (!string.IsNullOrEmpty(Filter))
        {
            url += $"&filter={Filter}";
        }
        var response = await repository.GetAsync<List<AdoptionCenter>>(url);
        if (response.Error)
        {
            var message = await response.GetErrorMessageAsync();
            await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return false;
        }
        AdoptionCenters = response.Response;
        return true;
    }

    private async Task LoadPagesAsync()
    {
        var url = "api/AdoptionCenter/totalPages";
        if (!string.IsNullOrEmpty(Filter))
        {
            url += $"?filter={Filter}";
        }

        var response = await repository.GetAsync<int>(url);
        if (response.Error)
        {
            var message = await response.GetErrorMessageAsync();
            await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }
        totalPages = response.Response;
    }

    private async Task CleanFilterAsync()
    {
        Filter = string.Empty;
        await ApplyFilterAsync();
    }

    private async Task ApplyFilterAsync()
    {
        int page = 1;
        await LoadAsync(page);
        await SelectedPageAsync(page);
    }

    private async Task DeleteAsync(AdoptionCenter adoptionCenter)
    {
        var result = await sweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmación",
            Text = $"¿Esta seguro que quieres borrar el centro de adopción: {adoptionCenter.Name}?",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true
        });

        var confirm = string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        var response = await repository.DeleteAsync($"api/AdoptionCenter/{adoptionCenter.Id}");
        if (response.Error)
        {
            var message = await response.GetErrorMessageAsync();
            await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        await LoadAsync();

        var toast = sweetAlertService.Mixin(new SweetAlertOptions
        {
            Toast = true,
            Position = SweetAlertPosition.BottomEnd,
            ShowConfirmButton = true,
            Timer = 3000
        });
        await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con éxito.");
    }
}
}