﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Page
/// </summary>
public class ShackToGoPage : System.Web.UI.Page
{
    private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
    private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

    protected override void Render(HtmlTextWriter writer)
    {
        using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
        {
            base.Render(htmlwriter);
            string html = htmlwriter.InnerWriter.ToString();

            html = REGEX_BETWEEN_TAGS.Replace(html, "> <");
            html = REGEX_LINE_BREAKS.Replace(html, string.Empty);

            writer.Write(html.Trim());
        }
    }
}
