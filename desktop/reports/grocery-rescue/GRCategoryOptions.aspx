<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="grcategoryoptions.aspx.cs" Inherits="desktop_reports_grocery_rescue_GRCategoryOptions" %>

<%@ Register Src="~/NumericKeypad.ascx" TagPrefix="uc1" TagName="NumericKeypad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    &nbsp;Grocery Rescue
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <h1>Grocery Rescue Report Options</h1>
    <div id="Title">
       <asp:Label ID="lblQuest" runat="server" Text="Enter the UFB product ID for each category"></asp:Label>
</div>

<style>
   #Title {text-align:center; margin: 0px auto;}
</style>
    
    <br />
    <div id="InputForm">


        <table align="center" class="auto-style1" style="width: 30%">
            <tr>
                <td><asp:Label ID="lblBakery" runat="server" Text="Bakery:"></asp:Label>
                <asp:TextBox ID="txtBakery" runat="server"  Width="166px"></asp:TextBox></td>
                <td><asp:Label ID="lblDairy" runat="server" Text="Dairy:"></asp:Label>
                <asp:TextBox ID="txtDairy" runat="server" Width="166px"></asp:TextBox></td>
                <td><asp:Label ID="lblProduce" runat="server" Text="Produce:"></asp:Label>
                <asp:TextBox ID="txtProduce" runat="server" Width="166px"></asp:TextBox></td>
                <td><asp:Label ID="lblDeli" runat="server" Text="Deli:"></asp:Label>
                <asp:TextBox ID="txtDeli" runat="server" Width="166px"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblMeat" runat="server" Text="Meat:"></asp:Label>
                <asp:TextBox ID="txtMeat" runat="server" Width="166px"></asp:TextBox></td>
                <td><asp:Label ID="lblFrozen" runat="server" Text="Frozen:"></asp:Label>
                <asp:TextBox ID="txtFrozen" runat="server" Width="166px"></asp:TextBox></td>
                <td><asp:Label ID="lblDry" runat="server" Text="Dry Grocery:"></asp:Label>
                <asp:TextBox ID="txtDry" runat="server" Width="166px"></asp:TextBox></td>
                <td><asp:Label ID="lblNonFood" runat="server" Text="Non-Food:"></asp:Label>
                <asp:TextBox ID="txtNonFood" runat="server" Width="166px"></asp:TextBox></td>
            </tr>
        </table>
  
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" />
    </div>
    <style>
   #InputForm {text-align:center; margin: 0px auto;}
</style>
    <br />
    
    <br />
    <div id="Button">
    <asp:Button ID="btnBack" CssClass="cancel" runat="server" Text="Back"  OnClientClick="JavaScript: window.history.go(-1); return false;" />
         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    <asp:Button ID="btnNext" runat="server" CssClass="submit" Text="Next" OnClick="btnNext_Click" />
     </div>
    <style>
        #Button{ text-align:center; margin: 0px auto;}
        .auto-style1
        {
            width: 100%;
        }
    </style>
</asp:Content>



