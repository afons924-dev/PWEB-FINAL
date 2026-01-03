using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RCLAPI.DTO;
using RCLAPI.Services;
using RCLProdutos.Services.Interfaces;
using System.Linq;

namespace RCLProdutos.Shared.CardProduto;

public partial class SlideInfoProdutoComponent
{


    [SupplyParameterFromQuery]
    private int Id { get; set; }


    [Inject]
    public IApiServices? _apiServices { get; set; }

    private List<ProdutoDTO>? produtos { get; set; }

    public ProdutoDTO infoProduto = new ProdutoDTO();

    protected override async Task OnInitializedAsync()
    {
        int infoProdutoID;

        infoProdutoID = Id;

        infoProduto = await _apiServices.GetDetalheProduto(infoProdutoID);
    }
}