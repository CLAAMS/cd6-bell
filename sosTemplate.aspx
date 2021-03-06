﻿<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="sosTemplate.aspx.cs" Inherits="CD6.sosTemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/secondary-nav.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <nav class="navbar navbar-default navbar-secondary" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#subnavbar" aria-expanded="false" aria-controls="subnavbar">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
            </div>
            <div id="subnavbar" class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li><a href="users.aspx">User Management</a></li>
                    <li><a href="sosTemplate.aspx">Sign Sheet Copy</a></li>
                    <li><a href="email.aspx">Email Copy</a></li>
                </ul>
            </div><!--/.nav-collapse -->
        </div><!--/.container-fluid -->
    </nav>
    <div class="row main_content">
        <div class="col-md-10 col-md-offset-1">
            <div class="row header_row"><div class="col-md-12" id="sosCopyHeader" runat="server" visible="true"><p><h1>Edit Sign Sheet</h1></p></div></div>
            <asp:Label ID="lblSOSCopy" Text="Sign Sheet Body:" runat="server" CssClass="label" />
            <asp:TextBox ID="txtSOSCopy" runat="server" TextMode="MultiLine" Rows="20" Columns="100" CssClass="form-control" ValidateRequestMode="Disabled"/>
            <div class="row"><div class="col-md-12 button_row" style="text-align:center;"><asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-default" OnClientClick="saveChanges()" OnClick="btnSubmit_Click" /></div></div>
        </div>
    </div>
</asp:Content>
