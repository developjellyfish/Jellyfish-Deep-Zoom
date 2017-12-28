<?xml version="1.0" encoding="utf-8" ?>
<%@ Page ContentType="text/xml" Language="C#" AutoEventWireup="true" CodeFile="dynamic.aspx.cs" Inherits="dynamic" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<Metadata version="1">

    <%= ExecStatus %>

<% if (!IsPageError) { %>

    <% if (Image_Item.Items.Count != 0)
       { %>
        <AspectRatio></AspectRatio>
        <Dzcpath><%= DzcPath %></Dzcpath>
        <SId><%= sid %></SId>
    <% } %>

    <asp:Repeater id="Image_Item" runat="Server">
        <ItemTemplate>
            <Image id="<%# Container.ItemIndex %>">
                <FileName></FileName>
                <x></x>
                <y></y>
                <Width></Width>
                <Height></Height>
                <ZOrder></ZOrder>
                <Tag><%# DataBinder.Eval(Container.DataItem,"Tags") %></Tag>

                <AdditionalData>
                    <Title><%# DataBinder.Eval(Container.DataItem,"Title") %></Title>
                    <Condition1><%# condition1 %></Condition1>
                    <Condition2></Condition2>
                    <UId><%# DataBinder.Eval(Container.DataItem,"UId") %></UId>
                    <Date><%# DataBinder.Eval(Container.DataItem,"Date") %></Date>
                    <Owner><%# DataBinder.Eval(Container.DataItem,"Owner") %></Owner>
                    <Thumbnail><%# DataBinder.Eval(Container.DataItem, "Thumbnail")%></Thumbnail>
                    <Share><%# DataBinder.Eval(Container.DataItem, "IsShare")%></Share>
                </AdditionalData>
            </Image>
        </ItemTemplate>
    </asp:Repeater>

<% } %>

</Metadata>
