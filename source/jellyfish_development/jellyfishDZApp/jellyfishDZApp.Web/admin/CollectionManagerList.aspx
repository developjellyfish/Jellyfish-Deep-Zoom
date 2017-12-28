<%@ Page EnableEventValidation="false"
 Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="CollectionManagerList.aspx.cs" Inherits="admin_CollectionManagerList" Title="Administration Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Collection Manager"></asp:Label>
    
    <br />
    <table class="style6" style="height: 24px; width: 800px;">
        <tr>
            <td class="style9">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="140px" 
                    onclick="btnDelete_Click" />
            </td>
            <td align="left" class="style8">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <div style="overflow:auto; width:800px; height:540px">
        <asp:DataGrid id="InfoGrid"
            AutoGenerateColumns="False"
            CellPadding="4"
            DataKeyField="CId"
            runat="server" OnItemCommand="InfoGrid_OnItemCommand" ForeColor="#333333" GridLines="None"
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
                                
                <asp:BoundColumn DataField="Title" HeaderText="Title" ItemStyle-Width="150" >
                               
<ItemStyle Width="150px"></ItemStyle>
                </asp:BoundColumn>
                               
                <asp:BoundColumn DataField="Date" HeaderText="Date" ItemStyle-Width="150" >
                
<ItemStyle Width="150px"></ItemStyle>
                </asp:BoundColumn>
                
                <asp:ButtonColumn
                    Text="Edit"
                    CommandName="Edit"
                    ButtonType="PushButton"
                    HeaderStyle-Wrap="false"
                    ItemStyle-Width="80"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    HeaderText="Action">
<HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                </asp:ButtonColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style8
        {
            width: 309px;
        }
        .style9
        {
            width: 167px;
        }
    </style>
</asp:Content>