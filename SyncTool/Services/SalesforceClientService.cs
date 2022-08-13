using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SyncTool.Contracts.Services;
using SyncTool.Core.Services;
using SyncTool.Models;
using Windows.Web.Http;

namespace SyncTool.Services;
public class SalesforceClientService : ISalesforceClientService
{
    private string _client_id;
    private string _client_secret;
    private string _codeChallenge;
    private string _codeVerifier;
    private Token _authToken;

    public SalesforceClientService()
    {
        var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        _client_id = settings.Values["sfAPIKey"] as string;
        _client_secret = settings.Values["sfAPISecret"] as string;
        if (settings.Values["SFToken"] is string result)
        {
            _authToken = JsonConvert.DeserializeObject<Token>(result);
        }
    }

    public string BuildUserAuthUrl()
    {
        var CCURL = "https://cloudalyzepartners.my.salesforce.com/services/oauth2/authorize?response_type=code&client_id="
        + _client_id
        + "&redirect_uri=" + Uri.EscapeDataString("https://localhost/SyncTool")
        + "&state=123456"
        + "display=popup";
        return CCURL;
    }

    public string GetClientID() => _client_id;
    
    public Token GetToken() => _authToken;

    public string ExtractCode(string codeuri)
    {
        var queryString = new Uri(codeuri).Query;
        var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
        return queryDictionary["code"];
    }

    public async Task<Token> RefreshAccessTokenAsync()
    {
        var token = new Token();

        Debug.WriteLine("**** START REFRESH ACCESS TOKEN ****");

        try
        {
            var client = new HttpClient();
            Uri ccRefreshURL = new Uri("https://cloudalyzepartners.my.salesforce.com/services/oauth2/token");
            var payload = new Dictionary<string, string>();
            payload.Add("client_id", _client_id);
            payload.Add("client_secret", _client_secret);
            payload.Add("refresh_token", _authToken.RefreshToken);
            payload.Add("grant_type", "refresh_token");
            var byteContent = new HttpFormUrlEncodedContent(payload);
            var response = await client.PostAsync(ccRefreshURL, byteContent);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<Token>(body);
                token.ExpiryDate = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                _authToken = token;
                // Also stash away for future use
                var strToken = JsonConvert.SerializeObject(_authToken);
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                settings.Values["SFToken"] = strToken;

            }
            else
            {
                Debug.WriteLine(response.Content);
                Debug.WriteLine(response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return token;
    }

    public async Task<Token> RequestAccessTokenAsync(string auth_code)
    {
        var token = new Token();

        Debug.WriteLine("**** START GET ACCESS TOKEN ****");

        try
        {
            var client = new HttpClient();

            Uri ccTokenURL = new Uri("https://cloudalyzepartners.my.salesforce.com/services/oauth2/token");

            var payload = new Dictionary<string, string>();
            payload.Add("client_id", _client_id);
            payload.Add("client_secret", _client_secret);
            payload.Add("redirect_uri", "https://localhost/SyncTool");
            payload.Add("code", auth_code);
            payload.Add("grant_type", "authorization_code");

            var byteContent = new HttpFormUrlEncodedContent(payload);
            var response = await client.PostAsync(ccTokenURL, byteContent);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<Token>(body);
                token.ExpiryDate = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                _authToken = token;
                // Also stash away for future use
                var strToken = JsonConvert.SerializeObject(_authToken);
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                settings.Values["SFToken"] = strToken;

            }
            else
            {
                Debug.WriteLine(response.Content);
                Debug.WriteLine(response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return token;
    }

    /// <summary>
    /// Takes a Code Verifier string and generates a URI safe Code Challenge string
    /// </summary>
    /// <param name="codeVerifier"></param>
    /// <returns>Code Challenge</returns>
    private string GenerateCodeChallenge(string codeVerifier)
    {
        //generate the code challenge based on the verifier
        string codeChallenge;
        using (var sha256 = SHA256.Create())
        {
            var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));

            Debug.WriteLine("Code Challenge Before Cleanup: " + Convert.ToBase64String(challengeBytes));

            codeChallenge = Convert.ToBase64String(challengeBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        Debug.WriteLine("Code Challenge After Cleanup: " + codeChallenge);

        return codeChallenge;
    }

    /// <summary>
    /// Generates a random 32 byte string that is URI safe to use as a Code Verifier
    /// </summary>
    /// <returns>Code Verifier</returns>
    private string GenerateCodeVerifier()
    {
        //Generate a random string for our code verifier
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);

        Debug.WriteLine("Code Verifier Before Cleanup: " + Convert.ToBase64String(bytes));

        var codeVerifier = Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        Debug.WriteLine("Code Verifier After Cleanup: " + codeVerifier);
        Debug.WriteLine("Code Verifer Length is (min 43): " + codeVerifier.Length);

        return codeVerifier;
    }

    public async Task<Core.Models.SF.Record> GetLeadByEmailAddressAsync(string emailAddress)
    {
        var query = $"SELECT+Id,+Name+from+Lead+WHERE+Email='{emailAddress}'";
        var apiClient = new HttpDataService("https://cloudalyzepartners.my.salesforce.com");
        try
        {
            var response = await apiClient.GetAsync<Core.Models.SF.SOQLResponse>("/services/data/v55.0/query/?q=" + query, _authToken.AccessToken);
            if (response.Done == true && response.TotalSize > 0)
            {
                return response.Records.FirstOrDefault();
            }
            return null;
        } catch (System.Net.Http.HttpRequestException ex)
        {
            Debug.WriteLine(ex);
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                await this.RefreshAccessTokenAsync();
                var response = await apiClient.GetAsync<Core.Models.SF.SOQLResponse>("/services/data/v55.0/query/?q=" + query, _authToken.AccessToken);
                if (response.Done == true && response.TotalSize > 0)
                {
                    return response.Records.FirstOrDefault();
                }
                return null;
            } 
            else
            {
                return null;
            }
        }
    }
}
