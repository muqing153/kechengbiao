using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp1.ViewData;

public class KcTabData
{
    public TabData Tab { get; set; }
    public Date date { get; set; }

    public class TabData
    {
        public string Section { get; set; }
        public string SubSection { get; set; }
        public string Time { get; set; }
    }


    public class Course
    {
        public string CourseName { get; set; }
        public string CreditsAndPeriod { get; set; }
        public string Classroom { get; set; }
        public string Time { get; set; }
        public string ClassName { get; set; }
    }


    public class Date
    {
        [JsonProperty("星期一")]
        public Course Monday { get; set; }

        [JsonProperty("星期二")]
        public Course Tuesday { get; set; }

        [JsonProperty("星期三")]
        public Course Wednesday { get; set; }

        [JsonProperty("星期四")]
        public Course Thursday { get; set; }

        [JsonProperty("星期五")]
        public Course Friday { get; set; }

        [JsonProperty("星期六")]
        public Course Saturday { get; set; }

        [JsonProperty("星期日")]
        public Course Sunday { get; set; }
    }

}
