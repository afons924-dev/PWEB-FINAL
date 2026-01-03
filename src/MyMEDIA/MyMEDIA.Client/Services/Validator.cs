using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyMEDIA.Client.Services;

public class Validator : IValidator
{
    public string NomeErro {  get; set; }
    public string EmailErro { get; set; }
    public string TelefoneErro { get; set; }
    public string SenhaErro { get; set; }

    private const string NomeVazioErrorMsg = "Por favor, indique o seu nome!";
    private const string EmailVazioErrorMsg = "Por favor, indique o seu email!";
    private const string TelefoneVazioErrorMsg = "Por favor, indique o seu telefone1";
    private const string SenhaVazioErrorMsg = "Por favor, indique a sua password"!;

    private const string NomeInvalidoErrorMsg = "Por favor, indique um nome válido!";
    private const string EmailInvalidoErrorMsg = "Por favor, indique um email válido!";
    private const string TelefoneInvalidoErrorMsg = "Por favor, indique um telefone válido!";
    private const string SenhaInvalidoErrorMsg = "Por favor, indique uma password válida!";

    public Task<bool> Validar(string nome, string email, string telefone, string senha)
    {
        var isNomeValido = ValidarNome(nome);
        var isEmailValido = ValidarEmail(email);
        var isTelefoneValido = ValidarTelefone(telefone);
        var isSenhaValido = ValidarSenha(senha);

        return Task.FromResult(isNomeValido && isEmailValido && isTelefoneValido && isSenhaValido);
    }

    public bool ValidarNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
        {
            NomeErro = NomeVazioErrorMsg;
            return false;
        }
        if (nome.Length < 3)
        {
            NomeErro = NomeInvalidoErrorMsg;
            return false;
        }
        NomeErro = "";
        return true;
    }
    public bool ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            EmailErro = EmailVazioErrorMsg;
            return false;
        }
        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            EmailErro = EmailInvalidoErrorMsg;
            return false;
        }
        EmailErro = "";
        return true;
    }

    public bool ValidarTelefone(string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
        {
            TelefoneErro = TelefoneVazioErrorMsg;
            return false;
        }
        if (telefone.Length < 9)
        {
            TelefoneErro = TelefoneInvalidoErrorMsg;
            return false;
        }
        TelefoneErro = "";
        return true;
    }
    public bool ValidarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
        {
            SenhaErro = SenhaVazioErrorMsg;
            return false;
        }
        if (senha.Length < 8 || !Regex.IsMatch(senha, @"[a-zA-Z]") || !Regex.IsMatch(senha, @"\d"))
        {
            SenhaErro = SenhaInvalidoErrorMsg;
            return false;
        }
        SenhaErro = "";
        return true;
    }

}
