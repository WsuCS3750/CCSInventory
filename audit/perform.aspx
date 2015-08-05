﻿<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="perform.aspx.cs" Inherits="Perform" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    <img src="../images/icon-clipboard.png" />Perform Audit 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <asp:Label ID="lblMessage" runat="server" Text="Label" ForeColor="Red"></asp:Label>
    
    <br />
    <asp:Label ID="Label1" runat="server" Text="Lookup by Container Number:"></asp:Label>
    <asp:TextBox ID="txtContainerNumber" type="number" CssClass="scanner" runat="server"></asp:TextBox>
    <asp:Button ID="btnLookupContainer" CssClass="submit scanSubmit" runat="server" Text="Lookup Container" OnClick="btnLookupContainer_Click" />
    <br />
    <asp:Label ID="lblAlreadyVerified" runat="server" CssClass="notification" Visible="false" Text="That container has already been audited."></asp:Label>
    <br />
    <asp:Label ID="lblUnverified" runat="server" Text="Unverified Containers:"></asp:Label>
    <asp:GridView ID="gvUnverified" CssClass="dataTable" runat="server" AllowPaging="True" CellPadding="5" PageSize="5" Width="100%"
        OnPageIndexChanging="gvUnverified_PageIndexChanging" AutoGenerateColumns="false" AllowSorting="true" OnSorting="TaskGridView_Sorting">
        <AlternatingRowStyle BackColor="#FFFFCC" />
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="ContainerID"
                DataNavigateUrlFormatString="verify.aspx?id={0}"
                Text="Verify" ControlStyle-CssClass="button"
                />

            <asp:TemplateField HeaderText="Container Number" SortExpression="ContainerNumber">
                <ItemTemplate>
                    <asp:Label ID="lblContainerNumber" runat="server" Text='<%# Bind("BinNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Location" SortExpression="Location">
                <ItemTemplate>
                    <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location.RoomName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

   <asp:Label ID="lblAdjustments" runat="server" Text="Container Adjustments:"></asp:Label>
    <asp:GridView ID="gvChanges" CssClass="dataTable smallFont" runat="server" AllowPaging="True" CellPadding="5" PageSize="5" Width="100%"
        OnPageIndexChanging="gvChanges_PageIndexChanging">
        <AlternatingRowStyle BackColor="#FFFFCC" />
    </asp:GridView>

    <table class="buttonGroup cols-2">
        <tr>
            <td><asp:LinkButton ID="btnCancel" CssClass="cancel confirm" runat="server" OnClick="btnCancel_Click">Cancel Audit</asp:LinkButton></td>
            <td><asp:LinkButton ID="btnFinish" CssClass="submit confirm" runat="server" OnClick="btnFinish_Click">Finish Audit</asp:LinkButton></td>
        </tr>
    </table>
</asp:Content>

