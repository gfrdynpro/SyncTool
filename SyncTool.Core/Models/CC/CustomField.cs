using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class CustomField
{
    [JsonProperty("custom_field_id")]
    public Guid CustomFieldId
    {
        get; set;
    }

    [JsonProperty("value")]
    public string Value
    {
        get; set;
    }
}
