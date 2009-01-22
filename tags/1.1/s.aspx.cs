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


public partial class s : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      BindTimeZones();
      string timeAdjust = "0";
      using (ShackToGoDataContext dc = new ShackToGoDataContext())
      {
        if (Request.QueryString["u"] != null)
        {
          ShackUser user = dc.ShackUsers.Where(u => u.UserName == Request.QueryString["u"]).SingleOrDefault();
          if (user != null)
          {
            timeAdjust = user.TimeAdjustment.ToString();
            this.CheckBoxThreading.Checked = user.EnableThreadedView;
            this.CheckBoxTextThreading.Checked = user.EnableThreadTextDisplay;
            this.DropDownListTimeZone.ClearSelection();
            
            // TODO: Can probably use a string format for this.. will fix later
            if (user.TimeAdjustment > 0)
              this.DropDownListTimeZone.Items.FindByText("+" + user.TimeAdjustment.ToString()).Selected = true;
            else
              this.DropDownListTimeZone.Items.FindByText(user.TimeAdjustment.ToString()).Selected = true;
          }
        }
      }
    }
  }
  protected void BindTimeZones()
  {
    for (int i = -11; i <= 11; i++)
    {
      if (i > 0)
        this.DropDownListTimeZone.Items.Add(new ListItem("+" + i.ToString(), i.ToString()));
      else
        this.DropDownListTimeZone.Items.Add(new ListItem(i.ToString(), i.ToString()));
    }
  }
  protected void ButtonSave_Click(object sender, EventArgs e)
  {
    using (ShackToGoDataContext dc = new ShackToGoDataContext())
    {
      if (Request.QueryString["u"] != null)
      {
        ShackUser user = dc.ShackUsers.Where(u => u.UserName == Request.QueryString["u"]).SingleOrDefault();
        if (user != null)
        {
          user.EnableThreadedView = Convert.ToBoolean(this.CheckBoxThreading.Checked);
          user.TimeAdjustment = Convert.ToInt32(this.DropDownListTimeZone.SelectedValue);
          user.EnableThreadTextDisplay = Convert.ToBoolean(this.CheckBoxTextThreading.Checked);
          dc.SubmitChanges();

        }
      }
    }
    this.PanelDone.Visible = true;
    this.PanelForm.Visible = false;
  }
}
