<%@ Page  Title="Administration Page" Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="CollectionManagerApplyLayoutDetail.aspx.cs" Inherits="admin_CollectionManagerApplyLayoutDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <script type="text/javascript">
        function onSilverlightError(sender, args) {
        
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            } 
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;
            
            var errMsg = "Unhandled Error in Silverlight 2 Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError")
            {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError")
            {           
                if (args.lineNumber != 0)
                {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
  </script>

    <asp:Label ID="Label1" runat="server" Text="Layout Manager - View"></asp:Label>
    <table>
        <tr>
            <td valign="top">
                <table class="style6" style="height: 336px">
                    <tr>
                        <td class="style12">
                            LID :</td>
                        <td>
                            <asp:Label ID="lblLId" runat="server" Text=""></asp:Label>
                                                </td>
                    </tr>
                    <tr>
                        <td class="style12">
                            Title :</td>
                        <td>
                            <asp:TextBox ID="txtTitle" ReadOnly="true" runat="server" Width="285px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style12">
                            Description :</td>
                        <td>
                            <asp:TextBox ID="txtDescription" ReadOnly="true" runat="server" TextMode="MultiLine" 
                                Width="283px" Height="110px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style13">
                            Share :</td>
                        <td class="style14">
                            <asp:CheckBox ID="cbxShare" Enabled="false" runat="server" />
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                <table class="style6">
                    <tr>
                        <td class="style15">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Back" Width="100px" 
                                onclick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    
    <div id="dzcPath" runat="server" >
    </div>
    <div id="silverlightControlHost" runat="server">
      <object data="data:application/x-silverlight," type="application/x-silverlight-2"
        width="100%" height="100%">
        <param name="source" value="../DeepZoomProject.xap" />
        <param name="onerror" value="onSilverlightError" />
        <param name="background" value="white" />
  		<param name="minRuntimeVersion" value="2.0.30923.0" />
		<param name="autoUpgrade" value="true" />
        <a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">
          <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight"
            style="border-style: none" />
        </a>
      </object>
      <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style12
        {
            width: 96px;
        }
        .style13
        {
            width: 96px;
            height: 39px;
        }
        .style14
        {
            height: 39px;
        }
        .style15
        {
            width: 93px;
        }
    </style>

</asp:Content>


