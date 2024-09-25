
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using UiDesktopApp1.ViewData;
using Wpf.Ui.Controls;

namespace UiDesktopApp1.UI;
/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : FluentWindow
{
    public const string ApplicationTitle = "课程表";
    public static string academicYear = "2024-2025-1";
    public static string week = "2024-08-26";
    /// <summary>
    /// 获取给定日期下一周的周一日期
    /// </summary>
    /// <param name="date">给定日期</param>
    /// <returns>下一周周一的日期</returns>
    public static DateTime GetNextWeekStart(DateTime date)
    {
        // 获取给定日期所在的那一周的周一
        int daysOffset = DayOfWeek.Monday - date.DayOfWeek;
        if (daysOffset > 0) daysOffset -= 7; // 如果给定日期已经是周一或之后，则需要调整偏移量
        DateTime weekStart = date.AddDays(daysOffset);

        // 返回下一周的周一
        return weekStart.AddDays(7);
    }
    public static List<Zhou> Zhoulist = [];
    public MainWindow()
    {
        InitializeComponent();

        // 给定日期
        DateTime givenDate = new(2024, 8, 26);

        for (int i = 0; i < 20; i++)
        {
            string weekName = $"第{i + 1}周";
            string weekDate = givenDate.ToString("yyyy-MM-dd");

            Zhoulist.Add(new Zhou()
            {
                Name = weekName,
                rq = weekDate
            });
            // 增加一周
            givenDate = givenDate.AddDays(7);
        }

        // 打印结果
        //foreach (var zhou in Zhoulist)
        //{
        //    Debug.WriteLine($"周次: {zhou.Name}, 日期: {zhou.rq}");
        //}

        // 获取下一周的周一和周日
        DateTime nextWeekStart = GetNextWeekStart(givenDate);
        DateTime nextWeekEnd = nextWeekStart.AddDays(6);

        Debug.WriteLine($"下一周的开始日期: {nextWeekStart:yyyy-MM-dd}");
        Debug.WriteLine($"下一周的结束日期: {nextWeekEnd:yyyy-MM-dd}");
        var page = new Page1();
        card.Content = page;

    }
    public static async Task<string> Init(string rq)
    {
        try
        {

            string html = await Api.getkechengbiao(rq);
            Debug.WriteLine(html);
            // 加载 HTML
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 解析表格
            var rows = doc.DocumentNode.SelectNodes("//tr[@align='center']") ?? throw new Exception("没有找到课程表数据");
            // 准备保存数据的对象
            var schedule = new List<Dictionary<string, object>>();

            char[] chars = ['\r', '\n'];

            // 遍历表格的每一行
            foreach (var row in rows)
            {
                var columns = row.SelectNodes(".//td");
                if (columns != null)
                {
                    var rowData = new Dictionary<string, object>();
                    var week = new Dictionary<string, object>();
                    // 遍历每一列
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var columnText = columns[i].InnerText.Trim();
                        if (i == 0)
                        {
                            var result = new Dictionary<string, string>();
                            var aa = columns[i].InnerHtml;

                            // 使用 <br> 作为分隔符，将内容分割成多个部分
                            string[] period = aa.Split(new string[] { "<br>" }, StringSplitOptions.None);
                            if (period != null && period.Length >= 3)
                            {
                                result["Section"] = period[0].Trim();     // 第一大节
                                result["SubSection"] = period[1].Trim(); // 01,02小节
                                result["Time"] = period[2].Trim();       // 08:20-10:00
                            }
                            rowData["Tab"] = result;
                        }
                        else
                        {
                            var columnHtml = columns[i].InnerHtml;

                            var columnDoc = new HtmlDocument();
                            columnDoc.LoadHtml(columnHtml);
                            //// 使用 HtmlDocument 解析
                            var itemBox = columnDoc.DocumentNode.SelectSingleNode("//div[@class='item-box']");
                            //Debug.WriteLine(itemBox.ToString());
                            var itemBoxData = new Dictionary<string, string>();
                            if (itemBox != null)
                            {
                                // 提取 <p> 标签中的内容
                                var courseName = itemBox.SelectSingleNode(".//p")?.InnerText.Trim();
                                if (!string.IsNullOrEmpty(courseName))
                                {
                                    itemBoxData["CourseName"] = courseName; // 大学英语（一）
                                }

                                // 提取学分和小节信息
                                var creditsAndPeriod = itemBox.SelectSingleNode(".//div[@class='tch-name']").InnerHtml;
                                if (!string.IsNullOrEmpty(creditsAndPeriod))
                                {
                                    string pattern = @"</span><span>";

                                    string[] parts = Regex.Split(creditsAndPeriod, pattern);

                                    itemBoxData["CreditsAndPeriod"] = $"{Regex.Replace(parts[0], @"<span>|</span>", "")}/{Regex.Replace(parts[1], @"<span>|</span>", "")}"; // 学分：1 01~02节
                                }

                                // 提取教室和时间信息
                                var classroomAndTime = itemBox.SelectNodes("//div[@class='item-box']/div[2]/span");
                                if (classroomAndTime != null && classroomAndTime.Count >= 2)
                                {
                                    //Debug.WriteLine(classroomAndTime);
                                    itemBoxData["Classroom"] = classroomAndTime[0].InnerText.ToString(); // 博学楼-博学楼215
                                    itemBoxData["Time"] = classroomAndTime[1].InnerText.ToString();      // 第4周 星期三
                                }

                                // 提取班级信息
                                var className = itemBox.SelectSingleNode(".//div[contains(text(), '班级')]")?.InnerText.Trim();
                                if (!string.IsNullOrEmpty(className))
                                {
                                    itemBoxData["ClassName"] = className; // 2024级软件技术3班
                                }
                                week[$"{SwitchWeek(i)}"] = itemBoxData;
                            }
                            else
                            {
                                week[$"{SwitchWeek(i)}"] = null;
                            }
                        }
                    }
                    rowData["date"] = week;
                    schedule.Add(rowData);
                }
            }

            // 转换为 JSON 格式
            return JsonConvert.SerializeObject(schedule, Formatting.Indented);
            //text.Text = json;
            //Debug.WriteLine(json);

        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message);
            return string.Empty;
        }

    }

    private static string SwitchWeek(int week)
    {
        return week switch
        {
            1 => "星期一",
            2 => "星期二",
            3 => "星期三",
            4 => "星期四",
            5 => "星期五",
            6 => "星期六",
            7 => "星期日",
            _ => "未知",
        };
    }

    public static bool IsExist = true;
    private void FluentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        IsExist = false;
    }
}

public class Cookie
{
    public static string bzb_jsxsd = "6E84C4FBD2A95A85921608E080F93811";
    public static string bzb_njw = "87E901D210AC0072EF596E8939D58B2A";
}