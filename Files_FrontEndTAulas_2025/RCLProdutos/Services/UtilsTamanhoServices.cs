using RCLProdutos.Services.Interfaces;

namespace RCLProdutos.Services
{
    public class UtilsTamanhoServices : IUtilsTamanhoServices
    {

        public int _preco { get; set; } = 1;

        public float _precoRefTamanho { get; set; } = 0.00f;
        public int Index {
            get {
                return _preco;
            }
            set {
                _preco = value;
                NotificationOnChange();
            }
        }

        public float PrecoRefSize
        {
            get { 
                return _precoRefTamanho; 
            }
            set {
                _precoRefTamanho = value;
                NotificationOnChange();
            }
        }

        public event Action OnChange;

        private void NotificationOnChange() => OnChange?.Invoke();
    }
}
