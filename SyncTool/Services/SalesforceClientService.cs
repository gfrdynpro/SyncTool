using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncTool.Contracts.Services;
using SyncTool.Models;

namespace SyncTool.Services;
public class SalesforceClientService : ISalesforceClientService
{
    private string _client_id;
    private string _codeChallenge;
    private string _codeVerifier;
    private Token _authToken;

    public SalesforceClientService()
    {
        var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        _client_id = settings.Values["sfAPIKey"] as string;
        if (settings.Values["SFToken"] is string result)
        {
            _authToken = JsonConvert.DeserializeObject<Token>(result);
        }
    }
    
    public string GetClientID() => _client_id;
    
    public Token GetToken() => _authToken;

    public string ExtractCode(string codeuri) => throw new NotImplementedException();
    
    public Task<Token> RefreshAccessTokenAsync() => throw new NotImplementedException();
    
    public Task<Token> RequestAccessTokenAsync(string auth_code) => throw new NotImplementedException();
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
