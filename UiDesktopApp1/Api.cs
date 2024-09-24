
using RestSharp;
using System.Diagnostics;
using System.IO;
using System.Net;

using Newtonsoft.Json;
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
        request.AddHeader("Cookie", "bzb_jsxsd=692698C435C76D9594027498C7A405FE;bzb_njw=2FA8399F135C4263897A9A1ABA5FB555;SERVERID=123");
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

    public static async Task<string> getkechengbiao(string rq)
    {
        var a = new string[][]
        {

            ["rq", rq],
            ["sjmsValue", "EB936BEF0E0F4E30AC2FA620DB0A6CB3"],
            ["xnxqid", "2024-2025-1"],
            ["xswk", "false"],
        };
        string v = await get("/jsxsd/framework/mainV_index_loadkb.htmlx",
        a);

        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Html")))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Html"));
        }
        string gets = "?";
        foreach (var item in a)
        {
            gets += item[0] + "=" + item[1];
        }
        string originalUrl = $"{api}/jsxsd/framework/mainV_index_loadkb.htmlx{gets}";
        // 对整个URL进行编码
        string encodedUrl = WebUtility.UrlEncode(originalUrl);
        //Debug.WriteLine("原始URL: " + originalUrl);
        //Debug.WriteLine("编码后的URL: " + encodedUrl);
        //Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Html\\", encodedUrl);
        if (v == string.Empty|| IsJson(v))
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }else
        {

            File.WriteAllText(path, v);
            //Debug.WriteLine(v);
            return v;
        }
    }

    public class Message
    {
        public int flag1 { get; set; } = 0;
        public string msgContent { get; set; }=string.Empty;
    }
    public static bool IsJson(string str)
    {
        try
        {
            var obj = JsonConvert.DeserializeObject(str);
            return obj != null;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
