using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class CampaignActivity
{
    [JsonProperty("campaign_activity_id")]
    public Guid CampaignActivityId
    {
        get; set;
    }

    [JsonProperty("role")]
    public string Role
    {
        get; set;
    }
}
