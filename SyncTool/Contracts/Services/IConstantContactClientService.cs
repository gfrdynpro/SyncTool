using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncTool.Core.Models.CC;
using SyncTool.Models;

namespace SyncTool.Contracts.Services;
public interface IConstantContactClientService
{
    string GetClientID();
    Token GetToken();
    string BuildUserAuthUrl();
    string ExtractCode(string codeuri);
    Task<Token> RequestAccessTokenAsync(string auth_code);
    Task<Token> RefreshAccessTokenAsync();
    Task<List<Campaign>> GetCampaignsAsync();
    Task<Campaign> GetCampaignDetailsAsync(Guid CampaignId);
    Task<List<TrackingActivity>> GetAllTrackingActivitiesAsync(Guid CampaignActivityId);
}
