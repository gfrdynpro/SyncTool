using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class Links
{
    [JsonProperty("next")]
    public LinksNext Next
    {
        get; set;
    }
}
