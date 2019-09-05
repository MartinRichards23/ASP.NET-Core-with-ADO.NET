using System.Collections.Generic;

namespace WebsiteTemplate.ViewModels.GoogleCharts
{
    // https://developers.google.com/chart/interactive/docs/reference#dataparam

    public class Col
    {
        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string role { get; set; }
        public Properties p { get; set; }
    }

    public class Properties
    {
        public bool html { get; set; }
    }

    public class Cell
    {
        public object v { get; set; }
        public string f { get; set; }
    }

    public class Row
    {
        public List<Cell> c { get; set; } = new List<Cell>();
    }

    //public class P
    //{
    //    public string foo { get; set; }
    //    public string bar { get; set; }
    //}

    public class GoogleChart
    {
        public List<Col> cols { get; set; } = new List<Col>();
        public List<Row> rows { get; set; } = new List<Row>();
        //public P p { get; set; }
    }
}
