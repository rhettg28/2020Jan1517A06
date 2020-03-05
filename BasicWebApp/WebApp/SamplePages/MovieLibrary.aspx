<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieLibrary.aspx.cs" Inherits="WebApp.SamplePages.MovieLibrary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Movie Library</h1>
    <div class="row">
        <div class="col-md-6">
            <fieldset class="form-horizontal">
                <legend>Fill out the form below to add information on the movie for your movie</legend>
                <asp:Label ID="Label1" runat="server" Text="Title"
                     AssociatedControlID="TitleName"></asp:Label>
                <asp:TextBox ID="TitleName" runat="server"></asp:TextBox>

                <asp:Label ID="Label2" runat="server" Text="Year"
                     AssociatedControlID="YearMovie"></asp:Label>
                <asp:TextBox ID="YearMovie" runat="server"></asp:TextBox>

                <asp:Label ID="Label4" runat="server" Text="Media"
                     AssociatedControlID="MediaList"></asp:Label>
                <asp:CheckBoxList ID="MediaList" runat="server">
                    <asp:ListItem Value="1">DVD</asp:ListItem>
                    <asp:ListItem Value="2">VHS</asp:ListItem>
                    <asp:ListItem Value="3">File</asp:ListItem>
                </asp:CheckBoxList>

                <asp:Label ID="Label3" runat="server" Text="Rating"
                     AssociatedControlID="Rating"></asp:Label>
                <asp:RadioButtonList ID="Rating" runat="server">
                    <asp:ListItem Value="1">General (G)</asp:ListItem>
                    <asp:ListItem Value="2">Parental Guidance (PG)</asp:ListItem>
                    <asp:ListItem Value="3">14A</asp:ListItem>
                    <asp:ListItem Value="4">18A</asp:ListItem>
                    <asp:ListItem Value="5">Restricted (R)</asp:ListItem>
                </asp:RadioButtonList>

                <asp:Label ID="Label5" runat="server" Text="Review (1-5 Stars)"
                     AssociatedControlID="Review"></asp:Label>
                <asp:DropDownList ID="Review" runat="server">
                    <asp:ListItem Value="1">1 Star</asp:ListItem>
                    <asp:ListItem Value="2">2 Star</asp:ListItem>
                    <asp:ListItem Value="3">3 Star</asp:ListItem>
                    <asp:ListItem Value="4">4 Star</asp:ListItem>
                    <asp:ListItem Value="5">5 Star</asp:ListItem>
                </asp:DropDownList>

            </fieldset>
        </div>
        <div class="col-md-6">
            <asp:Button ID="Submit" runat="server" Text="Add to Library" 
                 CssClass="btn btn-success" OnClick="Submit_Click"/>
            <br /><br />
            <asp:Label ID="MessageLabel" runat="server"></asp:Label>
        </div>
    </div>
    <script src="../Scripts/bootwrap-freecode.js"></script>
</asp:Content>
