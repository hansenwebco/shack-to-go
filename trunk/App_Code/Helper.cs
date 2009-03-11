﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


/// <summary>
/// Summary description for Helper
/// </summary>
/// 
namespace ShackToGo.Helper
{
  public class Helper
  {

    public static string AppendUserName(string delimiter)
    {
      if (HttpContext.Current.Request.QueryString["u"] != null && HttpContext.Current.Request.QueryString["u"].Length > 0)
        return delimiter + "u=" + HttpContext.Current.Request.QueryString["u"];
      else
        return "";
    }
    public static string RenderThomW(string id, string storyID)
    {
      if (HttpContext.Current.Request.QueryString["u"] != null && HttpContext.Current.Request.QueryString["u"].Length > 0)
        return string.Format("<span class=\"l\"><a href=\"h.aspx?f=LOL&i={0}&s={1}{2}\">lol</a></span> <span class=\"i\"><a href=\"h.aspx?f=INF&i={3}&s={4}{5}\">inf</a></span>", id, storyID, AppendUserName("&"), id, storyID, AppendUserName("&"));
      else
        return "";
    }
    public static string MarkThread(string id, string storyid)
    {
      if (AppendUserName("").Length > 0)
        return String.Format("<a href=\"sm.aspx?i={0}&s={1}{2}\">mark</a> ", id, storyid, AppendUserName("&"));
      else
        return "";
    }
    public static string DrawCategory(string category)
    {
      switch (category)
      {
        case "nws":
          //return "<span class=\"jt_red\"><b>[nws] </b></span>";
          return "<img src=\"n.gif\">";
        case "stupid":
          //return "<span class=\"jt_green\"><b>[stupid] </b></span>";
          return "<img src=\"s.gif\">";
        case "political":
          //return "<span class=\"jt_orange\"><b>[political] </b></span>";
          return "<img src=\"p.gif\">";
        case "informative":
          //return "<span class=\"jt_blue\"><b>[interesting] </b></span>";
          return "<img src=\"i.gif\">";
        case "offtopic":
          //return "<span><b>[offtopic] </b></span>";
          return "<img src=\"o.gif\">";
        default:
          return "";
      }

    }
    public static string AdjustforTimeZone(string shackDate,int userOffset)
    {
      DateTime nodedate;
      DateTime.TryParseExact(shackDate.ToString().Substring(0, shackDate.ToString().Length - 4) , "MMM dd, yyyy h:mmtt", null, System.Globalization.DateTimeStyles.None , out nodedate);

      //return shackDate + " " + nodedate.AddHours(userOffset).ToString();
      return nodedate.AddHours(userOffset).ToString("M.d.yy h:mmt");
   
    }
    public static string GetPostNameCSSClass(string name)
    {

      if (HttpContext.Current.Request.QueryString["u"] != null && HttpContext.Current.Request.QueryString["u"].Length > 0)
        if (HttpContext.Current.Request.QueryString["u"].ToLower() ==name.ToLower())
          return "ah";


      return "a";

    }

  }

}
