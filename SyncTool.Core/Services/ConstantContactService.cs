using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using SyncTool.Core.Contracts.Services;

namespace SyncTool.Core.Services;
public class ConstantContactService : IConstantContactService
{
    public bool Login(string APIkey)
    {
        Debug.WriteLine($"API Key: {APIkey}");
        return true;
    }
}