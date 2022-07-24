using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncTool.Contracts.Services;
public interface IConstantContactClientService
{
    string BuildUserAuthUrl(string client_id);
    Task<string> RequestUserAuthAsync(string client_id);
}
