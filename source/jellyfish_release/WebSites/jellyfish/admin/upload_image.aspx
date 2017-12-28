<%@ Page Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true"
  CodeFile="upload_image.aspx.cs" Inherits="admin_upload_image"  Title="Administration Page" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <asp:Label ID="Label1" runat="server" Text="Upload Image"></asp:Label>
  <br />
  <br />
  <div style="height: 330px">
    <p>
      Select image file and Press [Upload] button.
        
    </p>
    <table class="style6">
            <tr>
                <td class="style8">
                     Upload your image file :</td>
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
            <tr>
                <td class="style8">
                   Tag :</td>
                <td>
                   <asp:TextBox ID="TagTextBox" runat="server"></asp:TextBox></td>
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

  </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style8
        {
            width: 160px;
        }
    </style>

</asp:Content>

