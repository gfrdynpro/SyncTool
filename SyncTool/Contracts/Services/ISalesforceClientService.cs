using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncTool.Core.Models.SF;
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
    Task<Core.Models.SF.Record> GetLeadByEmailAddressAsync(string emailAddress);
    Task<bool> UpdateLeadRecordAsync(Record sfLead, Dictionary<string, object> payload);
}
