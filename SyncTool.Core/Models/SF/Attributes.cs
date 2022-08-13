using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.SF;
public partial class Attributes
{
    [JsonProperty("type")]
    public string Type
    {
        get; set;
    }

    [JsonProperty("url")]
    public string Url
    {
        get; set;
    }
}
