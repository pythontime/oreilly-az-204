using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;

class AppConfig
{
    public string TenantId { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string[] Scopes { get; set; } = Array.Empty<string>();
}

class Program
{
    static async Task Main()
    {
        var cfg = LoadConfig();

        var credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
        {
            TenantId = cfg.TenantId,
            ClientId = cfg.ClientId
        });

        var graphClient = new GraphServiceClient(credential, cfg.Scopes);

        var me = await graphClient.Me.GetAsync(r => r.QueryParameters.Select = new[] { "displayName", "userPrincipalName", "mail" });
        Console.WriteLine($"Signed in as: {me?.DisplayName} ({me?.UserPrincipalName ?? me?.Mail})");        
    }

    static AppConfig LoadConfig()
    {
        var json = File.ReadAllText("appsettings.json");
        return JsonSerializer.Deserialize<AppConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}
