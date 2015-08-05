<%@ Page Title="" Language="C#" MasterPageFile="~/mobile.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Src="~/NumericKeypad.ascx" TagPrefix="uc1" TagName="NumericKeypad" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .heading {
            
        }
 
        .content {
            margin-left: 20px;
	        display: none;
	    }
        ol {
            margin-left: 20px !important;   
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolder_PageTitle" Runat="Server">
    Main Menu
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolder_Content" Runat="Server">
    <ul class="vertical-list">
        
        <li class="heading">
            <a href="#transactions" class="heading button text-left"><img src="images/icon-arrow-bi.png" /> Transactions</a>

        </li>
        <li class="content" id="transactions">
            <ul class="vertical-list">
                <li><a href="incoming-food/add.aspx" class="button text-left"><img src="images/icon-arrow-reverse.png" /> Incoming Food</a></li>
                <li><a href="outgoing-food/add.aspx" class="button text-left"><img src="images/icon-arrow.png" /> Outgoing Food</a></li>
            </ul>  
        </li>
        
        <li class="heading">
            <a href="#containers" class="heading button text-left"><img src="images/icon-container.png" /> Containers</a>
        </li>    
        <li class="content" id="containers">
            <ul class="vertical-list">
                <li><a href="container/scan.aspx" class="button text-left"><img src="images/icon-scan.png" /> Scan</a></li>
                <li><a href="container/locate.aspx" class="button text-left"><img src="images/icon-locate.png" />Locate</a></li>
                <li><a href="container/add.aspx" class="button text-left"><img src="images/icon-add.png" /> Create New</a></li>
                <li><a href="container/default.aspx" class="button text-left"><img src="images/icon-container.png" /> All Containers</a></li>
            </ul>
        </li>
        
        <li class="heading">
            <a href="#manage" class="heading button text-left"><img src="images/gear.gif" /> Manage</a>
        </li>
        <li class="content" id="manage">
            <ul class="vertical-list">
                <li><a href="agency/default.aspx" class="button text-left"><img src="images/icon-category.png" /> Agencies</a></li>
                <li><a href="audit/default.aspx" class="button text-left"><img src="images/icon-clipboard.png" /> Audit</a></li>
                <li><a href="log/view-log.aspx" class="button text-left"><img src="images/icon-clipboard.png" /> Change Log</a></li>
                <li><a href="distribution-type/default.aspx" class="button text-left"><img src="images/icon-category.png" /> Distribution Types</a></li>
                <li><a href="donor/default.aspx" class="button text-left"><img src="images/icon-person.png" />Donor</a></li>              
		<li><a href="donor-type/default.aspx" class="button text-left"><img src="images/icon-person.png" /> Donor Types</a></li>
                <li><a href="javascript:void(0)" class="button text-left"><img src="images/icon-category.png" /> Food Categories</a>
                    <ol class="vertical-list">
                        <li><a href="food-type/default.aspx" class="button text-left">Regular</a></li>
                        <li><a href="usda-type/default.aspx" class="button text-left">USDA</a></li>
                    </ol>
                </li>
                <li><a href="location/default.aspx" class="button text-left"><img src="images/search.png" /> Locations</a></li>
                <li><a href="desktop/managedata.aspx" class="button text-left"><img src="images/icon-arrow-bi.png" /> Transactions</a></li>
                <li><a href="desktop/users.aspx" class="button text-left"><img src="images/icon-person.png" /> Users</a></li>
            </ul>
        </li>

        <li class="heading">
            <a href="#reports" class="heading button text-left"><img src="images/icon-document.png" /> Reports</a>
        </li>
        <li class="content" id="reports">
            <ul class="vertical-list">
                <li><a href="desktop/reports/shared/choosetemplate.aspx?reportType=0" class="button text-left"><img src="images/icon-arrow-reverse.png" />Incoming</a></li>
                <li><a href="desktop/reports/shared/choosetemplate.aspx?reportType=1" class="button text-left"><img src="images/icon-arrow.png" /> Outgoing</a></li>
                <li><a href="desktop/reports/shared/choosetemplate.aspx?reportType=2" class="button text-left"><img src="images/icon-category.png" />Inventory</a></li>
                <li><a href="desktop/reports/shared/choosetemplate.aspx?reportType=4" class="button text-left"><img src="images/icon-arrow-bi.png" /> In/Out</a></li>
                <li><a href="desktop/reports/shared/choosetemplate.aspx?reportType=3" class="button text-left"><img src="images/icon-category.png" /> Grocery Rescue</a></li>
                <li><a href="incoming-food/print.aspx" class="button text-left"><img src="images/icon-category.png" />In-Kind Slip (Blank)</a></li>
            
            </ul>
        </li>   
    </ul>

    <script>
        
        
        $(document).ready(function () {

            //Hide all divs	
            $("li.content").hide();

            //Open a content section if there is a hash on page load
            if (window.location.hash) {
                $(window.location.hash).slideDown('normal');
            }
            
            //reopen content section on back button or link click
            $(window).on('hashchange', function() {
                $('li.content').slideUp('normal');
                $(window.location.hash).slideDown('normal');
            });

        });
    </script>

</asp:Content>