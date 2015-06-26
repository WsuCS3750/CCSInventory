<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="DistributionTypeManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    Manage Distribution Types
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>

    <asp:GridView ID="gvDistributionTypes" CssClass="dataTable" runat="server" AllowPaging="True" CellPadding="5" PageSize="5" Width="100%"
        OnPageIndexChanging="gvDistributionTypes_PageIndexChanging" AutoGenerateColumns="false">
        <AlternatingRowStyle BackColor="#FFFFCC" />
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="DistributionTypeID"
                DataNavigateUrlFormatString="edit.aspx?id={0}"
                Text="Edit" ControlStyle-CssClass="button"
                />
            <asp:TemplateField HeaderText="Distribution Type">
                <ItemTemplate>
                    <asp:Label ID="lblDistributionType" runat="server" Text='<%# Bind("DistributionType1") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <table class="buttonGroup cols-2">
        <tr>
            <td><asp:LinkButton ID="btnBack" runat="server" CssClass="cancel" onclientclick="JavaScript: window.history.go(-1); return false;">Back</asp:LinkButton></td>
            <td>
                <a href="add.aspx" class="submit">Add Distribution Type</a>
            </td>
        </tr>
    </table>

</asp:Content>

