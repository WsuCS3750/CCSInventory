<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="incoming_food_menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    Incoming Food Menu
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">

    <asp:GridView ID="grdFoodIn" runat="server"
                AutoGenerateColumns="false" DataKeyNames="id"
                CssClass="dataTable mediumFont"
                OnRowCommand="grdTemplates_RowCommand"
                HeaderStyle-HorizontalAlign="Center"
                CellSpacing="8" HorizontalAlign="Center" >
                <AlternatingRowStyle BackColor="#FFFFCC" />
                <Columns>

                     <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("category") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Weight">
                        <ItemTemplate>
                            <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("weight") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Button ButtonType="button" runat="server" Text="Move Out"  CssClass="submit" Font-Size="16px" OnClientClick="form1.target='_blank';"   RowIndex='<%# Container.DisplayIndex %>' OnClick="MoveOutClick" ></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Button ID="Button1" ButtonType="button" runat="server" Text="Add Container"  CssClass="submit" Font-Size="16px" OnClientClick="form1.target='_blank';" RowIndex='<%# Container.DisplayIndex %>' OnClick="AddContainerClick"    ></asp:Button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

     <ul class="vertical-list" style="max-width:400px; margin:0px auto;">
         <li><asp:button CssClass="button" Runat="Server" ID="btnMoveAllOut" Text="Move All Out" OnClick="btnMoveAllOut_Click"  /></li>
         <li><asp:button  CssClass="button" Runat="Server" ID="btnPrintInKind" Text="Print In Kind Slip" OnClick="btnPrintInKind_Click" OnClientClick="form1.target='_blank';" /></li>
         <li><a href="add.aspx" class="button" style="text-align:center" >New Transaction</a></li>
     </ul>
</asp:Content>

