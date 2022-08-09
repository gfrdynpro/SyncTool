using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class Campaign
{
    [JsonProperty("campaign_activities")]
    public CampaignActivity[] CampaignActivities
    {
        get; set;
    }

    [JsonProperty("campaign_id")]
    public Guid CampaignId
    {
        get; set;
    }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt
    {
        get; set;
    }

    [JsonProperty("current_status")]
    public string CurrentStatus
    {
        get; set;
    }

    [JsonProperty("name")]
    public string Name
    {
        get; set;
    }

    [JsonProperty("type")]
    public string Type
    {
        get; set;
    }

    [JsonProperty("type_code")]
    public long TypeCode
    {
        get; set;
    }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt
    {
        get; set;
    }
}
