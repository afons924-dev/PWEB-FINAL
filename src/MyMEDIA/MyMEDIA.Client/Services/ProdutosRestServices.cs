using MyMEDIA.Client.Services.Interfaces;
using System.Text.Json;

using MyMEDIA.Client.Services;
using MyMEDIA.Shared.DTO;
namespace MyMEDIA.Client.Services
{
    public class ProdutosRestServices : IProdutosRestServices
    {

        private readonly HttpClient _httpClient = new();
        JsonSerializerOptions _serializerOptions;

        private List<ProdutoDTO> produtos;

        private List<Categoria> categorias;

        public ProdutosRestServices()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            produtos = new List<ProdutoDTO>();

            categorias = new List<Categoria>();
        }
        public async Task<List<ProdutoDTO>> GetAllProdutos()
        {

            string endpoint = $"api/Produtos?tipoProduto=todos";

            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = "";

                    content = await httpResponseMessage.Content.ReadAsStringAsync();
                    produtos = JsonSerializer.Deserialize<List<ProdutoDTO>>(content, _serializerOptions)!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

            return produtos;
        }

        public async Task<List<Categoria>> GetAllCategorias()
        {

            string endpoint = $"api/Categorias";

            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = "";

                    content = await httpResponseMessage.Content.ReadAsStringAsync();
                    categorias = JsonSerializer.Deserialize<List<Categoria>>(content, _serializerOptions)!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                             return null;
            }

            return categorias;
        }


    }
}
