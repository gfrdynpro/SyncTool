using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class Note
{
    [JsonProperty("note_id")]
    public Guid NoteId
    {
        get; set;
    }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt
    {
        get; set;
    }

    [JsonProperty("content")]
    public string Content
    {
        get; set;
    }
}
