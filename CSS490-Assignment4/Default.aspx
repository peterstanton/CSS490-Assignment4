<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CSS490_Assignment4._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Peter Stanton&#39;s CSS490 Assignment4!</h1>
        <p class="lead">This is a simple web application that reads data from an Azure blob, copies it to another blob for archival, and uploads the data to a NoSQL table for querying.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                Bear in mind that you MUST click the Load Data button first!</p>
            <p>
                &nbsp;</p>
            <p style="margin-left: 120px">
                <asp:Label ID="Label2" runat="server" Text="First Name"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Last Name"></asp:Label>
            </p>
            <p>
                <asp:Button ID="loadButton" runat="server" OnClick="loadButton_Click" Text="Load Data" />
                <asp:TextBox ID="firstNameBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="lastNameBox" runat="server"></asp:TextBox>
                <asp:Button ID="queryButton" runat="server" Text="Query" OnClick="queryButton_Click" />
                <asp:TextBox ID="outputBox" runat="server" Height="300px" Width="600px" TextMode="MultiLine" Wrap="False"></asp:TextBox>
            </p>
            <p>
                &nbsp;</p>
        </div>
        <div class="col-md-4">
            <p>
                &nbsp;</p>
        </div>
    </div>

</asp:Content>
