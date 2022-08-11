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

    [JsonProperty("device_type")]
    public string DeviceType
    {

        get;set;
    }

    [JsonProperty("url_id")]
    public string UrlId
    {
        get;set;
    }

    [JsonProperty("link_url")]
    public string LinkUrl
    {
        get;set; 
    }

    [JsonProperty("opt_out_reason")]
    public string OptOutReason
    {
        get;set;
    }

    [JsonProperty("bounce_code")]
    public string BounceCode
    {
        get;set;
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

    public bool Sent
    {
        get;set;
    }
}
