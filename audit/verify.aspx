<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="verify.aspx.cs" Inherits="Verify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    <img src="../images/icon-clipboard.png" />Verify Container <asp:Label ID="lblNumber" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <table class="form">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Is USDA?</td>
            <td>
                <asp:CheckBox class="input" ID="chkIsUSDA" runat="server" CssClass="checkBox" onClick="checkboxClick()" />
            </td>
        </tr>
        <tr>
            <td>Weight</td>
            <td><asp:TextBox class="input" ID="txtWeight" runat="server" type="number" step="any"></asp:TextBox></td>
        </tr>
        <tr class="nonusda">
            <td>Food Category</td>
            <td>
                <asp:DropDownList class="input" ID="ddlFoodCategory" runat="server" DataTextField="CategoryType" DataValueField ="FoodCategoryID">
                    <asp:ListItem Value="1">Select Food Type</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="usda">
            <td>USDA Category</td>
            <td>
                <asp:DropDownList class="input" ID="ddlUSDACategory" runat="server"  DataTextField="Description" DataValueField ="USDAID">
                    <asp:ListItem Value="1">Select Food Type</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Number of Cases</td>
            <td>
                <asp:TextBox class="input" ID="txtNumberOfCases" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Location</td>
            <td>
                <asp:DropDownList class="input" ID="ddlLocation" runat="server"  DataTextField="RoomName" DataValueField ="LocationID">
                    <asp:ListItem Value="1">Select Location</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Food Source Type</td>
            <td>
                <asp:DropDownList class="input" ID="ddlFoodSourceType" runat="server" AppendDataBoundItems="true"
                    DataTextField="FoodSourceType1" DataValueField="FoodSourceTypeID">
                    <asp:ListItem Value="0">Select Food Source Type</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblWeightError" runat="server" ForeColor="Red" />
            </td>
        </tr>
        </table>      
    <table class="buttonGroup cols-2">
        <tr>
            <td><a href="perform.aspx" class="cancel">Cancel</a></td>
            <td>
                <asp:LinkButton ID="btnVerify" runat="server" Text="Verify" CssClass="submit" OnClick="btnVerify_Click"/>
                <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="submit" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>

    <script>

        function checkboxClick()
        {
            if ($('#<%= chkIsUSDA.ClientID %>').is(':checked'))
            {
                $('.usda').show('fast');
                $('.nonusda').hide();
            } else
            {
                $('.usda').hide();
                $('.nonusda').show('fast');
            }
        }
        
        $('#<%= chkIsUSDA.ClientID %>').click(function ()
        {
            checkboxClick();

            $('#<%=btnVerify.ClientID %>').fadeOut(400,
                function ()
                {
                $('#<%=btnSave.ClientID %>').fadeIn();
            });
        });

        $(document).ready(function ()
        {
            checkboxClick();
            $('#<%=btnSave.ClientID %>').hide();

            $('.input').change(function ()
            {
                $('#<%=btnVerify.ClientID %>').fadeOut(400,
                    function ()
                    {
                        $('#<%=btnSave.ClientID %>').fadeIn();
                    });
            });
        });
            
    </script>
</asp:Content>

