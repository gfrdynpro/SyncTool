using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncTool.Core.Services;

namespace SyncTool.Core.Test;

[TestClass]
public class ConstantContact
{
    private static string _cckey = "";
    private static string _sfkey = "";

    [AssemblyInitialize]
    public static void Setup(TestContext testContext)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<ConstantContact>().Build();
        _cckey = configuration["CCAPIKey"];
        _sfkey = configuration["SFAPIKey"];
    }

    [TestMethod]
    public void TestLogin()
    {
        //var code = CCDataService.RequestUserAuthAsync(_cckey);
        Assert.IsNotNull("hello");
    }
}