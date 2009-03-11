using System;
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

public partial class _Default : System.Web.UI.Page
{

    public int _storyID = 0;
    public int _threadUpdateCount = 0;
    public int _pages = 0;
    public int _currentPage = 1;
    public int _timezoneOffset = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string user = Request.QueryString["u"];

        // Get the user and then set the last database pull session variables
        if (user != null && user.Length > 0)
        {
            Session["user"] = user;

            // check the database for this user
            using (ShackToGoDataContext dc = new ShackToGoDataContext())
            {
                ShackUser su = dc.ShackUsers.Where(u => u.UserName == user).SingleOrDefault();
                if (su == null)
                {
                    su = new ShackUser();
                    su.UserName = user;
                    su.LastDbPull = DateTime.Now.ToUniversalTime();
                    dc.ShackUsers.InsertOnSubmit(su);
                    dc.SubmitChanges();
                    Session["lastpull"] = DateTime.Now.ToUniversalTime();
                }
                else
                {
                    Session["lastpull"] = su.LastDbPull;
                    su.LastDbPull = DateTime.Now.ToUniversalTime();
                    dc.SubmitChanges();
                    _timezoneOffset = su.TimeAdjustment;
                }

            }
        }

        try // connect to API and get the latest chatty and requested page
        {
            string sUrl = "";

            if (Request.QueryString["p"] != null & Request.QueryString["s"] != null) // p is the page parameter
                sUrl = ConfigurationManager.AppSettings["siteAPIURL"] + Request.QueryString["s"] + "." + Request.QueryString["p"] + ".xml";
            else
                sUrl = ConfigurationManager.AppSettings["siteAPIURL"];

            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            XmlDocument doc = new XmlDocument();
            doc.Load(sUrl); // load the document with the writer
            doc.WriteTo(writer);

            XmlNode node = doc.SelectSingleNode("comments");
            _storyID = Convert.ToInt32(node.Attributes["story_id"].Value);

            if (node.Attributes["last_page"].Value.Length > 0)
                _pages = Convert.ToInt32(node.Attributes["last_page"].Value);

            this.LiteralStoryTitle.Text = node.Attributes["story_name"].Value.ToString();

            this.LiteralPages.Text = DrawPages(_pages);

            RepeaterPosts.DataSource = doc.SelectNodes("comments/comment");
            RepeaterPosts.DataBind();

            stream = null;
            writer = null;
            doc = null;
        }
        catch (Exception ex)
        {
            Response.Write("OH NOES! Squeegy's Server isn't responding...or I blew something up...<br/>");
            Response.Write("ERROR: " + ex.Message);
        }
    }
    /// <summary>
    /// Create links to reply to either the main chatty or to one of the posts displayed
    /// </summary>
    /// <param name="replyCount"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string DoReplyLink(int replyCount, int id)
    {
        if (replyCount > 0)
        {
            if (Request.QueryString["u"] != null)
                return string.Format("<a href=\"t.aspx?i={0}&s={1}{2}\">{3} replies</a>", id, _storyID, Helper.AppendUserName("&"), replyCount);
            else
                return string.Format("<a href=\"t.aspx?i={0}&s={1}\">{2} replies</a>", id, _storyID, replyCount);
        }
        else
            return "No Replies";
    }
    /// <summary>
    /// Determines is the post being bound to is newer then the last time the user retreived a main chatty
    /// </summary>
    /// <param name="postDate"></param>
    /// <returns></returns>
    public string CheckNewPost(object postDate)
    {
        if (Session["lastpull"] != null)
        {
            DateTime nodedate;
            DateTime.TryParseExact(postDate.ToString().Substring(0, postDate.ToString().Length - 4) + " -0500", "MMM dd, yyyy h:mmtt zzz", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out nodedate);

            if (DateTime.Compare(nodedate, DateTime.Parse(Session["lastpull"].ToString())) > 0)
            {
                _threadUpdateCount++;
                //return "<span class=\"n\">- New!</span>";
                return "<img src=\"w.gif\">&nbsp;";
            }
            else
                return "";
        }
        return "";
    }
    /// <summary>
    /// Draws the paging links along the bottom of the chatty listing
    /// </summary>
    /// <param name="pages"></param>
    /// <returns></returns>
    protected string DrawPages(int pages)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < pages; i++)
        {
            if ((Request.QueryString["p"] == null && i == 1) || Request.QueryString["p"] == i.ToString())
                sb.Append(i.ToString() + "&nbsp;");
            else
                sb.Append(string.Format("<a href=\"{0}?p={1}&s={2}{3}\">{4}</a>", Page.ResolveUrl("~"), i.ToString(), _storyID, Helper.AppendUserName("&"), i) + "&nbsp;");
        }
        return sb.ToString();
    }
}
