<%@ Page EnableEventValidation="false" 
 Language="C#" MasterPageFile="~/admin/Main.master" AutoEventWireup="true" CodeFile="CollectionManagerDetail.aspx.cs" Inherits="admin_CollectionManagerDetail" Title="Administration Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


		<script language="javascript" type="text/javascript">
			/// <summary>
			/// Launches the DatePicker page in a popup window, 
			/// passing a JavaScript reference to the field that we want to set.
			/// </summary>
			/// <param name="strField">String. The JavaScript reference to the field that we want to set, in the format: FormName.FieldName
			/// Please note that JavaScript is case-sensitive.</param>
			function calendarPicker(strField)
			{
				window.open('../common/DatePicker.aspx?field=' + strField,'calendarPopup','width=250,height=190,resizable=yes');
			}
		</script>


    <asp:Label ID="Label1" runat="server" Text="Collction Manager - Edit"></asp:Label>
    
    <br />
    <table class="style6" style="height: 24px; width: 800px;">
        <tr>
            <td class="style19">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="100px" 
                    onclick="btnDelete_Click" />
            </td>
            <td align="left" class="style18">
                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100px" 
                    onclick="btnAdd_Click" />
            </td>
            <td class="style17" align="right">
                <asp:Button ID="btnRegister" runat="server" Text="Generate Collection File" 
                    Width="170px" onclick="btnRegister_Click" />
            </td>
            <td align="right">
                <asp:LinkButton ID="btnApplyLayout" runat="server" Text="Apply Layout" 
                    Width="140px" onclick="btnApplyLayout_Click"/>
            </td>
        </tr>
    </table>
    <table class="style6" style="margin-right: 0px">
        <tr>
            <td class="style10" colspan="5">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" headertext="Please correct the following errors:" />
            </td>
        </tr>
        <tr>
            <td class="style21">
                Title :</td>
            <td class="style20">
                <asp:TextBox ID="txtTitle" runat="server" Width="123px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" controltovalidate="txtTitle" display="None" ErrorMessage="Title is required."></asp:RequiredFieldValidator>
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style9">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style21">
                Apply Start Date:</td>
            <td class="style20">
                <asp:TextBox ID="txtApplyStartDate" ReadOnly="false" runat="server" Width="100px" maxlength="10" 
                  columns="10" style="TEXT-ALIGN: right"></asp:TextBox>
                <a href="javascript:;" onclick="calendarPicker('aspnetForm.ctl00_ContentPlaceHolder1_txtApplyStartDate');" title="Pick Date from Calendar">
                pick</a>
  
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style9">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style21">
                Apply End Date:</td>
            <td class="style20">
                <asp:TextBox ID="txtApplyEndDate" ReadOnly="false" runat="server" Width="100px" maxlength="10" 
                  columns="10" style="TEXT-ALIGN: right"></asp:TextBox>
                <a href="javascript:;" onclick="calendarPicker('aspnetForm.ctl00_ContentPlaceHolder1_txtApplyEndDate');" title="Pick Date from Calendar">
                pick</a>
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style9">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        
      <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtApplyStartDate" ControlToCompare="txtApplyEndDate" display="None" Type="Date" Operator="LessThan" runat="server" ErrorMessage="Apply End Date SHOULD BE set Greater than Apply Start Date."></asp:CompareValidator>
        
    </table>
    <div style="overflow:auto; width:800px; height:515px">
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
            </Columns>
        </asp:DataGrid>
    </div>
    
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
     <style type="text/css">
        .style9
        {
            width: 167px;
        }
        .style10
        {
       }
        .style12
        {
            width: 46px;
        }
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
       .style20
       {
         width: 250px;
       }
       .style21
       {
         width: 123px;
       }
    </style>
</asp:Content>

