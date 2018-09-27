<%@ Page Language='C#' MasterPageFile="~/MasterPage.master" AutoEventWireup='true' CodeFile='Management.aspx.cs' Inherits='_Default' %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="right_col" role="main">
        <asp:GridView ID="GvCustomer" runat="server" AutoGenerateColumns="False"
            CssClass="table table-hover table-striped" GridLines="None"
            DataKeyNames="CustomerID" DataSourceID="DsCustomer">
            <Columns>
                <asp:TemplateField HeaderText="Location">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Customer ID">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
        </asp:GridView>  

        <asp:GridView ID="GvCrane" runat="server" AutoGenerateColumns="False"
            CssClass="table table-hover table-striped" GridLines="None"
            DataKeyNames="CM_iID" DataSourceID="DsCrane" OnRowUpdating="GvCrane_RowUpdating">
            <Columns>                
                <asp:TemplateField HeaderText="CM_iID" Visible="false"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("CM_iID") %>'></asp:Label> 
                    </ItemTemplate>   
                </asp:TemplateField> 

                <asp:TemplateField HeaderText="Game ID">
                    <ItemTemplate>
                        <asp:Label ID="lblItemNo" runat="server" Text='<%# Bind("GameID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Crane">
                    <ItemTemplate>
                        <asp:Label ID="lblCrane" runat="server" Text='<%# Bind("Crane") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Prize">
                    <ItemTemplate>
                        <asp:Label ID="lblDisplayPrize" runat="server" Text='<%# Bind("DisplayPrize") %>'></asp:Label> 
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="DDLPrize" runat="server" 
                            DataSourceID ="DsPrize"
                            DataTextField="DisplayPrize"
                            DataValueField="Prize">
                            <asp:ListItem Text="None Selected" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:CommandField ShowEditButton="True" /> 
            </Columns>
        </asp:GridView>  
        <asp:SqlDataSource ID="DsCrane" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LiveConnectionString %>" 
            SelectCommand="SELECT DISTINCT 
                            CM.iID as CM_iID
                            , GS.Game_Id as GameID
                            , GS.Swiper_Description as Crane
                            , P.Name as Prize
							,isNull(cast(P.[ItemNo] as nvarchar(20)),'0') + ' | ' + isNull(P.[Name], 'None') + ' | ' + isNull(cast(P.[Cost] as nvarchar(20)), 'None') + ' | ' + isNull(P.[Vendor], 'None') as DisplayPrize 
                            FROM dbo.Customer AS C 
                            INNER JOIN dbo.Game_Swipers AS GS ON C.CustomerID = GS.CustomerID 
                            INNER JOIN dbo.CraneManagement AS CM ON GS.Game_Id = CM.GameID
                            LEFT JOIN dbo.Prize AS P ON CM.PrizeID = P.iID
                            where C.CustomerID = @CustomerID 
                            AND Date = cast(GETDATE()-1 as Date) 
                            order by Crane" 
            UpdateCommand="UPDATE CM 
                            SET CM.PrizeID = (SELECT P.Iid from [dbo].[Prize] P where P.name = @Prize)
                            FROM [dbo].[CraneManagement] CM 
                            WHERE CM.iID = @CM_iID">
        </asp:SqlDataSource>
                  
        <asp:SqlDataSource ID="DsPrize" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LiveConnectionString %>" 
            SelectCommand="SELECT DISTINCT 
                            P.Name as Prize
                            ,isNull(cast(P.[ItemNo] as nvarchar(20)), '0') + ' | ' + isNull(P.[Name], 'None') + ' | ' + isNull(cast(P.[Cost] as nvarchar(20)), 'None') + ' | ' + isNull(P.[Vendor], 'None') as DisplayPrize 
                            FROM dbo.Prize P 
                            WHERE P.CustomerID = @CustomerID 
                            AND CreateDate = 
                            (SELECT MAX(CreateDate) FROM dbo.[Prize])">
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="DsCustomer" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LiveConnectionString %>" 
            SelectCommand="SELECT [CustomerID], [Location]
                            FROM [ProjectX].[dbo].[Customer]
                            where CustomerID = @CustomerID">
        </asp:SqlDataSource>
    </div>
</asp:Content>
