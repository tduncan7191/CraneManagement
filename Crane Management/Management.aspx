<%@ Page Language='C#' MasterPageFile="~/MasterPage.master" AutoEventWireup='true' CodeFile='Management.aspx.cs' Inherits='_Default' %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="right_col" role="main">
        <!-- top tiles -->
        <div class="row tile_count">
            <table class="table table-condensed">
                <tr>
                    <td>
                        <div class='form-group row'>
                            <label for='location' class='col-sm-2 col-form-label' style="font-size:20px;">Location</label>
                            <div class='col-sm-10'>
                                <asp:Label id='location' style="font-size:20px;" runat="server" />
                            </div>
                        </div>
                        <div class='form-group row'>
                            <label for='CustomerID' class='col-sm-2 col-form-label' style="font-size:20px;">CustomerID</label>
                            <div class='col-sm-10'>
                                <asp:Label id='CustomerID' style="font-size:20px;" runat="server" />
                            </div>
                        </div>
                        <div class='form-group row'>
                            <label for='date' class='col-sm-2 col-form-label' style="font-size:20px;">Date</label>
                            <div class='col-sm-10'>
                                <asp:Label id='date' style="font-size:20px;" runat="server" />
                            </div>
                        </div>
                        <br />
                        <br />
                        <asp:Table ID='tbl' runat='server' Width="100%" CellPadding="50">
                            <asp:TableRow >
                                <asp:TableCell>
                                <label class='form-control-plaintext' style="font-size:20px;">Game ID</label>
                                </asp:TableCell>
                                <asp:TableCell>
                                <label class='form-control-plaintext' style="font-size:20px;">Swiper Description</label>
                                </asp:TableCell>
                                <asp:TableCell>
                                <label class='form-control-plaintext' style="font-size:20px;">Prize</label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
