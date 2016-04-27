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

        <!-- Advanced Search -->
        <div class="row">
            <div class="col-lg-12">
                <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#advancedSearch" aria-expanded="false" aria-controls="advancedSearch">
                    Advanced Search
                </button>

                <!-- This is very much a Mess, perhaps custom javascript that extends/alters Collapse.js for radio buttons specifically -->
                <%--<input type="radio" data-toggle="collapse" data-target="#advancedSearch" aria-expanded="false" aria-controls="advancedSearch" />--%>
            </div>
            
            
            <div class="collapse" id="advancedSearch">
                <div class="col-lg-6">
                    Groups
                </div>
                <div class="col-lg-6">
                    <!-- See Enumerators.cs in APP_CODE for context -->
                    <!-- Grouping radio list (possibly make checklist)-->
                    <asp:RadioButtonList ID="RadioButtonListGroup" runat="server" CssClass="radio-list-vertical">
                        <asp:ListItem Selected="True" Value="0"> None specified</asp:ListItem>
                        <asp:ListItem Value="1">Patients</asp:ListItem>
                        <asp:ListItem Value="2">Operators</asp:ListItem>
                        <asp:ListItem Value="3">Organisations</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-lg-6">
                    Gender
                </div>


                <div id="advancedGender" class="col-lg-6">
                    <asp:Panel ID="PanelGender" runat="server">
                        <%--<asp:RadioButton runat="server" GroupName="Gender" Text="Males" />
                        <asp:Label runat="server" Text="Male"></asp:Label>
                        <asp:RadioButton runat="server" GroupName="Gender" Text="1" />
                        <asp:Label runat="server" Text="Female"></asp:Label>--%>
                    </asp:Panel>

                    
                    <!-- Gender radio list -->
                    <asp:RadioButtonList ID="RadioButtonListGender" runat="server" CssClass="radio-list-vertical">
                        <asp:ListItem Selected="True" Value="0"> None specified</asp:ListItem>
                        <asp:ListItem Value="1">Male</asp:ListItem>
                        <asp:ListItem Value="2">Female</asp:ListItem>
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
            
             <asp:Panel ID="PanelOperators" runat="server" CssClass="well" Visible="false">
                <h1>Operators</h1>
                <div class="row">
                    <!-- Could programatically make bootstrap collumn size based on number of values -->
                    <asp:Repeater ID="RepeaterOperators" runat="server">
                        <ItemTemplate>
                                <div class="col-lg-4">
                                    <div class="row">
                                        <asp:Label runat="server" ID="name" CssClass="name col-lg-3" Text='<%# Eval("name").ToString() %>' ></asp:Label>
                                        <asp:Label runat="server" ID="gender" CssClass="gender col-lg-3" Text='<%# Eval("gender") %>'></asp:Label>
                                        <asp:Label runat="server" ID="status" CssClass="status col-lg-3"  Text='<%# Eval("status") %>'></asp:Label>
                                    </div>
                                </div>
                        
                     </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>

             <asp:Panel ID="PanelOrganisations" runat="server" CssClass="well" Visible="false">
                <h1>Organisations</h1>
                <div class="row">
                    <!-- Could programatically make bootstrap collumn size based on number of values -->
                    <asp:Repeater ID="RepeaterOrganisations" runat="server">
                        <ItemTemplate>
                                <div class="col-lg-4">
                                    <div class="row">
                                        <asp:Label runat="server" ID="name" CssClass="name col-lg-3" Text='<%# Eval("name").ToString() %>' ></asp:Label>
                                    </div>
                                </div>
                        
                     </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>

        </div>
    </div>

</asp:Content>
