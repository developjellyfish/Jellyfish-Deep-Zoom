using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for DatePicker.
/// </summary>
public partial class DatePicker : System.Web.UI.Page
{

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void Page_Load(object sender, System.EventArgs e)
    {
        // Put user code to initialize the page here
    }

    #region Web Form Designer generated code
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Calendar1.DayRender += new System.Web.UI.WebControls.DayRenderEventHandler(this.Calendar1_DayRender);
        this.Load += new System.EventHandler(this.Page_Load);
    }
    #endregion



    /// <summary>
    /// Replaces the standard post-back link for each calendar day 
    /// with the javascript to set the opener window's TextBox text.
    /// Eliminates a needless round-trip to the server.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Calendar1_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
    {
        // Clear the link from this day
        e.Cell.Controls.Clear();

        // Add the custom link
        System.Web.UI.HtmlControls.HtmlGenericControl Link = new System.Web.UI.HtmlControls.HtmlGenericControl();
        Link.TagName = "a";
        Link.InnerText = e.Day.DayNumberText;
        Link.Attributes.Add("href", String.Format("JavaScript:window.opener.document.{0}.value = \'{1:d}\'; window.close();", Request.QueryString["field"], e.Day.Date));

        // By default, this will highlight today's date.
        if (e.Day.IsSelected)
        {
            Link.Attributes.Add("style", this.Calendar1.SelectedDayStyle.ToString());
        }

        // Now add our custom link to the page
        e.Cell.Controls.Add(Link);
    }

}

