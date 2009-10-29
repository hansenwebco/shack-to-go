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
using System.Xml;
using System.IO;
using ShackToGo.Helper;


public partial class h : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // handle ThomW LOL page
        if (Request.QueryString["f"] == "LOL" || Request.QueryString["f"] == "INF" || Request.QueryString["f"] == "UNF" || Request.QueryString["f"] == "TAG")
        {
            try
            {
                string person = Request.QueryString["u"];
                string threadid = Request.QueryString["i"];
                string tag = Request.QueryString["f"];

                if (person.ToLower().Contains("yourusername"))
                {
                    this.LiteralResult.Text = "You can't LOL/INF stuff without setting your actual username dummy";
                    return;
                }

                string sURL = String.Format("http://lmnopc.com/greasemonkey/shacklol/report.php?who={0}&what={1}&tag={2}&version=-1", person, threadid, tag);

                // try and load ThomW's page!
                HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(sURL);
                http.Method = "GET";

                HttpWebResponse wr = (HttpWebResponse)http.GetResponse();

                StreamReader sr = new StreamReader(wr.GetResponseStream());

                string result = sr.ReadToEnd();

                if (result != null && result.Length > 2)
                    if (result.Substring(0, 2) == "ok")
                    {
                        if (Request.QueryString["f"] == "LOL")
                            this.LiteralResult.Text = "Post [ <span class=\"l\">LOL'd</span> ]";
                        else if (Request.QueryString["f"] == "UNF")
                            this.LiteralResult.Text = "Post [ <span class=\"u\">UNF'd</span> ]";
                        else if (Request.QueryString["f"] == "TAG")
                            this.LiteralResult.Text = "Post [ <span class=\"t\">TAG'd</span> ]";
                        else
                            this.LiteralResult.Text = "Post [ <span class=\"i\">INF'd</span> ]";
                    }
                    else
                        this.LiteralResult.Text = result;
                else
                    this.LiteralResult.Text = "Error LOLing or INFing or TAGing or UNFing - Internets Boo!";
            }
            catch (Exception)
            {
                this.LiteralResult.Text = "Error LOLing or INFing or TAGing or UNFing  - Internets Boo!";
            }


            this.LiteralBackToChatty.Text = string.Format("<a href=\"{0}\">back to chatty</a>", Page.ResolveUrl("~") + Helper.AppendUserName("?"));
            this.LiteralBackToThread.Text = string.Format("<a href=\"t.aspx?i={0}&s={1}{2}\">back to thread</a>", Request.QueryString["i"], Request.QueryString["s"], Helper.AppendUserName("&"));
        }

    }
}
