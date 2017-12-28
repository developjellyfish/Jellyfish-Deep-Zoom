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
/// Dynamic Search API Class
/// </summary>
public partial class dynamic : System.Web.UI.Page
{
    public string condition1 = "";
    public string condition2 = "";
    public string searchType = "";
    public string order      = "";
    public string orderby    = "";
    public string sid        = "";
    public string CollectionXmlFile = "";
    public string DzcPath = "";

    public Boolean IsPageError = false;
    public string ExecStatus = "";

    private string CollectionXmlRootDir = "../sl/out/";
    private string DzcPathRoot = "out/";


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
        condition1 = Request.QueryString["condition1"];
        condition2 = Request.QueryString["condition2"];
        searchType = Request.QueryString["searchType"];
        order      = Request.QueryString["order"];
        orderby    = Request.QueryString["orderby"];
        sid        = Request.QueryString["sid"];


        try
        {

            if (String.IsNullOrEmpty(searchType))
            {
                Notify_ExecStatus("E001", "Param Error : searchType is not found...");
                return;
            }

            if (String.IsNullOrEmpty(order))
            {
                Notify_ExecStatus("E001", "Param Error : order is not found...");
                return;
            }

            if (String.IsNullOrEmpty(orderby))
            {
                Notify_ExecStatus("E001", "Param Error : orderby is not found...");
                return;
            }

            if (String.IsNullOrEmpty(sid))
            {
                sid = GetSId();
            }


            using (SqlConnection conn = new SqlConnection(connString))
            {
                // ***********************************
                // Create SQL Command
                // ***********************************
                string cmdText = "SELECT a.*, b.Tag FROM upload_info AS a LEFT OUTER JOIN tag_info AS b ON a.UId = b.UId ";

                if (!String.IsNullOrEmpty(condition1) || !String.IsNullOrEmpty(condition2))
                {
                    cmdText += "WHERE ";
                }

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

                //OrderBy
                if (!String.IsNullOrEmpty(order) && !String.IsNullOrEmpty(orderby))
                {
                    cmdText += "ORDER BY ";
                    if("Title".Equals(orderby))
                    {
                        cmdText += "a.Title ";
                    }
                    else if("Tag".Equals(orderby))
                    {
                        cmdText += "b.Tag ";
                    }
                    else
                    {
                        cmdText += "a.Title ";
                    }

                    cmdText += order;
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

                    if(!uid_hash.ContainsKey(key)){
                        uid_hash.Add(key, key+".jpg");
                        //Uidが重複しない場合のみインポート
                        CpyDs.Tables["upload_info"].ImportRow(drCurrent);
                    }
                }

                int maxNum = Utility.getMaxNumResultsForDynamicSearch();

                if (CpyDs.Tables["upload_info"].Rows.Count > maxNum)
                {
                    Notify_ExecStatus("E002", "Over " + maxNum + " results. Please narrow down conditions.");
                    return;
                }


                // ***********************************
                // Flush exiting files.
                // ***********************************
                if (Directory.Exists(GetSIdDirPath(sid)))
                {
                    util.DeleteFiles(GetSIdDirPath(sid));
                }
                else
                {
                    Directory.CreateDirectory(GetSIdDirPath(sid));
                }


                // ***********************************
                // Write target list of converting to DZC.
                // ***********************************
                Application.Lock();

                String targetList = "";
                foreach (string value in uid_hash.Values)
                {
                    targetList += value + "\n";
                }

                StreamWriter write = new StreamWriter(GetSIdDirPath(sid) + "source_image_list.txt");

                write.Write(targetList);
                write.Close();
                Application.UnLock();


                // **************************************************
                // Execute DzcConverter.exe as Waitting Process
                // **************************************************
                // Create Radom extension for collection.xml
                CollectionXmlFile = getCollectionXmlName();
                DzcPath = getDzcPath();
                ProcessStartInfo startInfo = new ProcessStartInfo(GetPhysicalPath(@"Bin\DzcConv\DzcConverter.exe"));
                startInfo.Arguments = GetSourceFilesDirPath() + " " + GetSIdDirPath(sid) + " 256 90 5 5 " + CollectionXmlFile + " " + GetUriBasePath() + "sl/out/" + " " + GetSIdDirPath(sid) + "source_image_list.txt" + " " + GetPhysicalPath(@"sl\out\collection_images\");
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                System.Diagnostics.Process _Process = Process.Start(startInfo);

                // Waiting until this process will exit...
                _Process.WaitForExit();
                _Process.Close();
                _Process.Dispose();


                // ***********************************
                // Set SQL Result into Repeater
                // ***********************************
                Image_Item.DataSource = CpyDs.Tables["upload_info"];
                Image_Item.DataBind();

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


    /// <summary>
    /// Get collection xml file name.
    /// </summary>
    /// <returns>string</returns>
    public string getCollectionXmlName()
    {
        return "collection." + (new Random()).Next(1, 999).ToString("000");
    }

    /// <summary>
    /// Get Dzc path.
    /// </summary>
    /// <returns>string</returns>
    public string getDzcPath()
    {
        return DzcPathRoot + sid + "/" + CollectionXmlFile;
    }

    /// <summary>
    /// Get SId.
    /// </summary>
    /// <returns>string</returns>
    private string GetSId()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(1, 999).ToString("000");
    }

    /// <summary>
    /// Get the sid directory path.
    /// </summary>
    /// <param name="sid">sid</param>
    /// <returns>string</returns>
    private string GetSIdDirPath(string sid)
    {
        return Server.MapPath(".") + CollectionXmlRootDir + sid + "/";
    }

    /// <summary>
    /// Get the source files directory path.
    /// </summary>
    /// <returns>string</returns>
    private string GetSourceFilesDirPath()
    {
        return Server.MapPath(".") + CollectionXmlRootDir + "source_files/";
    }

    /// <summary>
    /// Get the URL Base Path.
    /// </summary>
    /// <returns>string</returns>
    private string GetUriBasePath()
    {
        Uri basePath = new Uri(Request.Url.AbsoluteUri);
        Uri destPath = new Uri(basePath, ".");
        return destPath.AbsoluteUri;
    }

    /// <summary>
    /// Get Physical path
    /// </summary>
    /// <remarks>
    /// Non
    /// </remarks>
    /// <param name="path">
    /// path
    /// </param>
    /// <returns>
    /// string
    /// </returns>
    public string GetPhysicalPath(string path)
    {
        Uri basePath = new Uri(Request.PhysicalApplicationPath);
        Uri destPath = new Uri(basePath, ".");
        return destPath.LocalPath + path;
    }
}
