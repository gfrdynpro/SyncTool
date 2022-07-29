using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class NextNext
{
    [JsonProperty("href")]
    public string Href
    {
        get; set;
    }
}
