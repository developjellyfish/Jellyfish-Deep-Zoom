<%@ Page Language="c#" CodeFile="DatePicker.aspx.cs" AutoEventWireup="false" Inherits="DatePicker" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
  <title>DatePicker</title>
  <style type="text/css">
    BODY
    {
      padding-right: 0px;
      padding-left: 0px;
      padding-bottom: 0px;
      margin: 4px;
      padding-top: 0px;
    }
    BODY
    {
      font-size: 9pt;
      font-family: Verdana, Geneva, Sans-Serif;
    }
    TABLE
    {
      font-size: 9pt;
      font-family: Verdana, Geneva, Sans-Serif;
    }
    TR
    {
      font-size: 9pt;
      font-family: Verdana, Geneva, Sans-Serif;
    }
    TD
    {
      font-size: 9pt;
      font-family: Verdana, Geneva, Sans-Serif;
    }
  </style>
</head>

<body onblur="this.window.focus();" ms_positioning="FlowLayout">

  <form id="Form1" method="post" runat="server">
  <div align="center">
    <asp:Calendar ID="Calendar1" runat="server" ShowGridLines="True" BorderColor="Black">
      <TodayDayStyle ForeColor="White" BackColor="#FFCC66"></TodayDayStyle>
      <SelectorStyle BackColor="#FFCC66"></SelectorStyle>
      <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC"></NextPrevStyle>
      <DayHeaderStyle Height="1px" BackColor="#FFCC66"></DayHeaderStyle>
      <SelectedDayStyle Font-Bold="True" BackColor="#CCCCFF"></SelectedDayStyle>
      <TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="#FFFFCC" BackColor="#990000">
      </TitleStyle>
      <OtherMonthDayStyle ForeColor="#CC9966"></OtherMonthDayStyle>
    </asp:Calendar>
  </div>
  </form>

</body>

</html>
