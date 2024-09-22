
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using UiDesktopApp1.ViewData;
using Wpf.Ui.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Data.Common;

namespace UiDesktopApp1.UI;

/// <summary>
/// Page1.xaml 的交互逻辑
/// </summary>
public partial class Page1 : Page
{
    public ObservableCollection<GridTabData> gridTabDatas { get; set; }=new();

    public Page1()
    {
        InitializeComponent();
    }
    int ri= 23;
        //datagrid.ItemsSource = gridTabDatas;
    public void SetJson(string json)
    {
        //09 - 16
        //Debug.WriteLine(json);
        if (json == "") return;
        Monday_column.Header = $"星期一({ri++})";
        Tuesday_column.Header = $"星期二({ri++})";
        Wednesday_column.Header = $"星期三({ri++})";
        Thursday_column.Header = $"星期四({ri++})";
        Friday_column.Header = $"星期五({ri++})";
        Saturday_column.Header = $"星期六({ri++})";
        Sunday_column.Header = $"星期日({ri})";
        //Monday_column.ItemsSource = gridTabDatas;
        new Thread(() =>
        {
            while (MainWindow.IsExist)
            {
                //获取当前时间
                var now = DateTime.Now;
                string formattedTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                Dispatcher.Invoke(() =>
                {
                    itemtext.Text = $"{formattedTime}";
                });
                Thread.Sleep(1000);
            }
        }).Start();
        var schedules = JsonConvert.DeserializeObject<List<KcTabData>>(json);
        if (schedules == null) return;
        datagrid.ItemsSource = schedules;
    }

    private double _scale = 1.0;
    private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        //Debug.WriteLine(e.Delta);
        // 检测是否按住 Ctrl 键
        if (Keyboard.Modifiers == ModifierKeys.Shift)
        {
            // 控制横向滚动
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true; // 标记事件已处理
        }else if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            // 根据鼠标滚轮的方向调整缩放比例
            if (e.Delta > 0)
            {
                _scale += 0.1; // 放大
            }
            else if (e.Delta < 0)
            {
                _scale = Math.Max(0.1, _scale - 0.1); // 缩小，确保不小于0.1
            }
            datagrid.LayoutTransform = new ScaleTransform(_scale, _scale);

            e.Handled = true;

        }
    }

    private void Page_KeyDown(object sender, KeyEventArgs e)
    {
        jianpan.Text = $"按下 {e.Key}";
    }

    private void Page_KeyUp(object sender, KeyEventArgs e)
    {
        jianpan.Text =string.Empty;
    }

    private void datagrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        var dataGrid = sender as Wpf.Ui.Controls.DataGrid;
        var hit = e.OriginalSource as FrameworkElement;

        // 查找单元格
        while (hit != null && hit is not DataGridCell)
        {
            hit = (FrameworkElement)VisualTreeHelper.GetParent(hit);
        }

        if (hit is DataGridCell cell)
        {
            // 获取行数据上下文
            var rowData = cell.DataContext;
            if (rowData is KcTabData.TabData a)
            {
                System.Windows.MessageBox.Show($"{a.Section}");
                return;
            }else if(rowData is KcTabData aa)
            {
                
                var cellValue = (cell.Content as System.Windows.Controls.TextBlock)?.Text;

                var column = cell.Column as DataGridTextColumn;
                var header = column?.Header.ToString();
                if (header != null)
                {
                    setSwitch(header, aa);
                }


            }
            else
            {
                Debug.WriteLine(rowData);
            }

            // 获取单元格内容

            // 处理点击事件
            //System.Windows.MessageBox.Show($"你点击了单元格，行数据: {rowData}, 单元格内容: {cellValue}");
        }
    }

    private void setSwitch(string header ,KcTabData aa)
    {
        if (header.ToString().StartsWith("星期一"))
        {
            if (aa.date.Monday != null)
            {
                System.Windows.MessageBox.Show($"{aa.Tab.Section}  {aa.date.Monday.CourseName}");
            }
        }
        else if (header.ToString().StartsWith("星期二"))
        {
            if (aa.date.Tuesday != null)
            {
                System.Windows.MessageBox.Show($"{aa.Tab.Section} {aa.date.Tuesday.CourseName}");
            }

        }
        else if (header.ToString().StartsWith("星期三"))
        {
            if (aa.date.Wednesday != null)
            {
                System.Windows.MessageBox.Show($"{aa.Tab.Section}   {aa.date.Wednesday.CourseName}");
            }

        }
        else if (header.ToString().StartsWith("星期四"))
        {
            if (aa.date.Thursday != null)
            {
                System.Windows.MessageBox.Show($"{aa.Tab.Section} {aa.date.Thursday.CourseName}");
            }

        }
        else if (header.ToString().StartsWith("星期五"))
        {
            if (aa.date.Friday != null)
            {

                System.Windows.MessageBox.Show($"{aa.Tab.Section} {aa.date.Friday.CourseName}");
            }

        }
        else if (header.ToString().StartsWith("星期六"))
        {
            if (aa.date.Saturday != null)
            {
                System.Windows.MessageBox.Show($"{aa.Tab.Section}  {aa.date.Saturday.CourseName}");
            }

        }
        else if (header.ToString().StartsWith("星期日"))
        {
            if (aa.date.Sunday != null)
            {
                System.Windows.MessageBox.Show($"{aa.Tab.Section}   {aa.date.Sunday.CourseName}");
            }
        }

    }
}
