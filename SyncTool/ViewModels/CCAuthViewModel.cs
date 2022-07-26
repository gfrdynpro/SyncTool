using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using SyncTool.Contracts.Services;

namespace SyncTool.ViewModels;
public class CCAuthViewModel : ObservableRecipient
{
    private readonly Windows.Storage.ApplicationDataContainer localSettings = 
        Windows.Storage.ApplicationData.Current.LocalSettings;
    private readonly IConstantContactClientService _ccService;
    private readonly string _cckey;

    public CCAuthViewModel()
    {
        BrowserSource = "about:blank";
        RunCCAuthCommand = new RelayCommand(RunCCAuth);
        _ccService = App.GetService<IConstantContactClientService>();
        // var configuration = new ConfigurationBuilder().AddUserSecrets<MainViewModel>().Build();
        // _cckey = configuration["CCAPIKey"];
        _cckey = localSettings.Values["ccAPIKey"] as string;
        if (_cckey != null && _cckey.Length > 5)
        {
            MissingAPIKey = false;
        } else
        {
            MissingAPIKey = true;
        }
    }

    public ICommand RunCCAuthCommand { get; }

    private void RunCCAuth()
    {
        if (_cckey != null && _cckey.Length > 5)
        {
            MissingAPIKey = false;
            var authURL = _ccService.BuildUserAuthUrl(_cckey);
            BrowserSource = authURL;
        }
        else
        {
            MissingAPIKey = true;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public async void OnNavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs e)
    {
        Debug.WriteLine("NAVIGATION STARTING");
        Debug.WriteLine($"Event Args Url: {e.Uri}");
        if (e.Uri.StartsWith("https://localhost/SyncTool"))
        {
            AuthCode = _ccService.ExtractCode(e.Uri);
            BrowserSource = "about:blank";
            if(e.Uri.Length > 0)
            {
                var result = await _ccService.RequestAccessTokenAsync(_cckey, AuthCode);
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                RenewalDateTime = result.ExpiryDate.ToString(@"yyyy-MM-dd hh:mm:ss tt");
                Debug.WriteLine(result);
            }
        }
    }

    private bool _missingAPIKey;
    public bool MissingAPIKey
    { 
        get => _missingAPIKey;
        set => SetProperty(ref _missingAPIKey, value);
    }

    private string _browserSource;
    public string BrowserSource
    {
        get => _browserSource;
        set => SetProperty(ref _browserSource, value);
    }

    // NOTE. All details should be real-time pulled from ConstantContactClientService and not cached here
    // except for the AUTH code as it is used only on login. Easier to just copy here for now.
    private string _authCode;
    public string AuthCode
    {
        get => _authCode;
        set => SetProperty(ref _authCode, value);
    }

    private string _accessToken;
    public string AccessToken
    {
        get => _accessToken;
        set => SetProperty(ref _accessToken, value);
    }

    private string _refreshToken;
    public string RefreshToken
    {
        get => _refreshToken;
        set => SetProperty(ref _refreshToken, value);
    }

    private string _renewalDateTime;
    public string RenewalDateTime
    {
        get => _renewalDateTime;
        set => SetProperty(ref _renewalDateTime, value);
    }
}
