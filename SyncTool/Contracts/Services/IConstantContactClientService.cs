using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncTool.Models;

namespace SyncTool.Contracts.Services;
public interface IConstantContactClientService
{
    string BuildUserAuthUrl(string client_id);
    string ExtractCode(string codeuri);
    Task<Token> RequestAccessTokenAsync(string client_id, string auth_code);
    Task<Token> RefreshAccessTokenAsync(string client_id);
}
