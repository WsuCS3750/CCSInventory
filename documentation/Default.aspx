<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="documentation_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.9.1.js"></script>
    <link href="prettify/prettify.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="prettify/prettify.js"></script>

    <style>
        html, body {
            font-family:Verdana, Arial, sans-serif;
            margin:0px;
            padding:0px;
        }
        #header h1, #header h2 {
            background:#222;
            color:#f9f9f9;
            margin:0px;
            padding:5px;
        }
        h1 {
            font-size:24px;
        }
        h2 {
            font-size:18px;
        }
        #menu {
            width:400px;
            position:fixed;
            float:left;
            height:600px;
            overflow-y:scroll;
        }
            #menu li {
                color:blue;
                cursor:pointer;
            }
        li.indent {
            margin-left:20px;
        }
        #content {
            width:900px;
            margin-left:450px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="header">
            <h1>CCS Inventory</h1>
            <h2>Technical Documentation</h2>
        </div>
        <div id="menu">
            Misc.
            <ul>
                <li data-link="audit">Audit</li>
                <li data-link="behaviorjs">behavior.js</li>
                <li data-link="database-access">Database Access</li>
                <li data-link="errors">Error Logging</li>
                <li data-link="organization">Project organization</li>
                <li data-link="masterpage">Masterpage Inheritance</li>
				<li data-link="reports">Reports</li>
            </ul>
            Data Objects
            <ul>
                <li data-link="address">Address</li>
                <li data-link="agency">Agency</li>
                <li data-link="container">Container</li>
                <li data-link="foodsource">Food Source (AKA Donor)</li>
                <li class="indent" data-link="donorservice">Donor Service</li>
                <li data-link="foodtype">Food Category (AKA Food Type)</li>
                <li data-link="location">Location</li>
                <li data-link="usdatype">USDA Category (AKA USDA Type)</li>
            </ul>
            Management Functions
            <ul>
                <li data-link="managedata">Manage Food In / Food Out</li>
                <li data-link="editfood">Editing Food In / Food Out</li>
                <li data-link="users">Manage Users</li>
                <li data-link="edituser">Editing / Adding Users</li>
            </ul>
        </div>
        
        <div id="content">

        </div>
        <script>
            $(document).ready(function () {
                $("#menu li").click(function () {
                    var link = $(this).attr('data-link');
                    $("#content").load("docs/" + link + ".html", function () {
                        $("pre").addClass("prettyprint");
                        prettyPrint();
                    });
                });
            });
        </script>
    </div>
    </form>
</body>
</html>
