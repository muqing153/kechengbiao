
using RestSharp;

namespace UiDesktopApp1;

public class Api
{
    public const string api = "http://10.1.2.1";
    //public static async Task<string> post(string url, string[][] data)
    //{
    //    var client = new RestClient("http://10.1.2.1/jsxsd/framework/mainV_index_loadkb.htmlx?rq=2024-09-22&sjmsValue=EB936BEF0E0F4E30AC2FA620DB0A6CB3&xnxqid=2024-2025-1&xswk=false");

    //    var request = new RestRequest(Method.GET);
    //    request.AddHeader("Pragma", "no-cache");
    //    request.AddHeader("X-Requested-With", "XMLHttpRequest");
    //    request.AddHeader("Cookie", "bzb_jsxsd=6E84C4FBD2A95A85921608E080F93811; SERVERID=123");
    //    client.UserAgent = "Apifox/1.0.0 (https://apifox.com)";
    //    request.AddHeader("Accept", "*/*");
    //    request.AddHeader("Host", "10.1.2.1");
    //    request.AddHeader("Connection", "keep-alive");
    //    IRestResponse response = client.Execute(request);
    //    Console.WriteLine(response.Content);
    //    return string.Empty;
    //}


    public static async Task<string> get(string url, string[][] data)
    {
        var client = new RestClient(api);
        var request = new RestRequest("/jsxsd/framework/mainV_index_loadkb.htmlx",Method.Get);
        request.AddHeader("Pragma", "no-cache");
        request.AddHeader("X-Requested-With", "XMLHttpRequest");
        request.AddHeader("Cookie", "bzb_jsxsd=6E84C4FBD2A95A85921608E080F93811;bzb_njw=87E901D210AC0072EF596E8939D58B2A;SERVERID=123");
        request.AddHeader("Accept", "*/*");
        request.AddHeader("Host", "10.1.2.1");
        request.AddHeader("Connection", "keep-alive");

        foreach (var item in data)
        {
            request.AddParameter(item[0], item[1]);
        }
        var response = await client.ExecuteAsync(request);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return string.Empty;
        }else if (response.Content==null)
        {
            return string.Empty;
        }
        return response.Content;
    }

    public static async Task<string> getkechengbiao()
    {
       return await get("/jsxsd/framework/mainV_index_loadkb.htmlx",
        [
            ["rq","2024-09-23"],
            ["sjmsValue", "EB936BEF0E0F4E30AC2FA620DB0A6CB3"],
            ["xnxqid", "2024-2025-1"],
            ["xswk", "false"],
        ]);
    }
}
