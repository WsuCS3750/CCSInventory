<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    <img src="../images/gear.gif" />Advanced Settings
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <ul class="vertical-list">
        <li><a href="../container" class="button text-left"><img src="../images/icon-container.png" />Containers</a></li>
        <li><a href="../donor" class="button text-left"><img src="../images/icon-person.png" />Donors</a></li>
        <li><a href="../donor-type" class="button text-left"><img src="../images/icon-person.png" />Donor Types</a></li>
        <li><a href="../incoming-food" class="button text-left"><img src="../images/icon-arrow.png" /> Incoming Food</a></li>
        <li><a href="../food-type" class="button text-left"><img src="../images/icon-category.png" />Food Categories</a></li>
        <li><a href="../usda-type" class="button text-left"><img src="../images/icon-category.png" />USDA Categories</a></li>
        <li><a href="../agency" class="button text-left"><img src="../images/icon-category.png" />Agencies</a></li>     
        <li><a href="../audit" class="button text-left"><img src="../images/icon-clipboard.png" />Audit</a></li>     
    </ul>
</asp:Content>

