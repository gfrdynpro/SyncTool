using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SyncTool.Models;
public class Token
{
    [JsonProperty("access_token")]
    public string AccessToken
    {
        get; set;
    }

    [JsonProperty("token_type")]
    public string TokenType
    {
        get; set;
    }

    [JsonProperty("refresh_token")]
    public string RefreshToken
    {
        get; set;
    }

    [JsonProperty("expires_in")]
    public int ExpiresIn
    {
        get; set;
    }

    [JsonProperty("scope")]
    public string Scope
    {
        get; set;
    }

    public DateTime ExpiryDate
    {
        get; set;
    }

}
