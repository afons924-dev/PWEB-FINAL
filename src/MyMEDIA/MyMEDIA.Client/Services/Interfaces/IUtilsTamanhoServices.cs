namespace MyMEDIA.Client.Services.Interfaces
{
    public interface IUtilsTamanhoServices
    {
        int Index { get; set; }
        decimal PrecoRefSize { get; set; }

        public event Action OnChange;
    }
}
