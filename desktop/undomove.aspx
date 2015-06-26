<%@ Page Language="C#" MasterPageFile="~/desktop.master" AutoEventWireup="true" CodeFile="undomove.aspx.cs" Inherits="desktop_undomove" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" runat="Server">
    <img src="../images/icon-person.png" />
    <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" runat="Server">
    <asp:Label ID="lblError" runat="server" Text="" Visible="false" CssClass="error"></asp:Label>
    <div class="notify">
     <asp:Label ID="lblResponse" runat="server" Text=""  CssClass="notification" Visible="False"></asp:Label>
        </div>

        <div runat="server">

        <ul class="vertical-list">
            
        <li>
        <div class="left">
            <asp:Label ID="lblBinNumber" runat="server" Text=""></asp:Label>
        </div>
        </li>

            <br />
            <br />

        <li>
        <div class="left">
            <asp:Label ID="lblWeight" runat="server" Text=""></asp:Label>
        </div>
        </li>

            <br />
            <br />

        <li>
        <div class="left">
            <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
        </div>
        </li>

            <br />
            <br />

        <li>
        <div class="left">
            <asp:Label ID="lblLocation" runat="server" Text="Select Room:" Visible="true"></asp:Label>
        </div>
            <asp:DropDownList ID="ddlLocation" runat="server"
                        DataTextField="RoomName" DataValueField="LocationID" CssClass="ddlLocationCat" Visible="true"></asp:DropDownList>
        </li>
        <br />
        </ul>

        <table class="buttonGroup cols-2" runat="server">
            <tr>
                <td>
                    <a href="managedata.aspx?do=FO" class="cancel">Cancel</a>
                </td>
               <td>
                    <asp:LinkButton ID="btnUndoMove" runat="server" Text="Save" CssClass="submit" OnClick="btnUndoMoveSave_Click" 
                        OnClientClick="return confirm('Are you sure you would like to undo this food out transaction?');"/>
                </td>
            </tr>
        </table>
     </div>

</asp:Content>
