<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="quickout.aspx.cs" Inherits="outgoing_food_quickout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    Quick Out
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <asp:Panel runat="server" ID="pnlInput">
        <asp:GridView ID="grdFoodOut" runat="server"
                    AutoGenerateColumns="false" DataKeyNames="id"
                    CssClass="dataTable mediumFont"
                    HeaderStyle-HorizontalAlign="Center"
                    CellSpacing="8" HorizontalAlign="Center">
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

                        <asp:TemplateField HeaderText="Cases">
                            <ItemTemplate>
                                <asp:Label ID="lblCases" runat="server" Text='<%# Bind("cases") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            <table class="buttonGroup cols-3">

          <tr class="spaceBelow">
                <td>Donor Type:</td>
                <td><asp:DropDownList ID="ddlDonorType" runat="server" AppendDataBoundItems="true" DataTextField="FoodSourceType1" DataValueField="FoodSourceTypeID">
            </asp:DropDownList>
            </td>
                <td> </td>

             </tr>
            <tr class="spaceBelow">
                <td>Distribution Type:</td>
                <td><asp:DropDownList ID="ddlDistributionType" runat="server" AppendDataBoundItems="true"
                DataTextField="DistributionType1" DataValueField="DistributionTypeID" > </asp:DropDownList></td>
                <td> </td>
             </tr>
              <tr class="spaceBelow">
                <td>Agency:</td>
                <td> <asp:DropDownList ID="ddlAgency" runat="server" AppendDataBoundItems="true" DataTextField="AgencyName" DataValueField="AgencyID">
                    <asp:ListItem Value="0">No Agency</asp:ListItem>
                </asp:DropDownList></td>
                <td> </td>

             </tr>

     
                          
        </table>

        <table class="buttonGroup cols-2">
            <tr>
                <td><a href="javascript:history.back()" class="cancel">Cancel</a></td>
                <td><asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="submit" OnClick="btnSave_Click"/></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSuccess" Visible="false">
        <center>
            <h1 style="color:#0094ff">Success!</h1>
            <p><h2>Food Out Record Successfully Added.</h2></p>
            <p><em>You may now close this tab.</em></p>
        </center>
     </asp:Panel>
</asp:Content>

