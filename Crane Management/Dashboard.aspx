<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:HiddenField ID="hdnStartDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnEndDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnURL" runat="server" ClientIDMode="Static" />
    <!-- page content -->
    <div class="right_col" role="main">
        <!-- top tiles -->
        <div class="row tile_count">
            <div class="tile_stats_count">
                <asp:Button ID="BtnDownload" runat="server" Text="Download CSV" OnClick="BtnDownload_Click" class="btn btn-outline-primary" />
            </div>
            <div class="col-md-2 tile_stats_count">
                <span class="count_top"><img src="images/favicon.ico">Crane</span>
                <div id="craneName" class="count" style="font-size:25px;"></div>
            </div>
            <div class="col-md-2 tile_stats_count">
                <span class="count_top"><i class="fa fa-trophy"></i>Prize</span>
                <div id="prize" class="count" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="prize_load" style="margin: auto; display: block;" />
            </div>
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-credit-card"></i>Revenue</span>
                <div id="revenue" class="count green" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="revenue_load" style="margin: auto; display: block;" />
            </div>
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-play"></i>Plays</span>
                <div id="plays" class="count" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="plays_load" style="margin: auto; display: block;" />
            </div>
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-credit-card"></i>Wins</span>
                <div id="wins" class="count green" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="wins_load" style="margin: auto; display: block;" />
            </div>        
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-money"></i>Cost Of Goods</span>
                <div id="costOfGoods" class="count red" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="costOfGoods_load" style="margin: auto; display: block;" />
            </div>
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-credit-card-alt"></i>Revenue Per Play</span>
                <div id="revenuePerPlay" class="count" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="revenuePerPlay_load" style="margin: auto; display: block;" />
            </div>
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-percent"></i>Payout</span>
                <div id="payoutPercent" class="count" style="font-size:25px;"></div>
                <img src="/images/loading.gif" id="payoutPercent_load" style="margin: auto; display: block;" />
            </div>
            <div class="col-md-1 tile_stats_count">
                <span class="count_top"><i class="fa fa-star"></i>Hit Rate</span>
                <div id="hitRate" class="count" style="font-size:25px;">1 In</div>
                <img src="/images/loading.gif" id="hitRate_load" style="margin: auto; display: block;" />
            </div>
        </div>
    </div>
        <!-- /top tiles -->

        
       <%-- <div class="row">

            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panlel">
                    <div class="x_title">
                        <h2>Revenue / <small>Cost of Goods</small></h2>
                        <div class="clearfix"></div>
                    </div>
                    
                    <div class="x_content">
                        <div id="chart_plot_01" class="demo-placeholder" style="width:100%;"></div>
                        <img src="/images/loading.gif" id="chart_plot_load" style="margin: auto; display: block;" />
                    </div>      
                </div>
            </div>

                
            <%--<div class="col-md-4 col-sm-4 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Winners Chart</h2>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content"> 
                        
                    </div>
                </div>
            </div>--%>

        <%--</div>

        <br />

        <div class="row">

            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Payout %</h2>
                        <div class="clearfix"></div>
                    </div>
                    
                    <div class="x_content">
                        <div id="chart_plot_02" class="demo-placeholder" style="width:100%;" ></div>
                        <img src="/images/loading.gif" id="chart_plot_02_load" style="margin: auto; display: block;" />
                    </div>      
                </div>
            </div>
        </div>
    </div>--%>
    <!-- /page content -->

</asp:Content>