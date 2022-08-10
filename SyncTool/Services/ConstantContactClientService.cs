using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using SyncTool.Contracts.Services;
using SyncTool.Core.Models.CC;
using SyncTool.Core.Services;
using SyncTool.Models;
using Windows.Security.Authentication.Web;
using Windows.Web.Http;

namespace SyncTool.Services;
public class ConstantContactClientService : IConstantContactClientService
{
    private string _client_id;
    private string _codeChallenge;
    private string _codeVerifier;
    private Token _authToken;

    public ConstantContactClientService()
    {
        var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        _client_id = settings.Values["ccAPIKey"] as string;
        if (settings.Values["CCToken"] is string result)
        {
            _authToken = JsonConvert.DeserializeObject<Token>(result);
        }

    }

    public string GetClientID()
    {
        return _client_id;
    }

    public Token GetToken()
    {
        return _authToken;
    }

    public string BuildUserAuthUrl()
    {
        _codeVerifier = GenerateCodeVerifier();
        _codeChallenge = GenerateCodeChallenge(_codeVerifier);

        var CCURL = "https://authz.constantcontact.com/oauth2/default/v1/authorize?response_type=code&client_id="
                + _client_id
                + "&redirect_uri=" + Uri.EscapeDataString("https://localhost/SyncTool")
                + "&scope=contact_data+campaign_data+offline_access"
                + "&state=12345"
                + "&code_challenge=" + _codeChallenge
                + "&code_challenge_method=S256";
        return CCURL;
    }

    public string ExtractCode(string codeuri)
    {
        var queryString = new Uri(codeuri).Query;
        var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
        return queryDictionary["code"];
    }

    public async Task<Token> RequestAccessTokenAsync(string auth_code)
    {
        var token = new Token();

        Debug.WriteLine("**** START GET ACCESS TOKEN ****");

        try
        {
            var client = new HttpClient();

            Uri ccTokenURL = new Uri("https://authz.constantcontact.com/oauth2/default/v1/token");

            var payload = new Dictionary<string, string>();
            payload.Add("client_id", _client_id);
            payload.Add("redirect_uri", "https://localhost/SyncTool");
            payload.Add("code", auth_code);
            payload.Add("code_verifier", _codeVerifier);
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
                settings.Values["CCToken"] = strToken;

            }
            else
            {
                Debug.WriteLine(response.Content);
                Debug.WriteLine(response.ReasonPhrase);
            }
        } 
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return token;
    }

    public async Task<Token> RefreshAccessTokenAsync()
    {
        Debug.WriteLine("**** START REFRESH TOKEN ****");

        if (_authToken == null)
        {
            return _authToken;
        }

        try
        {
            var client = new HttpClient();

            Uri ccURL = new Uri("https://authz.constantcontact.com/oauth2/default/v1/token");

            var payload = new Dictionary<string, string>();
            payload.Add("client_id", _client_id);
            payload.Add("redirect_uri", "https://localhost/SyncTool");
            payload.Add("refresh_token", _authToken.RefreshToken);
            payload.Add("grant_type", "refresh_token");
            var byteContent = new HttpFormUrlEncodedContent(payload);

            var response = await client.PostAsync(ccURL, byteContent);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<Token>(body);
                token.ExpiryDate = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                _authToken = token;
                // Also stash away for future use
                var strToken = JsonConvert.SerializeObject(_authToken);
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                settings.Values["CCToken"] = strToken;
            }
            else
            {
                Debug.WriteLine(response.Content);
                Debug.WriteLine(response.ReasonPhrase);
            }
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return _authToken;
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

    public async Task<List<Campaign>> GetCampaignsAsync()
    {
        var theList = new List<Campaign>();
        var apiClient = new HttpDataService("https://api.cc.email");
        var results = await apiClient.GetAsync<CampaignList>("/v3/emails", _authToken.AccessToken);
        theList.AddRange(results.Campaigns);
        var next = results.Links.Next.Href;
        while (next != null)
        {
            var results2 = await apiClient.GetAsync<CampaignList>(next, _authToken.AccessToken);
            theList.AddRange(results2.Campaigns);
            next = results2.Links?.Next.Href; 
        }
        return theList;
    }

    public async Task<Campaign> GetCampaignDetailsAsync(Guid CampaignId)
    {
        var apiClient = new HttpDataService("https://api.cc.email");
        var details = await apiClient.GetAsync<Campaign>("/v3/emails/" + CampaignId.ToString(), _authToken.AccessToken);
        return details;
    }

    // Build up a full list of Tracking Activities - not sure why cannot get them all in one go.
    // Get /reports/email_reports/{campaign_activity_id}/tracking/opens
    // Get /reports/email_reports/{campaign_activity_id}/tracking/optouts
    // Get /reports/email_reports/{campaign_activity_id}/tracking/bounces
    // Get /reports/email_reports/{campaign_activity_id}/tracking/didnotopens
    // Get /reports/email_reports/{campaign_activity_id}/tracking/clicks
    public async Task<List<TrackingActivity>> GetAllTrackingActivitiesAsync(Guid CampaignActivityId)
    {
        var theList = new List<TrackingActivity>();
        var apiClient = new HttpDataService("https://api.cc.email");
        var opens = await apiClient.GetAsync<TrackingActivitiesList>($"/v3/reports/email_reports/{CampaignActivityId}/tracking/opens", _authToken.AccessToken);
        theList.AddRange(opens.TrackingActivities);
        return theList;
    }
}
