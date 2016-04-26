<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="PsadWebsite.Search" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Search</h1>
        <div class="row">
            <div class="col-lg-6">
                <div class="input-group">
                    <asp:TextBox ID="TextBoxSearch" CssClass="form-control movo-bg-grey" runat="server" placeholder="Search for..."></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:LinkButton ID="LinkButtonSearch" runat="server" CssClass="btn btn-primary" OnClick="ButtonSearch_Click">
                            <span class="glyphicon glyphicon-search"></span>
                        </asp:LinkButton>                      
                    </span>
                </div><!-- /input-group -->
            </div>
            

        </div>


        <div class="row">
            <div class="col-lg-12">
                <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#advancedSearch" aria-expanded="false" aria-controls="advancedSearch">
                    Advanced Search
                </button>
            </div>
            

            <div class="collapse" id="advancedSearch">
                <div class="col-lg-6">
                    Groups
                </div>
                <div class="col-lg-6">
                    <!-- See Enumerators.cs in APP_CODE for context -->
                    <!-- Grouping radio list (possibly make checklist)-->
                    <asp:RadioButtonList ID="RadioButtonListGroup" runat="server" CssClass="radio-list-vertical">
                        <asp:ListItem Selected="True" Value="-1"> None specified</asp:ListItem>
                        <asp:ListItem Value="0">Patients</asp:ListItem>
                        <asp:ListItem Value="1">Operators</asp:ListItem>
                        <asp:ListItem Value="2">Organisations</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-lg-6">
                    Gender
                </div>
                <div class="col-lg-6">
                

                    <!-- Gender radio list -->
                    <asp:RadioButtonList ID="RadioButtonListGender" runat="server" CssClass="radio-list-vertical">
                        <asp:ListItem Selected="True" Value="-1"> None specified</asp:ListItem>
                        <asp:ListItem Value="0">Male</asp:ListItem>
                        <asp:ListItem Value="0">Female</asp:ListItem>
                    </asp:RadioButtonList>
                            
                       
                </div>            

            </div>
            
        </div>


    </div>

    <div class="jumbotron">
        <div class="row">
            <asp:Panel ID="PanelPatients" runat="server" CssClass="well" Visible="false">
                <h1>Patients</h1>
                <div class="row">
                    <!-- Could programatically make bootstrap collumn size based on number of values -->
                    <asp:Repeater ID="RepeaterPatients" runat="server">
                        <ItemTemplate>
                                <div class="col-lg-4">
                                    <div class="row">
                                        <asp:Label runat="server" ID="name" CssClass="name col-lg-3" Text='<%# Eval("name").ToString() %>' ></asp:Label>
                                        <asp:Label runat="server" ID="gender" CssClass="gender col-lg-3" Text='<%# Eval("gender") %>'></asp:Label>
                                        <asp:Label runat="server" ID="year" CssClass="year col-lg-3"  Text='<%# Eval("year") %>'></asp:Label>
                                        <asp:Label runat="server" ID="status" CssClass="status col-lg-3"  Text='<%# Eval("status") %>'></asp:Label>
                                    </div>
                                </div>
                        
                     </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            

            <asp:Repeater ID="RepeaterOperators" runat="server">
                <ItemTemplate>
                    <div>
                        <asp:Label runat="server" CssClass="col-lg-4" Text='<%# Eval("name").ToString() %>'></asp:Label>
                    </div>
                </ItemTemplate>

            </asp:Repeater>

            <asp:Repeater ID="RepeaterOrganisations" runat="server">
                <ItemTemplate>
                    <div>
                        <asp:Label runat="server"  CssClass="col-lg-4" Text='<%# Eval("name").ToString() %>'></asp:Label>
                    </div>
                </ItemTemplate>

            </asp:Repeater>
        </div>
    </div>

</asp:Content>
