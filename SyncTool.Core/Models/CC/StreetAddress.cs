using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class StreetAddress
{
    [JsonProperty("street_address_id")]
    public Guid StreetAddressId
    {
        get; set;
    }

    [JsonProperty("kind")]
    public string Kind
    {
        get; set;
    }

    [JsonProperty("street")]
    public string Street
    {
        get; set;
    }

    [JsonProperty("city")]
    public string City
    {
        get; set;
    }

    [JsonProperty("state")]
    public string State
    {
        get; set;
    }

    [JsonProperty("postal_code")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long PostalCode
    {
        get; set;
    }

    [JsonProperty("country")]
    public string Country
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
}
