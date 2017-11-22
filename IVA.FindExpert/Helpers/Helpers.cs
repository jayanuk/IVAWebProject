using IVA.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
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
        string message = "SOLK - Service Outfitter for Sri Lanka  \n Your verification Code: " + Code;
        await SendMessage(Phone, message);
    }

    public static async Task SendMessage(string Phone, string Message)
    {
        var smsGateway = System.Configuration.ConfigurationManager.AppSettings[Constant.ConfigurationKeys.SMS_GatewayURL];
        var authCode = System.Configuration.ConfigurationManager.AppSettings[Constant.ConfigurationKeys.SMS_GatewayAuthCode];

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
        var tokenUrl = System.Configuration.ConfigurationManager.AppSettings[Constant.ConfigurationKeys.Token_Url];
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

public class DynamicEntity : DynamicObject
{
    private IDictionary<string, object> _values;

    public DynamicEntity(IDictionary<string, object> values)
    {
        _values = values;
    }
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        if (_values.ContainsKey(binder.Name))
        {
            result = _values[binder.Name];
            return true;
        }
        result = null;
        return false;
    }

    /*
    usage:
    var values = new Dictionary<string, object>();
    values.Add("Title", "Hello World!");
    values.Add("Text", "My first post");
    values.Add("Tags", new[] { "hello", "world" });

    var post = new DynamicEntity(values);

    dynamic dynPost = post;
    var text = dynPost.Text;
     */
}