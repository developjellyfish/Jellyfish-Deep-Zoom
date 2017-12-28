<%@ Page Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="ImageManagerDetail.aspx.cs" Inherits="admin_ImageManagerDetail"  Title="Administration Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Image Manager - Edit"></asp:Label>
    <table>
        <tr>
            <td valign="top">
                <table class="style6" style="height: 336px">
                    <tr>
                        <td class="style12">
                            UID :</td>
                        <td>
                            <asp:Label ID="lblUId" runat="server" Text=""></asp:Label>
                                                </td>
                    </tr>
                    <tr>
                        <td class="style12">
                            Title :</td>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" Width="285px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style12">
                            Description :</td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                                Width="283px" Height="110px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style12">
                            Tag :</td>
                        <td>
                            <asp:TextBox ID="txtTag" runat="server" Width="285px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style13">
                            Share :</td>
                        <td class="style14">
                            <asp:CheckBox ID="cbxShare" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            </td>
                        <td>
                            <asp:Image ID="imgThumbnail" runat="server" />
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
                            <asp:Button ID="btnCommit" runat="server" Text="OK" Width="100px" 
                                onclick="btnCommit_Click" style="height: 26px" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" 
                                onclick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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


