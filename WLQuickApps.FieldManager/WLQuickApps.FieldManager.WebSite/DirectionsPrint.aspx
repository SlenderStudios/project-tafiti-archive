<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Directions</title>
        <style type="text/css">
            body
            {
                font-family: Verdana;
                font-size: .7em;
            }
            table
            {
                border-collapse: collapse;
            }
            a
            {
                text-decoration: none;
                color: #000;
            }
            th
            {
                text-align: left;
            }
            td
            {
                text-align: left;
                padding: 3px;
                padding-right: 10px;
                border-bottom: 1px Solid #BBB;
            }
        </style>
    </head>
    <body>
        <div id="directionsTable">
        
        </div>
        
        <script type="text/javascript">
            document.getElementById("directionsTable").innerHTML = window.opener.GetRouteTable();
            
            window.print();
            self.close();
        </script>
    </body>
</html>
