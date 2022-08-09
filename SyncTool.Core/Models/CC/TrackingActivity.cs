using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class TrackingActivity
{
    [JsonProperty("contact_id")]
    public Guid ContactId
    {
        get; set;
    }

    [JsonProperty("campaign_activity_id")]
    public Guid CampaignActivityId
    {
        get; set;
    }

    [JsonProperty("tracking_activity_type")]
    public string TrackingActivityType
    {
        get; set;
    }

    [JsonProperty("email_address")]
    public string EmailAddress
    {
        get; set;
    }

    [JsonProperty("first_name")]
    public string FirstName
    {
        get; set;
    }

    [JsonProperty("last_name")]
    public string LastName
    {
        get; set;
    }

    [JsonProperty("created_time")]
    public DateTimeOffset CreatedTime
    {
        get; set;
    }

    [JsonProperty("deleted_at")]
    public DateTimeOffset DeletedAt
    {
        get; set;
    }
}
