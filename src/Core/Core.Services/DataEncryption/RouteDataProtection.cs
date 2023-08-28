using Core.Services.Interfaces;
using Core.SharedKernel.Data.Enums;
using Microsoft.AspNetCore.DataProtection;

namespace Core.Services.DataEncryption;
public class RouteDataProtection : IDataEncryption
{
    private readonly IDataProtector _dataProtector;

    public RouteDataProtection(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(DataProtectionPurpose.RouteValues.ToString());
    }

    public string Encrypt(string plainInput)
    {
        return _dataProtector.Protect(plainInput);
    }

    public string Decrypt(string cipherText)
    {
        return _dataProtector.Unprotect(cipherText);
    }
}
