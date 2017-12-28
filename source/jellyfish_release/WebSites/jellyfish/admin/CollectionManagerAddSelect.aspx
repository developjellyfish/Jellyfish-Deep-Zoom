<%@ Page  Title="Administration Page" Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="CollectionManagerAddSelect.aspx.cs" Inherits="admin_CollectionManagerAddSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Collction Manager - Add Image Select"></asp:Label>
    
    <br />
    <table bgcolor="#FFFFCC" class="style6" style="height: 24px; width: 800px;">
        <tr>
            <td class="style19">
                <asp:Button ID="btnCommit" runat="server" Text="OK" Width="100px" 
                    onclick="btnCommit_Click" />
            </td>
            <td align="left" class="style18">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" 
                    onclick="btnCancel_Click" />
            </td>
            <td class="style17" align="right">
                &nbsp;</td>
            <td align="right">
                &nbsp;</td>
        </tr>
    </table>
    <div style="overflow:auto; width:800px; height:540px">
        <asp:DataGrid id="InfoGrid"
            AutoGenerateColumns="false"
            CellPadding="4"
            DataKeyField="UId"
            runat="server"
            >
            
            <HeaderStyle BackColor="#BB2255" ForeColor="white" />
            <ItemStyle   BackColor="#FFEEEE" />
            <AlternatingItemStyle BackColor="#FFDDDD" />
            <Columns>
                <asp:TemplateColumn runat="server" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" id="cbxSelect" value=""/>
                    </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:BoundColumn DataField="Share" HeaderText="Share" ItemStyle-Width="30" />
                
                <asp:BoundColumn DataField="Thumbnail" HeaderText="Thumbnail" />
                
                <asp:BoundColumn DataField="Title" HeaderText="Title" ItemStyle-Width="150" />
                
                <asp:BoundColumn DataField="Description" HeaderText="Discription" ItemStyle-Width="300" />
                
                <asp:BoundColumn DataField="Tag" HeaderText="Tag" ItemStyle-Width="300" />
            </Columns>
        </asp:DataGrid>
    </div>
    
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
     <style type="text/css">
         .style17
         {
             width: 337px;
         }
         .style18
         {
             width: 203px;
         }
         .style19
         {
             width: 121px;
         }
    </style>
</asp:Content>



