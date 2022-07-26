using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using SyncTool.Contracts.Services;
using SyncTool.Helpers;

using Windows.ApplicationModel;

namespace SyncTool.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    readonly Windows.Storage.ApplicationDataContainer localSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;

    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    private string _constantContactAPIKey;
    public string ConstantContactAPIKey
    {
        get => _constantContactAPIKey;
        set
        {
            SetProperty(ref _constantContactAPIKey, value);
            localSettings.Values["ccAPIKey"] = value;
        }
    }

    private string _salesforceAPIKey;
    public string SalesforceAPIKey
    {
        get => _salesforceAPIKey;
        set
        {
            SetProperty(ref _salesforceAPIKey, value);
            localSettings.Values["sfAPIKey"] = value;
        }
    }

    private string _versionDescription;

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    private ICommand _switchThemeCommand;

    public ICommand SwitchThemeCommand
    {
        get
        {
            if (_switchThemeCommand == null)
            {
                _switchThemeCommand = new RelayCommand<ElementTheme>(
                    async (param) =>
                    {
                        if (ElementTheme != param)
                        {
                            ElementTheme = param;
                            await _themeSelectorService.SetThemeAsync(param);
                        }
                    });
            }

            return _switchThemeCommand;
        }
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        VersionDescription = GetVersionDescription();
        ConstantContactAPIKey = localSettings.Values["ccAPIKey"] as string;
        SalesforceAPIKey = localSettings.Values["sfAPIKey"] as string;
    }

    private static string GetVersionDescription()
    {
        var appName = "AppDisplayName".GetLocalized();
        var version = Package.Current.Id.Version;
        return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
