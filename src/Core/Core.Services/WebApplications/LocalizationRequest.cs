using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Core.Services.WebApplications;

public static class LocalizationRequest
{
    public static void LocalizationRequestSetup(WebApplication app)
    {
        // List of supported cultures for localization used in RequestLocalizationOptions
        var supportedCultures = new[]
        {
            new CultureInfo("en"),
            new CultureInfo("es")
        };

        // Configure RequestLocalizationOptions with supported culture
        var requestLocalizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),

            // Formatting numbers, date etc.
            SupportedCultures = supportedCultures,

            // UI strings that are localized
            SupportedUICultures = supportedCultures
        };
        // Enable Request Localization
        app.UseRequestLocalization(requestLocalizationOptions);
    }
}
