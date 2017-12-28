using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

/// <summary>
/// Semi-Dynamic Search API Class
/// </summary>
public partial class semi_dynamic : System.Web.UI.Page
{
    public string exec       = "";
    public string cid        = "";
    public string lid        = "";
    public string condition1 = "";
    public string condition2 = "";
    public string searchType = "";
    public string order      = "";
    public string orderby    = "";
    public string filter_uid = "";

    public Boolean IsPageError = false;
    public string ExecStatus = "";


    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // ********************************************
        // Get DB Config from web.config
        // * Read the connection string from web.config--regardless it is encrypted or clear
        // *********************************************
        string connString = ConfigurationManager.ConnectionStrings["LocalJFish"].ConnectionString;

        // ********************************************
        //  Setup
        // ********************************************
        Utility util = new Utility();

        // ********************************************
        //  Get query string from Request
        // ********************************************
        exec = Request.QueryString["exec"];
        cid = Request.QueryString["cid"];
        lid = Request.QueryString["lid"];
        condition1 = Request.QueryString["condition1"];
        condition2 = Request.QueryString["condition2"];
        searchType = Request.QueryString["searchType"];
        order      = Request.QueryString["order"];
        orderby    = Request.QueryString["orderby"];
        filter_uid = Request.QueryString["filter_uid"];


