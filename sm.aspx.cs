using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShackToGo.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Text;
using System.Net;

public partial class sm : System.Web.UI.Page
{
    public int _timezoneOffset = 0;
    public int _storyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        using (ShackToGoDataContext dc = new ShackToGoDataContext())
        {
            if (string.IsNullOrEmpty(Request.QueryString["u"]) == true)
            {
                this.LiteralError.Text = "No username set, you shouldn't be here";
                return;
            }
            if (string.IsNullOrEmpty(Request.QueryString["i"]) == false && string.IsNullOrEmpty(Request.QueryString["c"]) == true) // we are bookmarking something
            {
                try
                {
                    string url = String.Format("http://socksandthecity.net/shackmarks/shackmark.php?user={0}&id={1}", Server.UrlEncode(Request.QueryString["u"]), Request.QueryString["i"]);
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    myRequest.Method = "GET";

                    HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                    StreamReader read = new StreamReader(response.GetResponseStream());
                    string result = read.ReadToEnd();
                    response.Close();
                    //Response.Write(result);


                }
                catch (Exception)
                {
                    this.LiteralError.Text = "Bookmark Add failed. There was a problem contacting ShackMarks, try again later.";
                }


            }
            if (string.IsNullOrEmpty(Request.QueryString["c"]) == false && Request.QueryString["c"] == "d") // deleteing
            {
                try
                {
                    string url = String.Format("http://socksandthecity.net/shackmarks/unshackmark.php?user={0}&id={1}", Server.UrlEncode(Request.QueryString["u"]), Request.QueryString["i"]);
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    myRequest.Method = "GET";

                    HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                    StreamReader read = new StreamReader(response.GetResponseStream());
                    string result = read.ReadToEnd();
                    response.Close();
                    //Response.Write(result);

                }
                catch (Exception)
                {
                    this.LiteralError.Text = "Bookmark Remove failed. There was a problem contacting ShackMarks, try again later.";
                }

            }


            String test = dc.Connection.ConnectionString;
            ShackUser su = dc.ShackUsers.Where(u => u.UserName == Request.QueryString["u"]).SingleOrDefault();
            if (su != null)
                _timezoneOffset = su.TimeAdjustment;
            else
                _timezoneOffset = 0;

            try
            {

                string sURL = "http://socksandthecity.net/shackmarks/xml.php?user=" + Server.UrlEncode(Request.QueryString["u"]);

                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
                XmlDocument doc = new XmlDocument();
                doc.Load(sURL); // load the document with the writer
                doc.WriteTo(writer);

                if (doc.SelectNodes("comments").Count > 0)
                {

                    XmlNodeList nodes = doc.SelectNodes("comments/comment");

                    RepeaterBookMarks.DataSource = doc.SelectNodes("comments/comment");
                    RepeaterBookMarks.DataBind();
                }
                else
                    this.LiteralError.Text = "You have no bookmarks";



                stream = null;
                writer = null;
                doc = null;
            }
            catch (Exception ex)
            {
                //TODO: Clean this up
                if (ex.Message == "Root element is missing.")
                    this.LiteralError.Text = "You have no bookmarks.";
                else
                    this.LiteralError.Text = "There was a problem contacting ShackMarks, try again later.";
            }

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
