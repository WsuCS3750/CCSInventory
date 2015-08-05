<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print-label.aspx.cs" Inherits="container_print" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        h1 {
            font-size:83px;
            margin:.5em 0 0 0;
        }
        h2 {
            margin:0 0 1em 0;
            font-size:52px;
        }
        h3 {
            margin:0 0 1em 0;
            font-size:22px;
        }
        .printarea {
            border:2px solid #000;
            width:600px;
            margin:5px auto;
        }
    </style>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
    <div class="printarea" style="text-align:center; font-family:Calibri">
        <h1><asp:Label ID="lblBinNumber" runat="server" Text=""></asp:Label></h1>
        <h3><asp:Label ID="lblCategory" runat="server" Text=""></asp:Label></h3>
        <img src="#" runat="server" id="image" />
        <h2><asp:Label ID="lblDate" runat="server" Text=""></asp:Label></h2>
    </div>
        <br />
    <div class="printarea" style="text-align:center; font-family:Calibri">
        <h1><asp:Label ID="lblBinNumber2" runat="server" Text=""></asp:Label></h1>
        <h3><asp:Label ID="lblCategory2" runat="server" Text=""></asp:Label></h3>
        <img src="#" runat="server" id="image2" />
        <h2><asp:Label ID="lblDate2" runat="server" Text=""></asp:Label></h2>
    </div>
    </form>
</body>
</html>
