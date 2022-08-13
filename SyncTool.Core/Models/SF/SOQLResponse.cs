using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.SF;
public partial class SOQLResponse
{
    [JsonProperty("totalSize")]
    public long TotalSize
    {
        get; set;
    }

    [JsonProperty("done")]
    public bool Done
    {
        get; set;
    }

    [JsonProperty("records")]
    public Record[] Records
    {
        get; set;
    }
}
