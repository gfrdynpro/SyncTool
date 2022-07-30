using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class EmailAddress
{
    [JsonProperty("address")]
    public string Address
    {
        get; set;
    }

    [JsonProperty("permission_to_send")]
    public string PermissionToSend
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

    [JsonProperty("opt_in_source")]
    public string OptInSource
    {
        get; set;
    }

    [JsonProperty("opt_in_date")]
    public DateTimeOffset OptInDate
    {
        get; set;
    }

    [JsonProperty("opt_out_source")]
    public string OptOutSource
    {
        get; set;
    }

    [JsonProperty("opt_out_date")]
    public DateTimeOffset OptOutDate
    {
        get; set;
    }

    [JsonProperty("opt_out_reason")]
    public string OptOutReason
    {
        get; set;
    }

    [JsonProperty("confirm_status")]
    public string ConfirmStatus
    {
        get; set;
    }
}
