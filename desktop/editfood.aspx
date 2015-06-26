<%@ Page Language="C#" MasterPageFile="~/desktop.master" AutoEventWireup="true" CodeFile="editfood.aspx.cs" Inherits="desktop_editfood" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" runat="Server">
    <img src="../images/icon-person.png" />
    <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" runat="Server">
    <div class="notify">
     <asp:Label ID="lblResponse" runat="server" Text=""  CssClass="notification" Visible="False"></asp:Label>
        </div>
    <div id="editFoodInDiv" runat ="server">  
         
            <ul class="vertical-list">
                <li>
                    <div class="left">
                    <asp:Label ID="lblEditFoodInID" runat="server" Text="Food In ID"></asp:Label>
                    </div>
                    <asp:TextBox ID="txtEditFoodInID" runat="server" Enabled="false" Text="" CssClass="txtEditFood"></asp:TextBox>
                </li>
                <li>
                    <div class="left">
                        <asp:Label ID="lblFoodInCatType" runat="server" Text="Food Category" Visible="false"></asp:Label>
                    </div>
                    <asp:DropDownList ID="ddlFoodInCategoryType" runat="server"
                        DataTextField="CategoryType" DataValueField="FoodCategoryID" CssClass="ddlFoodOutCat" Visible="false">
                    </asp:DropDownList>

                    <div class="left">
                        <asp:Label ID="lblFoodInUSDA" runat="server" Text="USDA Category" Visible="false"></asp:Label>
                    </div>
                    <asp:DropDownList ID="ddlFoodInUSDA" runat="server"
                        DataTextField="Description" DataValueField="USDAID" CssClass="ddlFoodOutUSDA" Visible="false">
                    </asp:DropDownList>

                    <asp:CheckBox ID="chkUSDAin" runat="server" Text=" Is USDA?" ControlStyle-CssClass="editCheckBox" OnCheckedChanged="toggleEditUSDAin" AutoPostBack="true" />
                </li>
                <li>
                    <div class ="left">
                    <asp:Label ID="lblFoodInSource" runat="server" Text="Source"></asp:Label>
                    </div>
                    <asp:DropDownList ID="ddlFoodInSource" runat="server" DataTextField="Source" DataValueField="FoodSourceID" CssClass="ddlSource">
                    </asp:DropDownList>
                </li>
                <li>
                    <div class ="left">
                    <asp:Label ID="lblFoodInWeight" runat="server" Text="Weight" ></asp:Label>
                    </div>
                    <asp:TextBox ID="txtFoodInWeight" runat="server" Text="" CssClass="weight"></asp:TextBox>
                </li>
                <li>
                    <div class ="left">
                    <asp:Label ID="lblFoodInTime" runat="server" Text="Time Stamp"></asp:Label>
                    </div>
                    <asp:TextBox ID="txtFoodInTime" runat="server" Text="" CssClass="time"></asp:TextBox>
                </li>
            </ul>

        <table class="buttonGroup cols-2">
            <tr>
                <td>
                    <a href="managedata.aspx?do=FI" class="cancel">Cancel</a>
                </td>
                <td>
                    <asp:LinkButton ID="btnEditSave" runat="server" Text="Save" CssClass="submit" OnClick="btnEditFoodInSave_Click" 
                        OnClientClick="return confirm('Are you sure you would like to edit this data?');"/>
                </td>
            </tr>
        </table>

        </div>

        <div id="editFoodOutDiv" runat="server">

            
        <ul class="vertical-list">
            
        <li>
            <div class="left">
            <asp:Label ID="lblFoodOutDistID" runat="server" Text="Distribution ID"></asp:Label>
            </div>
        <asp:TextBox ID="txtFoodOutDistID" runat="server" Text="" CssClass="txtEditDist" Enabled="false"></asp:TextBox>
        </li>
        <li>
        <div class="left">
            <asp:Label ID="lblFoodOutCatType" runat="server" Text="Food Category" Visible="false"></asp:Label>
                </div>
        <asp:DropDownList ID="ddlFoodOutCategoryType" runat="server"
                        DataTextField="CategoryType" DataValueField="FoodCategoryID" CssClass="ddlFoodOutCat" Visible="false"></asp:DropDownList>
   
        <div class="left">
            <asp:Label ID="lblFoodOutUSDA" runat="server" Text="USDA Category" Visible="false"></asp:Label>
                </div>
        <asp:DropDownList ID="ddlFoodOutUSDA" runat="server"
                        DataTextField="Description" DataValueField="USDAID" CssClass="ddlFoodOutUSDA" Visible="false"></asp:DropDownList>

        <asp:CheckBox ID="chkUSDA" runat="server" Text="  Is USDA?" ControlStyle-CssClass="editCheckBox" OnCheckedChanged="toggleEditUSDAout" AutoPostBack="true"/>
        </li>
        <li>
            <div class="left">
            <asp:Label ID="lblFoodOutWeight" runat="server" Text="Weight"></asp:Label>
                 </div>
        <asp:TextBox ID="txtFoodOutWeight" runat="server" Text="" CssClass="txtEditWeight"></asp:TextBox>

        </li>
        <li>
            <div class="left">
            <asp:Label ID="lblFoodOutCount" runat="server" Text="Count"></asp:Label>
                 </div>
        <asp:TextBox ID="txtFoodOutCount" runat="server" Text="" CssClass="txtEditCount"></asp:TextBox>

        </li>
        <li>
        <div class="left">
            <asp:Label ID="lblFoodOutTime" runat="server" Text="Time Stamp"></asp:Label>
                 </div>
        <asp:TextBox ID="txtFoodOutTime" runat="server" Text="" CssClass="txtEditTime"></asp:TextBox>

        </li>
        </ul>    
            

        <table class="buttonGroup cols-2" runat="server">
            <tr>
                <td>
                    <a href="managedata.aspx?do=FO" class="cancel">Cancel</a>
                </td>
               <td>
                    <asp:LinkButton ID="btnEditFoodOut" runat="server" Text="Save" CssClass="submit" OnClick="btnEditFoodOutSave_Click" 
                        OnClientClick="return confirm('Are you sure you would like to edit this data?');"/>
                </td>
            </tr>
        </table>
            </div>

    <div id="editHistoryDiv" runat="server">

            
        <ul class="vertical-list">
            
        <li>
            <div class="left">
            <asp:Label ID="lblHistoryDistID" runat="server" Text="Distribution ID"></asp:Label>
            </div>
        <asp:TextBox ID="txtHistoryDistID" runat="server" Text="" CssClass="txtEditDist" Enabled="false"></asp:TextBox>
        </li>
        <li>
        <div class="left">
            <asp:Label ID="lblHistoryCatType" runat="server" Text="Food Category" Visible="false"></asp:Label>
                </div>
        <asp:DropDownList ID="ddlHistoryCategoryType" runat="server"
                        DataTextField="CategoryType" DataValueField="FoodCategoryID" CssClass="ddlFoodOutCat" Visible="false"></asp:DropDownList>
   
        <div class="left">
            <asp:Label ID="lblHistoryUSDA" runat="server" Text="USDA Category" Visible="false"></asp:Label>
                </div>
        <asp:DropDownList ID="ddlHistoryUSDA" runat="server"
                        DataTextField="Description" DataValueField="USDAID" CssClass="ddlFoodOutUSDA" Visible="false"></asp:DropDownList>

        <asp:CheckBox ID="chkUSDAhistory" runat="server" Text="  Is USDA?" ControlStyle-CssClass="editCheckBox" OnCheckedChanged="toggleEditUSDAhistory" AutoPostBack="true"/>
        </li>
        <li>
            <div class="left">
            <asp:Label ID="lblHistoryDistributionType" runat="server" Text="Distribution Type"></asp:Label>
                 </div>
        <asp:DropDownList ID="ddlHistoryDistributionType" runat="server"
                    DataTextField="DistributionType1" DataValueField="DistributionTypeID" CssClass="ddlDistType" Visible="false"></asp:DropDownList>

        </li>
        <li>
            <div class ="left">
                <asp:Label ID="lblHistoryTimeCreated" runat="server" Text="Time Created"></asp:Label>
                    </div>
        <asp:TextBox ID="txtHistoryTimeCreated" runat="server" Text="" CssClass="txtEditTimeCreated"></asp:TextBox>
            

        </li>
        <li>
        <div class="left">
            <asp:Label ID="lblHistoryTime" runat="server" Text="Time Stamp"></asp:Label>
                 </div>
        <asp:TextBox ID="txtHistoryTime" runat="server" Text="" CssClass="txtEditTime"></asp:TextBox>

        </li>
        </ul>    
            

        <table id="Table1" class="buttonGroup cols-2" runat="server">
            <tr>
                <td>
                    <a href="managedata.aspx?do=H" class="cancel">Cancel</a>
                </td>
               <td>
                    <asp:LinkButton ID="btnEditHistory" runat="server" Text="Save" CssClass="submit" OnClick="btnEditHistorySave_Click" 
                        OnClientClick="return confirm('Are you sure you would like to edit this data?');"/>
                </td>
            </tr>
        </table>
            </div>
</asp:Content>
