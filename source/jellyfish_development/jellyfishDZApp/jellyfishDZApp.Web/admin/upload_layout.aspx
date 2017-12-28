<%@ Page Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true"
  CodeFile="upload_layout.aspx.cs" Inherits="admin_upload_layout"  Title="Administration Page" %>

<%@ MasterType virtualpath="~/admin/Main.master" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

  <asp:Label ID="Label1" runat="server" Text="Upload Layout"></asp:Label>
  <br />

  <div>
    <p>
      Select your layout file and Press [Upload] button.</p>
      
      <table class="style6">
          <tr>
              <td class="style8">
                  Upload your layout file :</td>
              <td>
                  <input id="File1" type="file" name="userfile" runat="server"></td>
          </tr>
          <tr>
              <td class="style8">
                  Title :</td>
              <td>
                  <asp:TextBox ID="TitleTextBox" runat="server"></asp:TextBox></td>
          </tr>
          <tr>
              <td class="style8">
                  Description :</td>
              <td>
                  <asp:TextBox ID="DescTextBox" TextMode="MultiLine" runat="server" Height="84px" Width="202px"></asp:TextBox></td>
          </tr>
      </table>

    <p>
      <asp:CheckBox ID="IsShareCheckBox" Text="Share this file ?" runat="server" />
    </p>
    <p>
      <asp:Button ID="Button_Upload" runat="server" Text="Upload" OnClick="Button_Upload_Click" /></p>
    <span id="Span1" runat="Server"></span>
    <div id="dzcPath" runat="server" >
    </div>
    <div id="silverlightControlHost" runat="server">
      <object data="data:application/x-silverlight," type="application/x-silverlight-2"
        width="100%" height="100%">
        <param name="source" value="../DeepZoomProject.xap" />
        <param name="onerror" value="onSilverlightError" />
<%--        <param name="background" value="white" />
--%>
        <param name="background" value="#181818" />
   		<param name="minRuntimeVersion" value="2.0.30923.0" />
		<param name="autoUpgrade" value="true" />
        <a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">
          <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight"
            style="border-style: none" />
        </a>
      </object>
      <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style8
        {
            width: 161px;
        }
    </style>

</asp:Content>

