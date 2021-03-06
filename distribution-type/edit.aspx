﻿<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="EditDistributionType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    Edit Distribution Type
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>

    <table class="form">
        <tr>
            <td>Distribution Type:</td>
            <td><asp:TextBox ID="txtDistributionType" runat="server"></asp:TextBox></td> 
        </tr>
    </table>

    <table class="buttonGroup cols-2">
        <tr>
            <td><asp:LinkButton ID="btnRemove" runat="server" CssClass="delete" OnClick="btnRemove_Click">Remove</asp:LinkButton></td>
            <td>
                <asp:LinkButton ID="btnSave" runat="server" CssClass="submit confirm" OnClick="btnSave_Click">Save</asp:LinkButton>
            </td>
        </tr>
    </table>

</asp:Content>

