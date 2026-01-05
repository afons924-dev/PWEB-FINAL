using MyMEDIA.Client.Services.Interfaces;

namespace MyMEDIA.Client.Services
{
    public class UtilsTamanhoServices : IUtilsTamanhoServices
    {

        public int _preco { get; set; } = 1;

        public decimal _precoRefTamanho { get; set; } = 0.00m;
        public int Index {
            get {
                return _preco;
            }
            set {
                _preco = value;
                NotificationOnChange();
            }
        }

        public decimal PrecoRefSize
        {
            get {
                return _precoRefTamanho;
            }
            set {
                _precoRefTamanho = value;
                NotificationOnChange();
            }
        }

        public event Action? OnChange;

        private void NotificationOnChange() => OnChange?.Invoke();
    }
}
