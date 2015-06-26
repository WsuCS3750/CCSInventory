<%@ Page Language="C#" MasterPageFile="~/desktop.master" AutoEventWireup="true" CodeFile="managedata.aspx.cs" Inherits="desktop_managedata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script src="../js/jquery-ui-1.10.0.custom.min.js" type="text/javascript"></script>
    <script src="FoodService.ashx" type="text/javascript"></script>
    <script src="USDAService.ashx" type="text/javascript"></script>
    <link href="../css/ui-lightness/jquery-ui-1.10.0.custom.min.css" rel="stylesheet" />
    
    <script>
        $(document).ready(function () {
            $('#<%= txtSearchFoodIn.ClientID %>').autocomplete({ source: foodNames });
            $('#<%= txtSearchUSDAFoodIn.ClientID %>').autocomplete({ source: USDANames });
            $('#<%= txtSearchFoodOut.ClientID %>').autocomplete({ source: foodNames });
            $('#<%= txtSearchUSDAFoodOut.ClientID %>').autocomplete({ source: USDANames });
            $('#<%= txtSearchHistory.ClientID %>').autocomplete({ source: foodNames });
            $('#<%= txtSearchUSDAHistory.ClientID %>').autocomplete({ source: USDANames });

        });
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" runat="Server">
    <img src="../images/icon-person.png" />Manage Data
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" runat="Server">
    
    <div id="tablechoice">
        <asp:Button ID="btnFoodIn" runat="server" Text="Food In" OnClick="btnFoodIn_Click"  ControlStyle-CssClass="btnTablechoice"/>
        <asp:Label ID="lblBtnFoodIn" runat="server" Text="Food In" Visible="false" ControlStyle-CssClass="lblTablechoice"></asp:Label>
        <asp:Button ID="btnFoodOut" runat="server" Text="Food Out" OnClick="btnFoodOut_Click"  ControlStyle-CssClass="btnTablechoice"/>
        <asp:Label ID="lblBtnFoodOut" runat="server" Text="Food Out" Visible="false" ControlStyle-CssClass="lblTablechoice"></asp:Label>
        <asp:Button ID="btnHistory" runat="server" Text="History" Visible="false" onClick="btnHistory_Click" ControlStyle-CssClass="btnSearch" />     
        <asp:Label ID="lblBtnHistory" runat="server" Text="History" Visible="false" ControlStyle-CssClass="lblHistorychoice"></asp:Label>  
    </div>

    <div class="searchNotify">
        <asp:Label ID="lblResponse" runat="server" Text=""  CssClass="notification" Visible="False"></asp:Label>
    </div>
   
    <asp:Panel id="searchFoodInDiv" runat="server" Visible="false" >
        <div class="left">
            <asp:Label ID="lblSearchFoodIn" runat="server" Text="Search Food by:"></asp:Label>
            <asp:DropDownList ID="ddlSearchFInBy" runat="server" CssClass="ddlSearchBy">
                <asp:ListItem>Category</asp:ListItem>
                <asp:ListItem>Date</asp:ListItem>
                <asp:ListItem>Donor</asp:ListItem>
                <asp:ListItem>Weight</asp:ListItem>
            </asp:DropDownList>
        </div>
            <asp:TextBox ID="txtSearchFoodIn" runat="server" ControlStyle-CssClass="searchBox"></asp:TextBox>
            <asp:TextBox ID="txtSearchUSDAFoodIn" runat="server" ControlStyle-CssClass="searchBox" Visible="false"></asp:TextBox>
        <div class="searchPanel2">
            <asp:Button ID="btnSearchFoodIn" runat="server" Text="Search" ControlStyle-CssClass="btnSearch" OnClick="btnSearchFoodIn_Click"/>
            <asp:Button ID="btnResetFoodIn" runat="server" Text="Reset" ControlStyle-CssClass="btnSearch" OnClick="btnResetFoodIn_Click"/>
            <span class="chkUSDA">
                <asp:CheckBox ID="chkUSDAin" runat="server" Text=" Is USDA?" ControlStyle-CssClass="checkBox" OnCheckedChanged="toggleUSDAin" AutoPostBack="true"/>
            </span>
        </div>
    </asp:Panel>

    
    <asp:Panel id="searchFoodOutDiv" runat="server" Visible="false" >
        <div class="left">
            <asp:Label ID="lblSearchFoodOut" runat="server" Text="Search Food by:"></asp:Label>
             <asp:DropDownList ID="ddlSearchFOutBy" runat="server" CssClass="ddlSearchBy">
                <asp:ListItem>Category</asp:ListItem>
                <asp:ListItem>Bin Number</asp:ListItem>
                <asp:ListItem>Date</asp:ListItem>
                <asp:ListItem>Count</asp:ListItem>
                <asp:ListItem>Weight</asp:ListItem>
            </asp:DropDownList>
        </div>
            <asp:TextBox ID="txtSearchFoodOut" runat="server" ControlStyle-CssClass="searchBox" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtSearchUSDAFoodOut" runat="server" ControlStyle-CssClass="searchBox" Visible="false"></asp:TextBox>
        <div class="searchPanel2">
            <asp:Button ID="btnSearchFoodOut" runat="server" Text="Search" ControlStyle-CssClass="btnSearch" OnClick="btnSearchFoodOut_Click"/>
            <asp:Button ID="btnResetFoodOut" runat="server" Text="Reset" ControlStyle-CssClass="btnSearch" OnClick="btnResetFoodOut_Click"/>
            <span class="chkUSDA">
                <asp:CheckBox ID="chkUSDA" runat="server" Text=" Is USDA?" ControlStyle-CssClass="checkBox" OnCheckedChanged="toggleUSDAout" AutoPostBack="true"/>
            </span>

         </div>        
     </asp:Panel>    

    <asp:Panel id="searchHistoryDiv" runat="server" Visible="false">
        <div class="left">
            <asp:Label ID="lblSearchHistory" runat="server" Text="Search History by:"></asp:Label>
             <asp:DropDownList ID="ddlSearchHistoryBy" runat="server" CssClass="ddlSearchBy">                
                <asp:ListItem>Bin Number</asp:ListItem>
                <asp:ListItem>Category</asp:ListItem>
                <asp:ListItem>Date Created</asp:ListItem>
                <asp:ListItem>Date Out</asp:ListItem>
                <asp:ListItem>Distribution Type</asp:ListItem>
            </asp:DropDownList>
        </div>
            <asp:TextBox ID="txtSearchHistory" runat="server" ControlStyle-CssClass="searchBox" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtSearchUSDAHistory" runat="server" ControlStyle-CssClass="searchBox" Visible="false"></asp:TextBox>
        <div class="searchPanel2">
            <asp:Button ID="btnSearchHistory" runat="server" Text="Search" ControlStyle-CssClass="btnSearch" OnClick="btnSearchHistory_Click"/>
            <asp:Button ID="btnResetHistory" runat="server" Text="Reset" ControlStyle-CssClass="btnSearch" OnClick="btnResetHistory_Click"/>
            <span class="chkUSDAhistory">
                <asp:CheckBox ID="chkUSDAhistory" runat="server" Text=" Is USDA?" ControlStyle-CssClass="checkBox" OnCheckedChanged="toggleUSDAhistory" AutoPostBack="true"/>
            </span>

         </div>        
     </asp:Panel> 
    

    <div id="foodInDiv" runat="server">
         <asp:GridView ID="grdFoodInData" runat="server"
            AutoGenerateColumns="false" DataKeyNames="FoodInID"
            AllowPaging="True" PageSize="20"
            AllowSorting="True"
            OnSorting="FoodIn_Sorting"
            OnRowCommand="grdFoodInData_RowCommand"
            OnRowDeleting="grdFoodInData_RowDeleting"
            OnPageIndexChanging="grdFoodInData_PageIndexChanging"
            HeaderStyle-HorizontalAlign="Left"
            CssClass="dataTable"
            CellSpacing="8" >
        <Columns>

             <asp:TemplateField HeaderText="Food In ID">
                <ItemTemplate>
                    <asp:Label ID="lblFoodInID" runat="server" Text='<%# Bind("FoodInID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

             <asp:TemplateField HeaderText="Category" SortExpression="Category">
                <ItemTemplate>
                    <asp:Label ID="lblFoodCatType" runat="server" Text='<%# Bind("Types") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Weight" SortExpression="Weight">
                <ItemTemplate>
                    <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("Weight") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Source" SortExpression="Source">
                <ItemTemplate>
                    <asp:Label ID="lblSource" runat="server" Text='<%# Bind("Source") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Time Stamp" SortExpression="Timestamp">
                <ItemTemplate>
                    <asp:Label ID="lblTimeStamp" runat="server" Text='<%# Bind("TimeStamp") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:ButtonField ButtonType="button" CommandName="editData" runat="server" Text="Edit"  ControlStyle-CssClass="submit"></asp:ButtonField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnFoodInDelete" Text="Delete"
                            CommandName="delete" OnClientClick="return confirm('Are you sure you would like to remove this data?');"
                            CommandArgument='<%# Container.DataItemIndex %>'  ControlStyle-CssClass="delete"/>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
        </asp:GridView>
        <asp:Label ID="lblFoodInError" runat="server" Text=""></asp:Label>

    </div>

    <div id="foodOutDiv" runat="server">   
         <asp:GridView ID="grdFoodOutData" runat="server"
            AutoGenerateColumns="false" DataKeyNames="DistributionID"
            AllowPaging="True" PageSize="20"
            AllowSorting="True"
            OnSorting="FoodOut_Sorting"
            OnRowCommand="grdFoodOutData_RowCommand"
            OnRowDeleting="grdFoodOutData_RowDeleting"
            OnPageIndexChanging="grdFoodOutData_PageIndexChanging"
            HeaderStyle-HorizontalAlign="Left"
            CssClass="dataTable"
            CellSpacing="8" >
        <Columns>

            <asp:TemplateField HeaderText="Distribution ID">
                <ItemTemplate>
                    <asp:Label ID="lblDistributionID" runat="server" Text='<%# Bind("DistributionID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Bin Number" SortExpression="BinNumber">
                <ItemTemplate>
                    <asp:Label ID="lblBinNumber" runat="server" Text='<%# Bind("BinNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

             <asp:TemplateField HeaderText="Category" SortExpression="Category">
                <ItemTemplate>
                    <asp:Label ID="lblFoodCatType" runat="server" Text='<%# Bind("Types") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Weight" SortExpression="Weight">
                <ItemTemplate>
                    <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("Weight") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Count" SortExpression="Count">
                <ItemTemplate>
                    <asp:Label ID="lblCount" runat="server" Text='<%# Bind("Count") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Time Stamp" SortExpression="Timestamp">
                <ItemTemplate>
                    <asp:Label ID="lblTimestamp" runat="server" Text='<%# Bind("TimeStamp") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:ButtonField ButtonType="button" CommandName="editData" runat="server" Text="Edit"  ControlStyle-CssClass="submit"></asp:ButtonField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnFoodOutDelete" Text="Delete"
                            CommandName="delete" OnClientClick="return confirm('Are you sure you would like to remove this data?');"
                            CommandArgument='<%# Container.DataItemIndex %>'  ControlStyle-CssClass="delete"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:ButtonField ButtonType="button" CommandName="undo" runat="server" Text="Undo" ControlStyle-CssClass="undo"></asp:ButtonField>

        </Columns>
        </asp:GridView>
        <asp:Label ID="lblFoodOutError" runat="server" Text=""></asp:Label>

    </div>

    <div id="historyDiv" runat="server">   
         <asp:GridView ID="grdHistoryData" runat="server"
            AutoGenerateColumns="false" DataKeyNames="BinNumber"
            AllowPaging="True" PageSize="20"
            AllowSorting="True"
            OnSorting="History_Sorting"
            OnRowCommand="grdHistoryData_RowCommand"
            OnRowDeleting="grdHistoryData_RowDeleting"
            OnPageIndexChanging="grdHistoryData_PageIndexChanging"
            HeaderStyle-HorizontalAlign="Left"
            CssClass="dataTable"
            CellSpacing="8" >
        <Columns>

            <asp:TemplateField HeaderText="Distribution ID">
                <ItemTemplate>
                    <asp:Label ID="lblDistributionID" runat="server" Text='<%# Bind("DistributionID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Bin Number" SortExpression="BinNumber">
                <ItemTemplate>
                    <asp:Label ID="lblBinNumber" runat="server" Text='<%# Bind("BinNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

             <asp:TemplateField HeaderText="Category" SortExpression="Category">
                <ItemTemplate>
                    <asp:Label ID="lblFoodCatType" runat="server" Text='<%# Bind("Types") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Distribution Type" SortExpression="Distribution Type">
                <ItemTemplate>
                    <asp:Label ID="lblDistributionType" runat="server" Text='<%# Bind("DistributionType") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date Created" SortExpression="Date Created">
                <ItemTemplate>
                    <asp:Label ID="lblDateCreated" runat="server" Text='<%# Bind("DateCreated") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Time Stamp" SortExpression="Timestamp">
                <ItemTemplate>
                    <asp:Label ID="lblTimestamp" runat="server" Text='<%# Bind("TimeStamp") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:ButtonField ButtonType="button" CommandName="editData" runat="server" Text="Edit"  ControlStyle-CssClass="submit"></asp:ButtonField>

            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnHistoryDelete" Text="Delete"
                            CommandName="delete" OnClientClick="return confirm('Are you sure you would like to remove this data?');"
                            CommandArgument='<%# Container.DataItemIndex %>'  ControlStyle-CssClass="delete"/>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
        </asp:GridView>
        <asp:Label ID="lblHistoryError" runat="server" Text=""></asp:Label>

    </div>
</asp:Content>

