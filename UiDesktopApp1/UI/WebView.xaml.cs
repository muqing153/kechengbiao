using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UiDesktopApp1.UI;

/// <summary>
/// WebView.xaml 的交互逻辑
/// </summary>
public partial class WebView : Window
{
    public WebView()
    {
        InitializeComponent();
        //webBrowser.Source="http://pass.qdpec.edu.cn:19580/tpass/login";

        // 监听页面加载完成事件
        webView.NavigationCompleted += WebView_NavigationCompleted;
    }


    private async void WebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
    {
        if (e.IsSuccess)
        {
            await GetCookiesAsync("http://10.1.2.1/jsxsd/framework/xsMainV.htmlx");
        }
        else
        {
            Debug.WriteLine("页面加载失败");
        }
    }

    private async Task GetCookiesAsync(string url)
    {
        var cookieManager = webView.CoreWebView2.CookieManager;
        var cookies = await cookieManager.GetCookiesAsync(url);

        foreach (var cookie in cookies)
        {
            Debug.WriteLine($"Name: {cookie.Name}, Value: {cookie.Value}");
            if (cookie.Name == "bzb_njw") {
                Api.bzb_njw = cookie.Value;
            }else if (cookie.Name == "bzb_jsxsd") {
                Api.bzb_jsxsd = cookie.Value;
            }else if (cookie.Name == "SERVERID") {
                Api.SERVERID = cookie.Value;
            }
        }
    }
}
