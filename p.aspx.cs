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
using System.Net;
using ShackToGo.Helper;

public partial class p : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string threadid = Request.QueryString["i"];
        string storyid = Request.QueryString["s"];

        this.LiteralChattyURL.Text = "";
        this.LiteralChattyURL.Visible = false;

        this.LiteralThreadURL.Text = "";
        this.LiteralThreadURL.Visible = false;



        if (Request.Cookies["savepass"] != null && Request.Cookies["savepass"].Value.Length > 0)
        {
            TextBoxPassword.Attributes.Add("value", Request.Cookies["savepass"].Value.ToString());
            CheckBoxRemember.Checked = true;
        }


        if (Request.QueryString["u"] != null && Request.QueryString["u"].Length > 0)
            this.TextBoxLogin.Text = Request.QueryString["u"].ToString();

    }
    protected void ButtonSubmit_Click(object sender, EventArgs e)
    {
        
  

        if (CheckBoxRemember.Checked == true)
            Response.Cookies["savepass"].Value = TextBoxPassword.Text;
        else
            Response.Cookies["savepass"].Value = null;

        string threadid = Request.QueryString["i"];
        string storyid = Request.QueryString["s"];
        string topThreadId = Request.QueryString["t"]; // the root post for when we go back to the thread view

        WebRequest wr;
        string postURL = "";

        if (threadid != null && storyid != null)
        { // reply to thread
            //postURL = String.Format("http://shackchatty.com/create/{0}/{1}.xml", storyid, threadid);
            postURL = String.Format("http://www.shacknews.com/extras/post_laryn_iphone.x");

            this.LiteralChattyURL.Text = string.Format("<a href=\"{0}{1}\">Back to Chatty</a>", ResolveUrl("~"), Helper.AppendUserName("?"));
            this.LiteralThreadURL.Text = string.Format("<a href=\"t.aspx?i={0}&s={1}{2}\">Back to Thread</a>", topThreadId, storyid, Helper.AppendUserName("&"));
            wr = WebRequest.Create(postURL);
        }
        else if (storyid != null)
        {
            //postURL = String.Format("http://shackchatty.com/create/{0}.xml", storyid);
            postURL = String.Format("http://www.shacknews.com/extras/post_laryn_iphone.x");
            this.LiteralChattyURL.Text = string.Format("<a href=\"{0}{1}\">Back to Chatty</a>", ResolveUrl("~"), Helper.AppendUserName("?"));

            wr = WebRequest.Create(postURL);
        }
        else
        {
            LiteralPostResult.Text = "There is a problem posting to this thread.";
            this.PanelPostForm.Visible = false;
            return;
        }


        wr.Method = "POST";
        wr.ContentType = "application/x-www-form-urlencoded";

        //string request = string.Format("username={0}&password={1}&body={2}", this.TextBoxLogin.Text, this.TextBoxPassword.Text, this.TextBoxPost.Text);
        string request = string.Format("iuser={0}&ipass={1}&group={2}&parent={3}&body={4}", Server.UrlEncode(this.TextBoxLogin.Text), Server.UrlEncode( this.TextBoxPassword.Text), storyid, threadid, Server.UrlEncode(this.TextBoxPost.Text));

        Byte[] PostBytes = System.Text.Encoding.UTF8.GetBytes(request);
        wr.ContentLength = PostBytes.Length;

        System.IO.Stream stream = wr.GetRequestStream();
        stream.Write(PostBytes, 0, PostBytes.Length);
        stream.Close();

        PanelPostForm.Visible = false;
        this.LiteralChattyURL.Visible = true;
        this.LiteralThreadURL.Visible = true;

        try
        {
            WebResponse resp = wr.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

            //LiteralPostResult.Text = sr.ReadToEnd().ToString().Trim();
            string result = sr.ReadToEnd().ToString().Trim();

            // Shack sends back a <script> with a bunch of script.. so.. we look for error messages.. eh...
            if (result.Contains("You must be logged in to post") == true)
            {
                this.LiteralPostResult.Text = "Login failed, please check your username and password.<br/><br/>";
                this.PanelPostForm.Visible = true;
            }
            else if (result.Contains("Please post something with more than 5 characters.") == true)
            {
                this.LiteralPostResult.Text = "Please post something with more than 5 characters.<br/><br/>";
                this.PanelPostForm.Visible = true;
            }
            else if (result.Contains("Please wait a few minutes before trying to post again.") == true)
            {
                this.LiteralPostResult.Text = "Please wait a few minutes before trying to post again.<br/><br/>";
                this.PanelPostForm.Visible = true;
            }
            else
            {
                this.LiteralPostResult.Text = "Post Successful.<br><br>";
                this.PanelPostForm.Visible = false;
            }

        }
        catch (Exception)
        {
            LiteralPostResult.Text = "Post Failed, check your login and password.";
        }


    }
    protected void ButtonTag_Command(object sender, CommandEventArgs e)
    {
        string tag = e.CommandArgument.ToString();
        string append = "";

        switch (this.DropDownShackTags.SelectedValue)
        {
            case "red":
                append = "r{" + this.TextBoxCode.Text + "}r";
                break;
            case "green":
                append = "g{" + this.TextBoxCode.Text + "}g";
                break;
            case "blue":
                append = "b{" + this.TextBoxCode.Text + "}b";
                break;
            case "yellow":
                append = "y{" + this.TextBoxCode.Text + "}y";
                break;
            case "lime":
                append = "l{" + this.TextBoxCode.Text + "}l";
                break;
            case "orange":
                append = "n{" + this.TextBoxCode.Text + "}n";
                break;
            case "multi":
                append = "p{" + this.TextBoxCode.Text + "}p";
                break;
            case "olive":
                append = "e{" + this.TextBoxCode.Text + "}e";
                break;
            case "italic":
                append = "/[" + this.TextBoxCode.Text + "]/";
                break;
            case "bold":
                append = "b[" + this.TextBoxCode.Text + "]b";
                break;
            case "quote":
                append = "q[" + this.TextBoxCode.Text + "]q";
                break;
            case "sample":
                append = "s[" + this.TextBoxCode.Text + "]s";
                break;
            case "under":
                append = "_[" + this.TextBoxCode.Text + "]_";
                break;
            case "strike":
                append = "-[" + this.TextBoxCode.Text + "]-";
                break;
            case "spoiler":
                append = "o[" + this.TextBoxCode.Text + "]o";
                break;
            default:
                break;
        }

        this.TextBoxPost.Text = this.TextBoxPost.Text + append + " ";
        this.TextBoxCode.Text = "";
    }
}
