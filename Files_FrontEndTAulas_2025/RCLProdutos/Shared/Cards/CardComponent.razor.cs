using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Components;
using RCLAPI.DTO;
using RCLAPI.Services;
using RCLProdutos.Services.Interfaces;


namespace RCLProdutos.Shared.Cards
{
    public partial class CardComponent
    {
        [Parameter]
        public Categoria? categoria { get; set; }

        [Parameter]
        public int? selectedCatId { get; set; }    

        [Parameter]
        public string? marginLeft { get; set; }

        private List<TamanhosProdutoDTO>? sizes { get; set; }

        //[Inject]
        //public IUtilsTamanhoServices utilsSizeServices { get; set; }
        public int? selectedCategoriaId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            selectedCategoriaId = selectedCatId;
            //utilsSizeServices.OnChange += StateHasChanged;
        }

        private void Navega(Categoria categoria)
        {
            NavigationManager.NavigateTo($"slider?Id={categoria.Id}&nomeCat={categoria.Nome}");
        }
    }
}
