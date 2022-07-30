using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class PhoneNumber
{
    [JsonProperty("phone_number_id")]
    public Guid PhoneNumberId
    {
        get; set;
    }

    [JsonProperty("phone_number")]
    public string PhoneNumberPhoneNumber
    {
        get; set;
    }

    [JsonProperty("kind")]
    public string Kind
    {
        get; set;
    }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt
    {
        get; set;
    }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt
    {
        get; set;
    }

    [JsonProperty("update_source")]
    public string UpdateSource
    {
        get; set;
    }

    [JsonProperty("create_source")]
    public string CreateSource
    {
        get; set;
    }
}
