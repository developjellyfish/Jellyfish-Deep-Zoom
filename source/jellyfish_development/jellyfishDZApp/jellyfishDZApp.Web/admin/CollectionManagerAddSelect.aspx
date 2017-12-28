<%@ Page  Title="Administration Page" Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="CollectionManagerAddSelect.aspx.cs" Inherits="admin_CollectionManagerAddSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Collction Manager - Add Image Select"></asp:Label>
    
    <br />
    <table class="style6" style="height: 24px; width: 800px;">
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
            AutoGenerateColumns="False"
            CellPadding="4"
            DataKeyField="UId"
            runat="server" ForeColor="#333333" GridLines="None"
            >
            
            <HeaderStyle BackColor="#507CD1" ForeColor="white" Font-Bold="True" />
            <ItemStyle   BackColor="#EFF3FB" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditItemStyle BackColor="#2461BF" />
            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <AlternatingItemStyle BackColor="White" />
            <Columns>
                <asp:TemplateColumn runat="server" HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" id="cbxSelect" value=""/>
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>
                
                <asp:BoundColumn DataField="Share" HeaderText="Share" ItemStyle-Width="30" >
                
<ItemStyle Width="30px"></ItemStyle>
                </asp:BoundColumn>
                
                <asp:BoundColumn DataField="Thumbnail" HeaderText="Thumbnail" />
                
                <asp:BoundColumn DataField="Title" HeaderText="Title" ItemStyle-Width="150" >
                
<ItemStyle Width="150px"></ItemStyle>
                </asp:BoundColumn>
                
                <asp:BoundColumn DataField="Description" HeaderText="Discription" 
                    ItemStyle-Width="300" >
                
<ItemStyle Width="300px"></ItemStyle>
                </asp:BoundColumn>
                
                <asp:BoundColumn DataField="Tag" HeaderText="Tag" ItemStyle-Width="300" >
<ItemStyle Width="300px"></ItemStyle>
                </asp:BoundColumn>
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



