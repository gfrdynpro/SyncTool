using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using SyncTool.Contracts.Services;

namespace SyncTool.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private string _browserSource;
    public string BrowserSource
    {
        get => _browserSource;
        set => SetProperty(ref _browserSource, value);
    }

    public MainViewModel()
    {
        BrowserSource = "https://www.google.com";
        RunCCAuthTestCommand = new RelayCommand(RunCCAuthTest);
        RunSFAuthTestCommand = new RelayCommand(RunSFAuthTest);
    }

    public ICommand RunCCAuthTestCommand { get; }

    public ICommand RunSFAuthTestCommand { get; }

    private void RunCCAuthTest()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<MainViewModel>().Build();
        var _cckey = configuration["CCAPIKey"];
        var ccService = App.GetService<IConstantContactClientService>();
        // var code = ccService.RequestUserAuthAsync(_cckey);
        var authURL = ccService.BuildUserAuthUrl(_cckey);
        BrowserSource = authURL;
        //BrowserSource = "https://localhost/SyncTool";
    }

    private void RunSFAuthTest()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<MainViewModel>().Build();
        var _sfkey = configuration["SFAPIKey"];
    }

    public void OnNavigationStarting()
    {
        Debug.WriteLine("NAVIGATION STARTING");
        Debug.WriteLine($"Target: {BrowserSource}");
        if (BrowserSource.StartsWith("https://localhost/SyncTool"))
        {
            BrowserSource = "about:blank";
        }
    }
}
