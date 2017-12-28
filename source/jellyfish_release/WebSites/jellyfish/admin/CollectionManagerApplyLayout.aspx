<%@ Page EnableEventValidation="false"
     Title="Administration Page" Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="CollectionManagerApplyLayout.aspx.cs" Inherits="admin_CollectionManagerApplyLayout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Collection Manager - Apply Layout"></asp:Label>
    <br />

    <table bgcolor="#FFFFCC" class="style6" style="height: 24px; width: 800px;">
        <tr>
            <td class="style9">
                <asp:Button ID="btnApply" runat="server" Text="Apply" Width="140px" 
                    onclick="btnApply_Click" />
            </td>
            <td align="left" class="style8">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <table class="style6" style="margin-right: 0px">
        <tr>
            <td class="style10">
                Title :</td>
            <td class="style11">
                <asp:TextBox ID="txtTitle" runat="server" Width="123px"></asp:TextBox>
            </td>
            <td class="style12"></td>
            <td class="style9">
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="140px" 
                    onclick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <div style="overflow:auto; width:800px; height:510px">
        <asp:DataGrid id="InfoGrid"
            AutoGenerateColumns="false"
            CellPadding="4"
            DataKeyField="LId"
            OnItemCreated="InfoGrid_OnItemCreated"
            runat="server" OnItemCommand="InfoGrid_OnItemCommand" Width="800px"
            >
            
            <HeaderStyle BackColor="#BB2255" ForeColor="white" />
            <ItemStyle   BackColor="#FFEEEE" />
            <AlternatingItemStyle BackColor="#FFDDDD" />
            <Columns>
                <asp:TemplateColumn runat="server" HeaderText="Select" ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" id="cbxSelect" value=""/>
                    </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateColumn>

                <asp:BoundColumn DataField="Owner" HeaderText="Owner" ItemStyle-Width="30" >
               
<ItemStyle Width="30px"></ItemStyle>
                </asp:BoundColumn>

                <asp:BoundColumn DataField="Share" HeaderText="Share" ItemStyle-Width="30" >
               
<ItemStyle Width="30px"></ItemStyle>
                </asp:BoundColumn>
               
                <asp:BoundColumn DataField="Title" HeaderText="Title" ItemStyle-Width="150" >
                
<ItemStyle Width="150px"></ItemStyle>
                </asp:BoundColumn>
                
                <asp:BoundColumn DataField="Description" HeaderText="Discription" 
                  ItemStyle-Width="300" >
                
<ItemStyle Width="300px"></ItemStyle>
                </asp:BoundColumn>
                
                <asp:ButtonColumn
                    Text="View"
                    CommandName="View"
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
        .style10
        {
            width: 60px;
        }
        .style11
        {
            width: 140px;
        }
        .style12
        {
            width: 46px;
        }
    </style>
</asp:Content>