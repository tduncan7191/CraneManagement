<%@ Page Language='C#' MasterPageFile="~/MasterPage.master" AutoEventWireup='true' CodeFile='Management.aspx.cs' Inherits='_Default' %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="right_col" role="main">
        <!-- top tiles -->
        <div class="row tile_count">
            <table class="table table-condensed">
                <tr>
                    <td>
                        <div class='form-group row'>
                            <label for='location' class='col-sm-2 col-form-label'>Location</label>
                            <div class='col-sm-10'>
                                <asp:Label id='location' runat="server" />
                            </div>
                        </div>
                        <div class='form-group row'>
                            <label for='CustomerID' class='col-sm-2 col-form-label'>CustomerID</label>
                            <div class='col-sm-10'>
                                <asp:Label id='CustomerID' runat="server" />
                            </div>
                        </div>
                        <div class='form-group row'>
                            <label for='date' class='col-sm-2 col-form-label'>Date</label>
                            <div class='col-sm-10'>
                                <asp:Label id='date' runat="server" />
                            </div>
                        </div>
                        <br />
                        <br />
                        <asp:Table ID='tbl' runat='server' Width="100%">
                            <asp:TableRow>
                                <asp:TableCell>
                                <label class='form-control-plaintext'>Game ID</label>
                                </asp:TableCell>
                                <asp:TableCell>
                                <label class='form-control-plaintext'>Swiper Description</label>
                                </asp:TableCell>
                                <asp:TableCell>
                                <label class='form-control-plaintext'>Average Cost</label>
                                </asp:TableCell>
                                <asp:TableCell>
                                <label class='form-control-plaintext'>Prize Description</label>
                                </asp:TableCell>
                                <asp:TableCell>
                                <label class='form-control-plaintext'>Prize Type</label>      
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <br />
                        <br />
                        <label>Add New Prize Description</label>
                        <div class='form-group row'>
                            <input type='text' id='newPrizeDescription' runat="server" placeholder="Prize Description"/>
                            <asp:Button ID='addPrizeDescription' runat="server" Text='Add' OnClick="AddPrizeDescription_Click" />
                            <asp:Button ID="btnSave" style="float:right; margin-right:20%" runat="server" Text="Save Settings" OnClick="BtnSave_ServerClick" />
                        </div>
                        <input type="hidden" runat="server" id="hdnNumberOfRows" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
