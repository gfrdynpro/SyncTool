using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using SyncTool.Contracts.Services;
using Windows.Security.Authentication.Web;

namespace SyncTool.Services;
public class ConstantContactClientService : IConstantContactClientService
{
    private string _codeChallenge;
    private string _codeVerifier;
    private string _lastError;

    public string BuildUserAuthUrl(string client_id)
    {
        var code = string.Empty;
        _codeVerifier = GenerateCodeVerifier();
        _codeChallenge = GenerateCodeChallenge(_codeVerifier);

        var CCURL = "https://authz.constantcontact.com/oauth2/default/v1/authorize?response_type=code&client_id="
                + client_id
                + "&redirect_uri=" + Uri.EscapeDataString("https://localhost/SyncTool")
                + "&scope=contact_data+campaign_data+offline_access"
                + "&state=12345"
                + "&code_challenge=" + _codeChallenge
                + "&code_challenge_method=S256";
        return CCURL;
    }

    /// <summary>
    /// Makes initial request to Constant Contact API to obtain a user authorization code
    /// This is later used to obtain a token using full client credentials
    /// </summary>
    /// <param name="client_id">The provided Client ID for the app</param>
    /// <returns>Authorization Code</returns>
    public async Task<string> RequestUserAuthAsync(string client_id)
    {
        var code = string.Empty;
        _codeVerifier = GenerateCodeVerifier();
        _codeChallenge = GenerateCodeChallenge(_codeVerifier);

        try
        {
            var CCURL = "https://authz.constantcontact.com/oauth2/default/v1/authorize?response_type=code&client_id="
                + client_id
                + "&redirect_uri=" + Uri.EscapeDataString("https://localhost/SyncTool")
                + "&scope=contact_data+campaign_data+offline_access"
                + "&state=12345"
                + "&code_challenge=" + _codeChallenge
                + "&code_challenge_method=S256";


            Debug.WriteLine("User Auth URL: " + CCURL);

            Uri StartUri = new Uri(CCURL);
            Uri EndUri = new Uri("https://localhost/SyncTool");

            WebAuthenticationResult webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                            WebAuthenticationOptions.None,
                                            StartUri,
                                            EndUri);

            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var codeuri = webAuthenticationResult.ResponseData.ToString();
                code = ExtractCode(codeuri);
                Debug.WriteLine("User Auth Code: " + code);
            }
            else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                _lastError = webAuthenticationResult.ResponseErrorDetail.ToString();
                Debug.WriteLine("User Cancel returned by AuthenticateAsync() : " + _lastError);
                code = null;
                _lastError = webAuthenticationResult.ResponseErrorDetail.ToString();
            }
            else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                _lastError = webAuthenticationResult.ResponseErrorDetail.ToString();
                Debug.WriteLine("HTTP Error returned by AuthenticateAsync() : " + _lastError);
                code = null;
                _lastError = webAuthenticationResult.ResponseErrorDetail.ToString();
            }
            else
            {
                _lastError = webAuthenticationResult.ResponseErrorDetail.ToString();
                Debug.WriteLine("Error returned by AuthenticateAsync() : " + _lastError);
                code = null;
            }
        }
        catch (Exception error)
        {
            // Do something with errors here
            _lastError = error.Message;
            Debug.WriteLine(_lastError);
            code = null;
        }
        return code;
    }

    private string ExtractCode(string codeuri)
    {
        var queryString = new Uri(codeuri).Query;
        var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
        return queryDictionary["code"];
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
}
