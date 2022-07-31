using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using SyncTool.Contracts.Services;
using SyncTool.Models;
using SyncTool.Services;

namespace SyncTool.ViewModels;
public partial class SFAuthViewModel : ObservableRecipient
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    private readonly Windows.Storage.ApplicationDataContainer localSettings =
        Windows.Storage.ApplicationData.Current.LocalSettings;
    private readonly ISalesforceClientService _sfService;

    public SFAuthViewModel()
    {
        BrowserSource = "about:blank";
        RunSFAuthCommand = new RelayCommand(RunSFAuth);
        _sfService = App.GetService<ISalesforceClientService>();
        UpdateTokenDisplay(_sfService.GetToken());
        var _sfkey = _sfService.GetClientID();
        if (_sfkey != null && _sfkey.Length > 5)
        {
            MissingAPIKey = false;
        }
        else
        {
            MissingAPIKey = true;
        }
    }

    public ICommand RunSFAuthCommand
    {
        get;
    }

    private void RunSFAuth()
    {
    
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public async void OnNavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs e)
    {
        Debug.WriteLine("NAVIGATION STARTING");
        Debug.WriteLine($"Event Args Url: {e.Uri}");
        if (e.Uri.StartsWith("https://localhost/SyncTool"))
        {
            AuthCode = _sfService.ExtractCode(e.Uri);
            BrowserSource = "about:blank";
            if (e.Uri.Length > 0)
            {
                var result = await _sfService.RequestAccessTokenAsync(AuthCode);
                UpdateTokenDisplay(result);
                Debug.WriteLine(result);
            }
        }
    }

    private void UpdateTokenDisplay(Token result)
    {
        if (result != null)
        {
            AccessToken = result.AccessToken;
            RefreshToken = result.RefreshToken;
            RenewalDateTime = result.ExpiryDate.ToString(@"yyyy-MM-dd hh:mm:ss tt");
        }
    }

    [ObservableProperty]
    public bool missingAPIKey;

    [ObservableProperty]
    public string browserSource;

    // NOTE. All details should be real-time pulled from SalesforceClientService and not cached here
    // except for the AUTH code as it is used only on login. Easier to just copy here for now.
    [ObservableProperty]
    private string authCode;

    [ObservableProperty]
    private string accessToken;

    [ObservableProperty]
    private string refreshToken;

    [ObservableProperty]
    private string renewalDateTime;
}
