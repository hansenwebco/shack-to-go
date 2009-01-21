using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Text;
using ShackToGo.Helper;

public partial class sr : System.Web.UI.Page
{
 
    protected void Page_Load(object sender, EventArgs e)
    {

      if (string.IsNullOrEmpty(Request.QueryString["s"]) == false || string.IsNullOrEmpty(Request.QueryString["a"]) == false 
         || string.IsNullOrEmpty(Request.QueryString["pa"] ) == false )
      {
        DoSearch();
      }
            

    }
    protected void ButtonSearch_Search(object sender, EventArgs e)
    {

      string currentPage = "1";
      if (string.IsNullOrEmpty(Request.QueryString["p"]) == false)
        currentPage = Request.QueryString["p"];
      
      if (this.TextBoxSearch.Text.Length > 0 || this.TextBoxAuthor.Text.Length > 0 || this.TextBoxParentAuthor.Text.Length > 0)
      {

        Response.Redirect(string.Format("sr.aspx?s={0}&a={1}&pa={2}&p={3}", this.TextBoxSearch.Text, this.TextBoxAuthor.Text, this.TextBoxParentAuthor.Text, currentPage));
      }
    }

    protected void DoSearch()
    {
      this.PanelSearchResults.Visible = true;
      PanelSearchForm.Visible = false;



      string searchText = Request.QueryString["s"];
      string author = Request.QueryString["a"];
      string parentAuthor = Request.QueryString["pa"];
      string page = Request.QueryString["p"];

      string sUrl = string.Format("http://shackapi.stonedonkey.com/Search/?SearchTerm={0}&author={1}&ParentAuthor={2}&page={3}", searchText, author, parentAuthor,page );

      MemoryStream stream = new MemoryStream();
      XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
      XmlDocument doc = new XmlDocument();
      doc.Load(sUrl); // load the document with the writer
      doc.WriteTo(writer);

      XmlNode node = doc.SelectSingleNode("results");
      this.LiteralResults.Text = node.Attributes["total_results"].Value + " results";

      int lastPage = int.Parse(node.Attributes["last_page"].Value);

      // this won't work since there can be thosands of resorts
      //this.LiteralPages.Text = DrawPages(lastPage);


      RepeaterPosts.DataSource = doc.GetElementsByTagName("result");
      RepeaterPosts.DataBind();

      stream = null;
      writer = null;
      doc = null;
    }

    protected string DoViewLink(string id, string storyID)
    {
     
        if (Request.QueryString["u"] != null)
          return string.Format("<a href=\"t.aspx?i={0}&s={1}{2}\">View</a>", id, storyID, Helper.AppendUserName("&"));
        else
          return string.Format("<a href=\"t.aspx?i={0}&s={1}\">View</a>", id, storyID);
   
    }
    protected string DrawPages(int pages)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 1; i < pages; i++)
      {
        if ((Request.QueryString["p"] == null && i == 1) || Request.QueryString["p"] == i.ToString())
          sb.Append(i.ToString() + "&nbsp;");
        else
          sb.Append(string.Format("<a href=\"sr.aspx?p={0}{1}\">{2}</a>",  i.ToString(), Helper.AppendUserName("&"), i) + "&nbsp;");
      }
      return sb.ToString();
    }


}
