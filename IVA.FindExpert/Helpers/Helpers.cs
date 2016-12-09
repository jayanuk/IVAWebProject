using IVA.Common;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

public class Utility
{
    public static int GenerateRandomNumber()
    {
        Random r = new Random();
        int rnd = r.Next(1000, 10000);
        return rnd;
    }    

    public static async Task SendCode(string Phone, string Code)
    { 
        string message = "FindExpert \n Your verification Code: " + Code;
        await SendMessage(Phone, message);
    }

    public static async Task SendMessage(string Phone, string Message)
    {
        var smsGateway = ConfigurationManager.AppSettings[Constant.ConfigurationKeys.SMS_GatewayURL];
        var authCode = ConfigurationManager.AppSettings[Constant.ConfigurationKeys.SMS_GatewayAuthCode];

        using (var stringContent = new StringContent("destination=" + Phone + "&q=" + authCode + "&message=" + Message,
                                                        System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"))
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(smsGateway, stringContent);
                    var result = await response.Content.ReadAsStringAsync();

                }
                catch (Exception ex)
                {
                }
            }
        }
        
    }

    public static async Task<string> GetToken(string UserName, string Password)
    {
        //var tokenUrl = HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/Token";
        var tokenUrl = ConfigurationManager.AppSettings[Constant.ConfigurationKeys.Token_Url];
        var token = "";

        using (var stringContent = new StringContent("username=" + UserName + "&password=" + Password + "&grant_type=password",
                                                        System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"))
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(tokenUrl, stringContent);
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic tokenObj = JsonConvert.DeserializeObject<object>(result);
                    token = tokenObj.access_token;

                }
                catch (Exception ex)
                {
                }
            }
        }

        return token;        
    }
}