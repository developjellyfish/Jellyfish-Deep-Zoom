<%@ Page EnableEventValidation="false"
    Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="ImageManagerList.aspx.cs" Inherits="admin_ImageManagerList"  Title="Administration Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Image Manager"></asp:Label>
    
    <br />
    <table class="style6" style="height: 24px; width: 800px;">
        <tr>
            <td class="style9">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="140px" 
                    onclick="btnDelete_Click" />
            </td>
            <td align="left" class="style8">
                <asp:Button ID="btnCreateCollection" runat="server" Text="Create Collction" 
                    Width="140px" onclick="btnCreateCollection_Click" />
            </td>
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
            <td class="style12">
                Tag :</td>
            <td class="style9">
                <asp:TextBox ID="txtTag" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="140px" 
                    onclick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <div style="overflow:auto; width:800px; height:510px">
        <asp:DataGrid id="InfoGrid"
            AutoGenerateColumns="False"
            CellPadding="4"
            DataKeyField="UId"
            OnItemCreated="InfoGrid_OnItemCreated"
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
                
                <asp:BoundColumn DataField="Owner" HeaderText="Owner" ItemStyle-Width="30" >
                
<ItemStyle Width="30px"></ItemStyle>
                </asp:BoundColumn>
                
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