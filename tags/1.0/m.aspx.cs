using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using ShackToGo.Helper;
using System.Text.RegularExpressions;

public partial class m : System.Web.UI.Page
{
  public int _timezoneOffset = 0;
  protected void Page_Load(object sender, EventArgs e)
  {


    using (ShackToGoDataContext dc = new ShackToGoDataContext())
    {
      String test = dc.Connection.ConnectionString;
      _timezoneOffset = dc.ShackUsers.Where(u => u.UserName == Request.QueryString["u"]).SingleOrDefault().TimeAdjustment;

      // check for a delete 
      if (Request.QueryString["c"] != null && Request.QueryString["c"] == "d")
      {

        // delete book mark
        int UserID = dc.ShackUsers.Where(u => u.UserName == Request.QueryString["u"]).SingleOrDefault().UserID;


        BookMark bm = dc.BookMarks.Where(b => b.StoryID == Request.QueryString["s"]
                                         && b.ThreadID == Request.QueryString["i"]
                                         && b.UserID == UserID).SingleOrDefault();
        if (bm != null)
        {
          bm.Deleted = true;
          dc.SubmitChanges();
        }

      }

      // if we have a i, s, and u parameter we add it to the database
      else if (Request.QueryString["i"] != null && Request.QueryString["u"] != null
          && Request.QueryString["s"] != null)
      {

        try
        {

          // get the post that they are saving
          string sUrl = ConfigurationManager.AppSettings["siteAPIURL"] + "thread/" + Request.QueryString["i"] + ".xml";

          MemoryStream stream = new MemoryStream();
          XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
          XmlDocument doc = new XmlDocument();
          doc.Load(sUrl); // load the document with the writer
          doc.WriteTo(writer);

          XmlNode node = doc.SelectSingleNode("comments/comment/body");
          string body = node.InnerText.ToString();
          node = doc.SelectSingleNode("comments/comment");
          string author = node.Attributes["author"].Value;
          string postdate = node.Attributes["date"].Value;
          string replyCount = node.Attributes["reply_count"].Value;

          stream = null;
          writer = null;
          doc = null;


          // first get the user account
          int UserID = dc.ShackUsers.Where(u => u.UserName == Request.QueryString["u"]).SingleOrDefault().UserID;

          if (UserID > 0) // add book mark
          {
            BookMark bm; // check to make sure we don't already hve this one
            bm = dc.BookMarks.Where(b => b.StoryID == Request.QueryString["s"] && b.ThreadID == Request.QueryString["i"]).FirstOrDefault();
            if (bm == null)
            {

              bm = new BookMark();
              bm.UserID = UserID;
              bm.ThreadID = Request.QueryString["i"];
              bm.StoryID = Request.QueryString["s"];
              bm.Desc = body.Trim();
              bm.PosterName = author;
              bm.PostCreated = postdate;
              bm.ReplyCount = Convert.ToInt32(replyCount);
              bm.DateCreated = DateTime.Now.ToUniversalTime();
              dc.BookMarks.InsertOnSubmit(bm);
              dc.SubmitChanges();
            }
          }



        }
        catch (Exception ex)
        {
          Response.Write("OH NOES! Squeegy's Server isn't responding...or I blew something up...<br/>");
          Response.Write("ERROR: " + ex.Message);
        }

      }
    }
    BindData();
  }
  protected void BindData()
  {
    using (ShackToGoDataContext dc = new ShackToGoDataContext())
    {
      // bind the list of bookmarks
      var result = from b in dc.BookMarks
                   join u in dc.ShackUsers on b.UserID equals u.UserID
                   where u.UserName == Request.QueryString["u"]
                   && b.Deleted == false
                   orderby b.DateCreated descending
                   select new { b, u };

      this.RepeaterBookMarks.DataSource = result;
      this.RepeaterBookMarks.DataBind();

      if (this.RepeaterBookMarks.Items.Count > 0)
        this.LiteralError.Text = "";
      else
        this.LiteralError.Text = "<br/>Nothing bookmarked.";

    }
  }
  public string FormatBookMark(string bookmarkText)
  {

    // clean up URL's in the bookmark view, we don't want partial links
    Regex regex = new Regex(@"<a[\s]+[^>]*?href[\s]?=[\s\""\']+(.*?)[\""\']+.*?>([^<]+|.*?)?<\/a>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
    bookmarkText = regex.Replace(bookmarkText, "$1");


    if (bookmarkText.Length > 300)
      return bookmarkText.Substring(0, 300) + "...";
    else
      return bookmarkText;

  }
}
