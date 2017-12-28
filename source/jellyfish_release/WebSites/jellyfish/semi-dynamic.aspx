<?xml version="1.0" encoding="utf-8" ?>
<%@ Page ContentType="text/xml" Language="C#" AutoEventWireup="true" CodeFile="semi-dynamic.aspx.cs" Inherits="semi_dynamic" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<Metadata version="1">

    <%= ExecStatus %>

<% if (!IsPageError) { %>


    <asp:Repeater id="Collection_Item" runat="Server">
        <ItemTemplate>
            <Collection id="<%# Container.ItemIndex %>">
                <CId><%# DataBinder.Eval(Container.DataItem, "CId")%></CId>
                <CUrl><%# DataBinder.Eval(Container.DataItem, "CUrl")%></CUrl>
                <CTitle><%# DataBinder.Eval(Container.DataItem, "CTitle")%></CTitle>
                <LId><%# DataBinder.Eval(Container.DataItem, "LId")%></LId>
                <LUrl><%# DataBinder.Eval(Container.DataItem, "LUrl")%></LUrl>
                <LTitle><%# DataBinder.Eval(Container.DataItem, "LTitle")%></LTitle>
            </Collection>
        </ItemTemplate>
    </asp:Repeater>


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


    <asp:Repeater id="Layout_Item" runat="Server">
        <ItemTemplate>
            <Layout>
                <LId><%# DataBinder.Eval(Container.DataItem, "LId")%></LId>
                <LUrl><%# DataBinder.Eval(Container.DataItem, "LUrl")%></LUrl>
                <LTitle><%# DataBinder.Eval(Container.DataItem, "LTitle")%></LTitle>
            </Layout>
        </ItemTemplate>
    </asp:Repeater>

<% } %>

</Metadata>
