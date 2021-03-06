﻿using DataScience.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataScience
{
    public class HtmlReportBase
    {
        public readonly StringBuilder body = new StringBuilder();

        public void TD(object v)
        {
            Write($"<td>{v?.ToString() ?? ""}</td>");
        }

        public void AddChart(Chart chart, Size size)
        {
            chart.Size = size;
            chart.SaveImage("temp.png", ChartImageFormat.Png);
            var bytes = File.ReadAllBytes("temp.png");
            AddImage(bytes, "png");
        }

        public void AddTable<TRow, TColumn, TValue>(Table<TRow, TColumn, TValue> table, Func<TRow, string> rowWriter, Func<TColumn, string> columnWriter, Func<TValue, string> valueWriter)
        {
            body.AppendLine("<table><tr><th></th>");
            foreach (var e in table.Columns)
                body.AppendLine($"<th>{columnWriter(e)}</th>");
            body.AppendLine("</tr>");
            foreach (var e in table.Rows)
            {
                body.AppendLine($"<tr><th>{rowWriter(e)}</th>");
                foreach (var c in table.Columns)
                    body.AppendLine($"<td>{valueWriter(table.GetValue(e, c))}</td>");
                body.AppendLine("</tr>");
            }
            body.AppendLine("</table>");

        }

        public void Par(string s)
        {
            body.AppendLine("<p>" + s + "</p>");
        }

        public void H1(string s)
        {
            body.AppendLine("<h1>" + s + "</h1>");
        }

        public void H2(string s)
        {
            body.AppendLine("<h2>" + s + "</h2>");
        }

        public void Write(string v)
        {
            body.AppendLine(v);
        }

        public void H3(string s)
        {
            body.AppendLine("<h3>" + s + "</h3>");
        }

        public void AddImage(byte[] image, string type)
        {
            body.AppendLine($"<img src=\"data:image/{type};base64,{Convert.ToBase64String(image)}\"/>");
        }

        public string AnchorHere()
        {
            var guid = Guid.NewGuid();
            body.AppendLine($"<div id=\"" + guid + "\"></div>");
            return guid.ToString();
        }

        public void Link(string id, string text)
        {
            body.AppendLine("<a href=#" + id + ">" + text + "</a>");
        }
    }



    public class HtmlReport : HtmlReportBase, IDisposable
    {
        public readonly string filename;
        public readonly bool skipStyles;
        public readonly bool dontOpen;
        public Dictionary<string, HtmlReportBase> Sections;
        public List<string> SectionsOrder;


        public HtmlReport(string filename, bool skipStyles= false, string[] sections=null, bool dontOpen=false)
        {
            this.filename = filename;
            this.skipStyles = skipStyles;
            this.dontOpen = dontOpen;
            if (sections == null)
                sections = new[] { "main" };
            SectionsOrder = sections.ToList();
            if (!SectionsOrder.Contains("main"))
                SectionsOrder.Add("main");
            Sections = SectionsOrder.ToDictionary(z => z, z => z == "main" ? this : new HtmlReportBase());
        }

        public void Dispose()
        {
            var writer = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), Encoding.UTF8);

            writer.WriteLine(@"<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>");

            if (!skipStyles)
            writer.WriteLine(@"
<style type=""text/css"">
h1 {
  font-size: 34px;
}
h1.title {
  font-size: 38px;
}
h2 {
  font-size: 30px;
}
h3 {
  font-size: 24px;
}
h4 {
  font-size: 18px;
}
h5 {
  font-size: 16px;
}
h6 {
  font-size: 12px;
}
.table th:not([align]) {
  text-align: left;
}
table {
border-collapse: collapse;
border-spacing: 0;
}
td, th { border: 1px solid #CCC; }

th {
background: #DFDFDF; /* Darken header a bit */
font-weight: bold;
}

td {
background: #FAFAFA;
text-align: center;
}

/* Cells in even rows (2,4,6...) are one color */
tr:nth-child(even) td { background: #F1F1F1; }

/* Cells in odd rows (1,3,5...) are another (excludes header cells) */
tr:nth-child(odd) td { background: #FEFEFE; }
</style>
</head>
<body>");
            foreach (var e in SectionsOrder)
                writer.WriteLine(Sections[e].body);
            writer.WriteLine("</body></html>");
            writer.Close();
            if (!dontOpen)
                Process.Start(filename);
        }





    }
}
