using Microsoft.AspNetCore.Components;
using MyMEDIA.Shared.DTO;
using MyMEDIA.Client.Services.Interfaces;

namespace MyMEDIA.Client.Components.Slider
{
    public partial class TamanhosComponent
    {
        [Parameter]
        public List<TamanhosProdutoDTO> Tamanhos { get; set; }

        [Inject]
        public IUtilsTamanhoServices utilsTamanhoServices { get; set; }

        protected override async Task OnInitializedAsync()
        {
            int maxSizes = Tamanhos.Count;

            int index = 0;

            try
            {
                if (utilsTamanhoServices.Index >= 0 && utilsTamanhoServices.Index < maxSizes)
                {
                    index = utilsTamanhoServices.Index;
                    utilsTamanhoServices.PrecoRefSize = Tamanhos[index].Preco;
                }
                else
                    utilsTamanhoServices.PrecoRefSize = 0;
            }
            catch(Exception ex) {
                string message = ex.Message;
            }
            finally
            {

            }

        }

        void Changed(int index)
        {
            utilsTamanhoServices.Index = index;

            index = index == 0 ? utilsTamanhoServices.Index : index;

            utilsTamanhoServices.PrecoRefSize = Tamanhos[index].Preco;
        }

    }
}