        try
        {
            if ("list".Equals(exec))
            {
                if (String.IsNullOrEmpty(exec))
                {
                    Notify_ExecStatus("E001", "Param Error : exec is not found...");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // ***********************************
                    // Create SQL Command
                    // ***********************************
                    string cmdText = "SELECT a.CId AS CId, a.Url AS CUrl, b.LId AS LId, c.Url AS LUrl, a.Title AS CTitle, c.Title AS LTitle ";
                    cmdText += "FROM collection_info AS a INNER JOIN ";
                    cmdText += "collection_layout_info AS b ON a.CId = b.CId INNER JOIN ";
                    cmdText += "layout_info AS c ON b.LId = c.LId ";
                    cmdText += "WHERE a.ApplyStartDate <= GETDATE() AND a.ApplyEndDate > GETDATE()";

                    // ***********************************
                    // Execute SQL Command
                    // ***********************************
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "collection_info");

                    // ***********************************
                    // Set SQL Result into Repeater
                    // ***********************************
                    Collection_Item.DataSource = ds.Tables["collection_info"];
                    Collection_Item.DataBind();
                }
            }
            else if ("init".Equals(exec))
            {
                if (String.IsNullOrEmpty(cid))
                {
                    Notify_ExecStatus("E001", "Param Error : cid is not found...");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // ***********************************
                    // Create SQL Command
                    // ***********************************
                    string cmdText = "SELECT a.* FROM upload_info AS a INNER JOIN collection_rel_info AS c ON a.UId = c.UId ";

                    cmdText += "WHERE c.CId = '" + cid + "'";

                    cmdText += "ORDER BY a.Uid ASC";

                    // For Debug
                    //Response.Write(cmdText);

                    // ***********************************
                    // Execute SQL Command
                    // ***********************************
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "upload_info");

                    // ***********************************
                    // Set SQL Result into Repeater
                    // ***********************************
                    Image_Item.DataSource = ds.Tables["upload_info"];
                    Image_Item.DataBind();
                }  // End of SqlConnection
            }
            else if ("search".Equals(exec))
            {

                if (String.IsNullOrEmpty(cid))
                {
                    Notify_ExecStatus("E001", "Param Error : cid is not found...");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {

                    // ***********************************
                    // Create SQL Command
                    // ***********************************
                    string cmdText = "SELECT a.*, b.Tag FROM upload_info AS a LEFT OUTER JOIN tag_info AS b ON a.UId = b.UId INNER JOIN collection_rel_info AS c ON a.UId = c.UId ";

                    cmdText += "WHERE c.CId = '" + cid + "' ";


                    if (!String.IsNullOrEmpty(condition1) || !String.IsNullOrEmpty(condition2))
                    {

                        cmdText += "AND ( ";

                        bool IsExistCondition = false;

                        if (!String.IsNullOrEmpty(condition1))
                        {
                            IsExistCondition = true;
                            cmdText += "UPPER(a.Title) LIKE '%" + condition1.ToUpper() + "%' ";
                        }

                        if (!String.IsNullOrEmpty(condition2))
                        {
                            if (IsExistCondition)
                            {
                                if (String.IsNullOrEmpty(searchType))
                                {
                                    Notify_ExecStatus("E001", "Param Error : searchType is not found...");
                                    return;
                                }

                                if ("and".Equals(searchType))
                                {
                                    cmdText += "AND ";
                                }
                                else
                                {
                                    cmdText += "OR ";
                                }
                            }
                            cmdText += "( ";

                            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                            string[] list_condition2 = condition2.Split(delimiterChars);

                            int cnt_list_condition2 = 0;
                            foreach (string s in list_condition2)
                            {
                                if (cnt_list_condition2 > 0)
                                {
                                    cmdText += "OR ";
                                }
                                cmdText += "b.Tag LIKE '" + s.Trim().ToUpper() + "%' ";
                                cnt_list_condition2++;
                            }

                            cmdText += ") ";
                        }
                        cmdText += ") ";
                    }


                    if (!String.IsNullOrEmpty(filter_uid))
                    {
                        char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                        string[] list_filter_uid = filter_uid.Split(delimiterChars);

                        cmdText += "AND a.UId NOT IN ( ";

                        int cnt_list_filter_uid = 0;
                        foreach (string s in list_filter_uid)
                        {
                            if (cnt_list_filter_uid > 0)
                            {
                                cmdText += ", ";
                            }
                            cmdText += "'" + s + "'";
                            cnt_list_filter_uid++;
                        }

                        cmdText += ") ";
                    }


                    //OrderBy
                    if (!String.IsNullOrEmpty(order) && !String.IsNullOrEmpty(orderby))
                    {
                        cmdText += "ORDER BY ";
                        if ("Title".Equals(orderby))
                        {
                            cmdText += "a.Title ";
                        }
                        else if ("Tag".Equals(orderby))
                        {
                            cmdText += "b.Tag ";
                        }
                        else
                        {
                            cmdText += "a.Title ";
                        }

                        cmdText += order;
                    }
                    else
                    {
                        cmdText += "ORDER BY a.UId ASC";
                    }

                    // For Debug
                    //Response.Write(cmdText);

                    // ***********************************
                    // Execute SQL Command
                    // ***********************************
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "upload_info");


                    DataSet CpyDs = new DataSet();
                    CpyDs = ds.Clone();

                    Dictionary<string, string> uid_hash = new Dictionary<string, string>();

                    foreach (DataRow drCurrent in ds.Tables["upload_info"].Rows)
                    {
                        string key = drCurrent["UId"].ToString();

                        if (!uid_hash.ContainsKey(key))
                        {
                            uid_hash.Add(key, key + ".jpg");
                            //Uidが重複しない場合のみインポート
                            CpyDs.Tables["upload_info"].ImportRow(drCurrent);
                        }
                    }

                    // ***********************************
                    // Set SQL Result into Repeater
                    // ***********************************
                    Image_Item.DataSource = CpyDs.Tables["upload_info"];
                    Image_Item.DataBind();
                }  // End of SqlConnection
            }
            else if ("layout".Equals(exec))
            {
                if (String.IsNullOrEmpty(lid))
                {
                    Notify_ExecStatus("E001", "Param Error : lid is not found...");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // ***********************************
                    // Create SQL Command
                    // ***********************************
                    string cmdText = "SELECT  LId, Url AS LUrl, Title AS LTitle FROM layout_info ";
                    
                    cmdText += "WHERE LId = '" + lid + "'";

                    // ***********************************
                    // Execute SQL Command
                    // ***********************************
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "layout_info");

                    // ***********************************
                    // Set SQL Result into Repeater
                    // ***********************************
                    Layout_Item.DataSource = ds.Tables["layout_info"];
                    Layout_Item.DataBind();
                }
            }

            // Here comes with no error
            Notify_ExecStatus("200", "OK");

        } catch (Exception ex) {
            Notify_ExecStatus("E999", "System Error : " + ex.Message);
        }
    }

    /// <summary>
    /// Notify the status of each execution.
    /// </summary>
    /// <remarks>Non</remarks>
    /// <param name="ExecStatusCode">ExecStatusCode</param>
    /// <param name="ExecStatusMsg">ExecStatusMsg</param>
    protected void Notify_ExecStatus(string ExecStatusCode, string ExecStatusMsg)
    {
        if(!"200".Equals(ExecStatusCode)) {
            IsPageError = true;
        }

        ExecStatus = "<status code=\"" + ExecStatusCode + "\">"+ ExecStatusMsg +"</status>";
    }

}
