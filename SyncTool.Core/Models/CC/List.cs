using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class List
{
    [JsonProperty("list_id")]
    public Guid ListId
    {
        get; set;
    }

    [JsonProperty("name")]
    public string Name
    {
        get; set;
    }

    [JsonProperty("description")]
    public string Description
    {
        get; set;
    }

    [JsonProperty("favorite")]
    public bool Favorite
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

    [JsonProperty("membership_count")]
    public long MembershipCount
    {
        get; set;
    }
}
