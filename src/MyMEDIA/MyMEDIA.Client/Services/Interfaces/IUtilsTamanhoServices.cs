namespace MyMEDIA.Client.Services.Interfaces
{
    public interface IUtilsTamanhoServices
    {
        int Index { get; set; }
        float PrecoRefSize { get; set; }

        public event Action OnChange;
    }
}
