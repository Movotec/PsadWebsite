<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="PsadWebsite.Search" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Search</h1>
        <div class="row">
            <div class="col-lg-6">
                <div class="input-group">
                    <input type="text" class="form-control movo-bg-grey" placeholder="Search for...">
                    <span class="input-group-btn">
                        <button class="btn btn-default movo-bg-orange" type="button">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>
                    </span>
                </div><!-- /input-group -->
            </div>
            

        </div>


        <div class="row">
            <div class="col-lg-6">
                Gender
            </div>
            <div class="col-lg-6">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="radio-list-vertical">
                    <asp:ListItem Selected="True" Value="Male">Male</asp:ListItem>
                    <asp:ListItem Value="Femlae">Female</asp:ListItem>
                </asp:RadioButtonList>
                            
                       
            </div>
        </div>


    </div>


</asp:Content>
