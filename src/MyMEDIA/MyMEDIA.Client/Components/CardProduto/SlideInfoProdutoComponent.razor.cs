using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyMEDIA.Shared.DTO;
using MyMEDIA.Client.Services;
using MyMEDIA.Client.Services.Interfaces;
using System.Linq;

namespace MyMEDIA.Client.Components.CardProduto;

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