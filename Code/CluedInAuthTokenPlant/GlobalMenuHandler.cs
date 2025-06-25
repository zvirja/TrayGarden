using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TrayGarden.Reception.Services;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace CluedInAuthTokenPlant;

public class GlobalMenuHandler : IExtendsGlobalMenu, IChangesGlobalIcon, IClipboardWorks
{
    public static GlobalMenuHandler Instance { get; } = new();
    
    private INotifyIconChangerClient GlobalIconChanger { get; set; }
    
    private IClipboardProvider ClipboardProvider { get; set; }
    
    public bool FillProvidedContextMenuBuilder(IMenuEntriesAppender menuAppender)
    {
        menuAppender.AppentMenuStripItem("Copy CluedIn JWT token", Resources.cluedin, OnCopyTokenClick);
        menuAppender.AppentMenuStripItem("Copy CluedIn Organization ID", Resources.cluedin, OnCopyTokenOrganizationClick);
        return true;
    }

    private async void OnCopyTokenClick(object sender, EventArgs e)
    {
        try
        {
            var token = await GetJwtToken();
            
            ClipboardProvider.SetCurrentClipboardText(token, silent: true);
            GlobalIconChanger.NotifySuccess();
        }
        catch (Exception)
        {
            GlobalIconChanger.NotifyFailed();
        }
    }
    
    private async void OnCopyTokenOrganizationClick(object sender, EventArgs e)
    {
        try
        {
            var token = await GetJwtToken();

            string tokenPayload = token.Split(".")[1];
            var parsedToken = JsonNode.Parse(Base64UrlDecode(tokenPayload));
            var orgId = parsedToken["OrganizationId"].GetValue<string>();
            
            ClipboardProvider.SetCurrentClipboardText(orgId, silent: true);
            GlobalIconChanger.NotifySuccess();
        }
        catch (Exception)
        {
            GlobalIconChanger.NotifyFailed();
        }
    }

    public void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient)
    {
        GlobalIconChanger = notifyIconChangerClient;
    }

    public void StoreClipboardValueProvider(IClipboardProvider provider)
    {
        ClipboardProvider = provider;
    }

    private async Task<string> GetJwtToken()
    {
        var config = PlantConfiguration.Instance;

        var response = await new HttpClient().PostAsync(
            config.AuthUrl.Value,
            new FormUrlEncodedContent(
                new Dictionary<string, string>()
                {
                    ["username"] = config.AuthUser.Value,
                    ["password"] = config.AuthPassword.Value,
                    ["client_id"] = config.AuthOrgName.Value,
                    ["grant_type"] = "password",
                })
        );

        var responseObj = await response.Content.ReadFromJsonAsync<AuthResponse>();
        
        return responseObj.Token;
    }
    
    private static string Base64UrlDecode(string input)
    {
        // Pad the input to make it a valid Base64 string
        string padded = input.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
        }

        byte[] bytes = Convert.FromBase64String(padded);
        return Encoding.UTF8.GetString(bytes);
    }

    private class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }
    }
}