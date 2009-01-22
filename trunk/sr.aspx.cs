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
       || string.IsNullOrEmpty(Request.QueryString["pa"]) == false)
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

      Response.Redirect(string.Format("sr.aspx?s={0}&a={1}&pa={2}&p={3}{4}", this.TextBoxSearch.Text, this.TextBoxAuthor.Text, this.TextBoxParentAuthor.Text, currentPage, Helper.AppendUserName("&")));
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

    string sUrl = string.Format("http://shackapi.stonedonkey.com/Search/?SearchTerm={0}&author={1}&ParentAuthor={2}&page={3}", searchText, author, parentAuthor, page);

    MemoryStream stream = new MemoryStream();
    XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
    XmlDocument doc = new XmlDocument();
    doc.Load(sUrl); // load the document with the writer
    doc.WriteTo(writer);

    XmlNode node = doc.SelectSingleNode("results");
    int totalResults = 0;
    int.TryParse(node.Attributes["total_results"].Value,out totalResults);

    this.LiteralResults.Text = totalResults + "";

    int lastPage = int.Parse(node.Attributes["last_page"].Value);

    int currentPage = 1;
    int.TryParse(page, out currentPage);

    if (currentPage - 1 > 0)
      this.LiteralPagePrev.Text = string.Format("<a href=\"sr.aspx?s={0}&a={1}&pa={2}&p={3}{4}\">Prev</a>", searchText, author, parentAuthor, currentPage - 1, Helper.AppendUserName("&"));
    else
      this.LiteralPagePrev.Text = "";

    if (currentPage + 1 <= lastPage)
      this.LiteralPageNext.Text = string.Format("<a href=\"sr.aspx?s={0}&a={1}&pa={2}&p={3}{4}\">Next</a>", searchText, author, parentAuthor, currentPage + 1, Helper.AppendUserName("&"));
    else
      this.LiteralPageNext.Text = "";

    this.LiteralCurrentPage.Text = currentPage + " of " + lastPage;

    RepeaterPosts.DataSource = doc.GetElementsByTagName("result");
    RepeaterPosts.DataBind();

    stream = null;
    writer = null;
    doc = null;
  }
  /// <summary>
  /// Generates link that goes back to view the thread
  /// </summary>
  /// <param name="id"></param>
  /// <param name="storyID"></param>
  /// <returns></returns>
  protected string DoViewLink(string id, string storyID)
  {

    if (Request.QueryString["u"] != null)
      return string.Format("<a href=\"t.aspx?i={0}&s={1}{2}&j=t#{3}\">View</a>", id, storyID, Helper.AppendUserName("&"),id);
    else
      return string.Format("<a href=\"t.aspx?i={0}&s={1}&j=t#{2}\">View</a>", id, storyID,id);

  }



}
