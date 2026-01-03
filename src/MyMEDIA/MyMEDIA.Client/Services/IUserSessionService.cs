using MyMEDIA.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Client.Services;

public interface IUserSessionService
{
    Task Login(Token token);
    Task<Token?> GetToken();
    Task Logout();
    Task<bool> IsUserLoggedIn();
}
