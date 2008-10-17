using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;

public partial class t : System.Web.UI.Page
{
  public int _threadUpdateCount = 0;
  public List<int> threading = new List<int>();
  public bool _threadedView = false;
  public bool _threadedTextView = false;
  public int _timezoneOffset = 0;
  public int _currentIndent = 0;

  protected void Page_Load(object sender, EventArgs e)
  {

    if (Request.QueryString["u"] != null && Request.QueryString["u"].ToString().Length > 0)
    {
      string user = Request.QueryString["u"].ToString();

      // check the database for this user
      using (ShackToGoDataContext dc = new ShackToGoDataContext())
      {
        ShackUser su = dc.ShackUsers.Where(u => u.UserName == user).SingleOrDefault();
        if (su == null)
        {
          su = new ShackUser();
          su.UserName = user;
          dc.ShackUsers.InsertOnSubmit(su);
          dc.SubmitChanges();
          Session["lastpull"] = DateTime.Now.ToUniversalTime();
        }
        else
        {
          Session["lastpull"] = su.LastDbPull;
          _threadedView = su.EnableThreadedView;
          _threadedTextView = su.EnableThreadTextDisplay;
          _timezoneOffset = su.TimeAdjustment;
          dc.SubmitChanges();
        }
      }
    }
    try // connect to API and get thread
    {
      string threadid = Request.QueryString["i"];

      string sUrl = ConfigurationManager.AppSettings["siteAPIURL"] + "thread/" + threadid + ".xml";

      MemoryStream stream = new MemoryStream();
      XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
      XmlDocument doc = new XmlDocument();
      doc.Load(sUrl); // load the document with the writer
      doc.WriteTo(writer);

      XmlNode node = doc.SelectSingleNode("comments");

      RepeaterPosts.DataSource = doc.GetElementsByTagName("comment");
      RepeaterPosts.DataBind();

      stream = null;
      writer = null;
      doc = null;
    }
    catch (Exception ex)
    {
      Response.Write("OH NOES! Squeegy's Server isn't responding...or I blew something up...<br/><br/>");
      Response.Write("ERROR: " + ex.Message);
    }
  }

  public string CheckNewPost(object postDate)
  {
    if (Session["lastpull"] != null)
    {

      DateTime nodedate;
      DateTime.TryParseExact(postDate.ToString().Substring(0, postDate.ToString().Length - 4) + " -0500", "MMM dd, yyyy h:mmtt zzz", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out nodedate);


      if (DateTime.Compare(nodedate, DateTime.Parse(Session["lastpull"].ToString())) > 0)
      {
        _threadUpdateCount++;
        return "<img src=\"w.gif\">&nbsp;";
      }
      else
        return "";
    }
    return "";
  }
  /// <summary>
  /// Returns the indention of the current thread based on the number of replies.
  /// </summary>
  /// <param name="replies"></param>
  /// <returns></returns>
  public string SetIndent(int replies)
  {
    string result = "";

    // First time through we return 0 indent, after that we a Generic list to keep track of the current
    // indention of the thread
    if (threading.Count > 0)
    {
      result = (threading.Count * 10).ToString() + "px";
      _currentIndent = (threading.Count);
    }
    else
    {
      result = "0px";
      _currentIndent = 0;
    }

    // reduce our collection of indents by 1 for each, and then remove the zeros
    for (int i = 0; i < threading.Count; i++)
    {
      threading[i]--;
      if (threading[i] == 0)
      {
        threading.RemoveAt(i);
        i--;
      }
    }

    // if we have replies add them to our collection
    if (replies > 0)
      threading.Add(replies);

    return result;
  }
  /// <summary>
  /// Renders the textual indent for people who can't see the regular threading
  /// </summary>
  /// <returns></returns>
  public string DrawTextIndent()
  {
    return "".PadLeft(_currentIndent, '*') + " ";
  }

}
