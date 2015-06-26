<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="Default2" %>
<%@ Register Src="~/NumericKeypad.ascx" TagPrefix="uc1" TagName="NumericKeypad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../js/jquery-1.9.1.js"></script>
    <script src="../js/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="../donor/DonorService.ashx" type="text/javascript"></script>
    <script src="CategoryService.ashx" type="text/javascript"></script>
    <script src="USDAItemService.ashx" type="text/javascript"></script>
    <link href="../css/ui-lightness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" />
    <style>
      tr.spaceBelow > td
        {
            padding-bottom:10px;
        }
      tr.spaceAbove > td
        {
            padding-top:10px;
        }

    </style>
    <script>
        $(document).ready(function () {
            toggleField();

            //$('#<%= txtDonorName.ClientID %>').autocomplete({ source: donorNames });

            $("#<%= txtDonorName.ClientID %>").autocomplete(
            {
                minLength: 2,
                source: donorNames,
                focus: function (event, ui) {
                    $("#<%= txtDonorName.ClientID %>").val(ui.item.label);
                        return false;
                    },

                    select: function (event, ui) {
                        $("#<%= txtDonorName.ClientID %>").val(ui.item.label);
                        //$( "#project-description" ).val( ui.item.address );

                        return false;
                    }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .append("<a>" + item.label + " (" + item.type + ")" + " </br>&nbsp;&nbsp;" + item.address + "</a>")
                  .appendTo(ul);
            };

            $('#<%= txtCategoryType1.ClientID %>').autocomplete({ source: categoryNames });
            $('#<%= txtCategoryType2.ClientID %>').autocomplete({ source: categoryNames });
            $('#<%= txtCategoryType3.ClientID %>').autocomplete({ source: categoryNames });
            $('#<%= txtCategoryType4.ClientID %>').autocomplete({ source: categoryNames });
            $('#<%= txtCategoryType5.ClientID %>').autocomplete({ source: categoryNames });
            // $('#<%= txtUSDAItemNo1.ClientID %>').autocomplete({ source: usdaItmNumbers });
            // $('#<%= txtUSDAItemNo2.ClientID %>').autocomplete({ source: usdaItmNumbers });
            // $('#<%= txtUSDAItemNo3.ClientID %>').autocomplete({ source: usdaItmNumbers });
            // $('#<%= txtUSDAItemNo4.ClientID %>').autocomplete({ source: usdaItmNumbers });
            // $('#<%= txtUSDAItemNo5.ClientID %>').autocomplete({ source: usdaItmNumbers });

            $("#<%=txtDate.ClientID%>").datepicker({ defaultDate: new Date() });

            $("#<%= txtUSDAItemNo1.ClientID %>").autocomplete(
            {
                minLength: 1,
                source: usdaItmNumbers,
                focus: function (event, ui) {
                    $("#<%= txtUSDAItemNo1.ClientID %>").val(ui.item.label);
                    return false;
                },

                select: function (event, ui) {
                    $("#<%= txtUSDAItemNo1.ClientID %>").val(ui.item.label);
                        //$( "#project-description" ).val( ui.item.address );

                        return false;
                    }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .append("<a>" + item.label + " (" + item.description + ")</a>")
                  .appendTo(ul);
            };


            $("#<%= txtUSDAItemNo2.ClientID %>").autocomplete(
            {
                minLength: 1,
                source: usdaItmNumbers,
                focus: function (event, ui) {
                    $("#<%= txtUSDAItemNo2.ClientID %>").val(ui.item.label);
                    return false;
                },

                select: function (event, ui) {
                    $("#<%= txtUSDAItemNo2.ClientID %>").val(ui.item.label);
                    //$( "#project-description" ).val( ui.item.address );

                    return false;
                }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .append("<a>" + item.label + " (" + item.description + ")</a>")
                  .appendTo(ul);
            };



            $("#<%= txtUSDAItemNo3.ClientID %>").autocomplete(
            {
                minLength: 1,
                source: usdaItmNumbers,
                focus: function (event, ui) {
                    $("#<%= txtUSDAItemNo3.ClientID %>").val(ui.item.label);
                    return false;
                },

                select: function (event, ui) {
                    $("#<%= txtUSDAItemNo3.ClientID %>").val(ui.item.label);
                    //$( "#project-description" ).val( ui.item.address );

                    return false;
                }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .append("<a>" + item.label + " (" + item.description + ")</a>")
                  .appendTo(ul);
            };



            $("#<%= txtUSDAItemNo4.ClientID %>").autocomplete(
            {
                minLength: 1,
                source: usdaItmNumbers,
                focus: function (event, ui) {
                    $("#<%= txtUSDAItemNo4.ClientID %>").val(ui.item.label);
                    return false;
                },

                select: function (event, ui) {
                    $("#<%= txtUSDAItemNo4.ClientID %>").val(ui.item.label);
                    //$( "#project-description" ).val( ui.item.address );

                    return false;
                }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .append("<a>" + item.label + " (" + item.description + ")</a>")
                  .appendTo(ul);
            };



            $("#<%= txtUSDAItemNo5.ClientID %>").autocomplete(
            {
                minLength: 1,
                source: usdaItmNumbers,
                focus: function (event, ui) {
                    $("#<%= txtUSDAItemNo5.ClientID %>").val(ui.item.label);
                    return false;
                },

                select: function (event, ui) {
                    $("#<%= txtUSDAItemNo5.ClientID %>").val(ui.item.label);
                    //$( "#project-description" ).val( ui.item.address );

                    return false;
                }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                  .append("<a>" + item.label + " (" + item.description + ")</a>")
                  .appendTo(ul);
            };


        });

            function toggleField() {

                if ($('#<%= ckbxIsUSDA.ClientID %>').is(":checked")) {
                $('#<%= nonUSDA7.ClientID %>').hide("fast");
                $('#<%= nonUSDA1.ClientID %>').hide("fast");
                $('#<%= nonUSDA2.ClientID %>').hide("fast");
                $('#<%= nonUSDA3.ClientID %>').hide("fast");
                $('#<%= nonUSDA4.ClientID %>').hide("fast");
                $('#<%= nonUSDA5.ClientID %>').hide("fast");
                $('#<%= nonUSDA6.ClientID %>').hide("fast");
                $('#<%= isUSDA1.ClientID %>').show("fast");
                $('#<%= isUSDA2.ClientID %>').show("fast");
                $('#<%= isUSDA3.ClientID %>').show("fast");
                $('#<%= isUSDA4.ClientID %>').show("fast");
                $('#<%= isUSDA5.ClientID %>').show("fast");
                $('#<%= isUSDA6.ClientID %>').show("fast");
            }
            else {
                if ($("#<%= txtDonorName.ClientID %>").val().toLowerCase() == "usda") {
                    $("#<%= txtDonorName.ClientID %>").val("");
                }

                $('#<%= nonUSDA7.ClientID %>').show("fast");
                $('#<%= nonUSDA1.ClientID %>').show("fast");
                $('#<%= nonUSDA2.ClientID %>').show("fast");
                $('#<%= nonUSDA3.ClientID %>').show("fast");
                $('#<%= nonUSDA4.ClientID %>').show("fast");
                $('#<%= nonUSDA5.ClientID %>').show("fast");
                $('#<%= nonUSDA6.ClientID %>').show("fast");

                $('#<%= isUSDA1.ClientID %>').hide("fast");
                $('#<%= isUSDA2.ClientID %>').hide("fast");
                $('#<%= isUSDA3.ClientID %>').hide("fast");
                $('#<%= isUSDA4.ClientID %>').hide("fast");
                $('#<%= isUSDA5.ClientID %>').hide("fast");
                $('#<%= isUSDA6.ClientID %>').hide("fast");
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    <img src="../images/icon-arrow.png" />Add Incoming Food
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    
    <asp:Panel ID="panelForBtn" runat="server" DefaultButton="btnSave">

    <h1>Add Incoming Food</h1>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" style ="font-size:12pt; font-weight:bold;"></asp:Label>
    
    <table class="cols-3" style ="width: 100%;">
        <tr class="spaceBelow spaceAbove">
           <td>Is USDA?</td>
            <td><asp:CheckBox ID="ckbxIsUSDA" runat="server"  CssClass="checkBox"  onClick="toggleField()" /></td>
             <td> </td>
        </tr>
        <tr id="nonUSDA7" runat="server" class="spaceBelow">
            <td>Donor:</td>
            <td><asp:TextBox runat="server" ID="txtDonorName" style="margin-right:2em;"></asp:TextBox></td>
            <td>
                <asp:Button ID="btnAddDonor" runat="server" Text="Create Donor" CssClass="submit" Width="97%" style="
                    margin-left:3%; font-size:x-large;" OnClick="btnAddDonor_Click"/>
            </td>
        </tr>
        <tr>
            <td>Date:</td>
            <td><asp:TextBox ID="txtDate" type="text" runat="server"></asp:TextBox></td>
            <td> </td>
        </tr>
    </table>
    <table class="cols-3"  style ="width: 100%;">
        <tr>
            <td id="nonUSDA1" runat="server" style="font-weight:bold;">Category:</td>
            <td id="isUSDA1" runat="server" style="font-weight:bold;">USDA Item#:</td>
            <td style="font-weight:bold;">Weight:</td>
            <td style="font-weight:bold;">Cases:</td>
        </tr>
        <tr>
            <td id="nonUSDA2" runat="server"><asp:TextBox runat="server" ID="txtCategoryType1" style="margin-right:2em;"></asp:TextBox></td>
            <td id="isUSDA2" runat="server"><asp:TextBox runat="server" ID="txtUSDAItemNo1" style="margin-right:2em;" ></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtWeight1" style="margin-right:2em;" type="number" step="any"></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtCases1" style="margin-right:2em;" type="number"></asp:TextBox></td>
        </tr>
        <tr>
            <td id="nonUSDA3" runat="server"><asp:TextBox runat="server" ID="txtCategoryType2" style="margin-right:2em;"></asp:TextBox></td>
            <td id="isUSDA3" runat="server"><asp:TextBox runat="server" ID="txtUSDAItemNo2" style="margin-right:2em;" ></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtWeight2" style="margin-right:2em;" type="number" step="any"></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtCases2" style="margin-right:2em;" type="number"></asp:TextBox></td>
        </tr>
        <tr>
            <td id="nonUSDA4" runat="server"><asp:TextBox runat="server" ID="txtCategoryType3" style="margin-right:2em;"></asp:TextBox></td>
            <td id="isUSDA4" runat="server"><asp:TextBox runat="server" ID="txtUSDAItemNo3" style="margin-right:2em;" ></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtWeight3" style="margin-right:2em;" type="number" step="any"></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtCases3" style="margin-right:2em;" type="number"></asp:TextBox></td>
        </tr>
        <tr>
            <td id="nonUSDA5" runat="server"><asp:TextBox runat="server" ID="txtCategoryType4" style="margin-right:2em;"></asp:TextBox></td>
            <td id="isUSDA5" runat="server"><asp:TextBox runat="server" ID="txtUSDAItemNo4" style="margin-right:2em;" ></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtWeight4" style="margin-right:2em;" type="number" step="any"></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtCases4" style="margin-right:2em;" type="number"></asp:TextBox></td>
        </tr>
        <tr class="spaceBelow">
            <td id="nonUSDA6" runat="server"><asp:TextBox runat="server" ID="txtCategoryType5" style="margin-right:2em;"></asp:TextBox></td>
            <td id="isUSDA6" runat="server"><asp:TextBox runat="server" ID="txtUSDAItemNo5" style="margin-right:2em;" ></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtWeight5" style="margin-right:2em;" type="number" step="any"></asp:TextBox></td>
            <td><asp:TextBox runat="server" ID="txtCases5" style="margin-right:2em;" type="number"></asp:TextBox></td>
        </tr>
        
      <%--  <tr class="spaceBelow">
            <td>Create a new container from this donation?</td>
            <td><asp:CheckBox ID="chkNewContainer" CssClass="checkBox" runat="server" /></td>
            <td> </td>
        </tr>--%>

    </table>

    <table class="buttonGroup cols-2">
        <tr>
            <td><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel"  Width="100%" OnClick="btnCancel_Click" /></td>
            <td><asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="submit confirm" OnClick="btnSave_Click" /></td>
        </tr>
    </table>

    </asp:Panel>
</asp:Content>