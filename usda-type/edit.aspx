<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="USDACategoryEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    <img src="../images/icon-category.png" />Edit USDA Category
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSave">
        <h1>Edit USDA Category <!--<asp:Label runat="server" ID="lblID"></asp:Label>--></h1>
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        <table class="form">
            <tr>
                <td>Name:</td>
                <td><asp:TextBox runat="server" ID="txtDescription"></asp:TextBox></td>
            </tr>
            <tr>
                <td>USDA Number:</td>
                <td><asp:TextBox runat="server" ID="txtUSDANumber"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Case Per Weight:</td>
                <td><asp:TextBox runat="server" type="number" step="any" min="0" ID="txtCaseWeight"></asp:TextBox></td>
            </tr>
        </table>

        <table class="buttonGroup cols-3">
            <tr>
                <td>
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="cancel" onclientclick="JavaScript: window.history.go(-1); return false;">Cancel</asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="delete" OnClick="btnDelete_Click" />
                </td>
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="submit confirm" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>