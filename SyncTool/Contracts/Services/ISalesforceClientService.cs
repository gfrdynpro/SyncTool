using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncTool.Models;

namespace SyncTool.Contracts.Services;
public interface ISalesforceClientService
{
    string GetClientID();
    Token GetToken();
    string BuildUserAuthUrl();
    string ExtractCode(string codeuri);
    Task<Token> RequestAccessTokenAsync(string auth_code);
    Task<Token> RefreshAccessTokenAsync();
    Task<object> GetLeadByEmailAddressAsync(string emailAddress);
}
