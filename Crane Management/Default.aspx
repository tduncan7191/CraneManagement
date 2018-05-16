<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css"/>    
    <link rel="stylesheet" href="vendors/bootstrap-select/css/bootstrap-select.css"/>

    <!-- Custom Theme Style -->
    <link href="css/custom.css" rel="stylesheet"/>



</head>
<body>
    <form runat="server">    
        <h1 style="width:50%; margin: auto; padding: 10% 0 0 0;">Crane Management Module</h1>
        <div class="form-login" style="width: 50%; margin: auto; padding: 5% 0 0 0;">
            <h4>Welcome back.</h4>
            <input type="text" id="txtUserName" class="form-control input-sm chat-input" placeholder="email" runat="server" />
            <br />
            <input type="text" id="txtUserPassword" class="form-control input-sm chat-input" placeholder="password" runat="server" />
            <br />
        </div>
        <div id="divCraneSelection" style="text-align:center">
            <select id="craneSelection" class="selectpicker" multiple="true" data-live-search="true" data-live-search-placeholder="Search" data-actions-box="true" runat="server" required="required">
                <option value="The Giant">The Giant</option>
                <option value="Ice Crane 1">Ice Crane 1</option>
                <option value="Key Master">Key Master</option>
                <option value="Big Choice Crane 1">Big Choice Crane 1</option>

                <option value="Big Choice Crane 2">Big Choice Crane 2</option>
                <option value="Big Choice Crane 3">Big Choice Crane 3</option>
                <option value="Big Choice Crane 4">Big Choice Crane 4</option>
                <option value="UFO Catcher #1">UFO Catcher #1</option>
                <option value="UFO Catcher #2">UFO Catcher #2</option>
            </select>
            <asp:button class="btn btn-default" Text="Login" runat="server" OnClick="BtnLogin_Click" />
        </div>
    </form>
    
    
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>   
    <script src="vendors/bootstrap-select/js/bootstrap-select.js"></script>

</body>
</html>
