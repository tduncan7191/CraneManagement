<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductManagement.aspx.cs" Inherits="ProductManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="right_col" role="main">
        <asp:GridView ID="GvPrize" runat="server" 
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
            CssClass="table table-hover table-striped" GridLines="None" 
            DataKeyNames="iID" DataSourceID="DsPrize"> 
            <Columns> 
                <asp:TemplateField Visible="false"> 
                    <HeaderTemplate>
                        <asp:Label ID="headerLbl_ID" runat="server" Text='ID' />
                    </HeaderTemplate>
                    <EditItemTemplate> 
                        <asp:Label ID="editLbl_ID" runat="server" Text='<%# Bind("iID") %>'></asp:Label> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="itemLbl_ID" runat="server" Text='<%# Bind("iID") %>'></asp:Label> 
                    </ItemTemplate>   
                </asp:TemplateField> 

                <asp:TemplateField> 
                    <HeaderTemplate>
                        <asp:Label ID="headerLbl_ItemNo" runat="server" Text='ItemNo' />
                        <br />
                        <asp:TextBox ID="headerTxt_ItemNo" runat="server" Text='<%# Bind("ItemNo") %>'></asp:TextBox> 
                    </HeaderTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="editTxt_ItemNo" runat="server" Text='<%# Bind("ItemNo") %>'></asp:TextBox> 
                    </EditItemTemplate>                    
                    <ItemTemplate> 
                        <asp:Label ID="itemLbl_ItemNo" runat="server" Text='<%# Bind("ItemNo") %>'></asp:Label> 
                    </ItemTemplate>   
                </asp:TemplateField> 
                
                <asp:TemplateField>  
                    <HeaderTemplate>
                        <asp:Label ID="headerLbl_Prize" runat="server" Text='Prize' />
                        <br />
                        <asp:TextBox ID="headerTxt_Prize" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox> 
                    </HeaderTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="editTxt_Prize" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="itemLbl_Prize" runat="server" Text='<%# Bind("Name") %>'></asp:Label> 
                    </ItemTemplate>   
                </asp:TemplateField> 

                <asp:TemplateField>  
                    <HeaderTemplate>
                        <asp:Label ID="headerLbl_Cost" runat="server" Text='Cost' />
                        <br />
                        <asp:TextBox ID="headerTxt_Cost" runat="server" Text='<%# Bind("Cost") %>'></asp:TextBox> 
                    </HeaderTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="editTxt_Cost" runat="server" Text='<%# Bind("Cost") %>' ></asp:TextBox> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="itemLbl_Cost" runat="server" Text='<%# Bind("Cost") %>'></asp:Label> 
                    </ItemTemplate>  
                </asp:TemplateField>  
                                
                <asp:TemplateField>  
                    <HeaderTemplate>
                        <asp:Label ID="headerLbl_Vendor" runat="server" Text='Vendor' />
                        <br />
                        <asp:TextBox ID="headerTxt_Vendor" runat="server" Text='<%# Bind("Vendor") %>'></asp:TextBox> 
                    </HeaderTemplate>
                    <EditItemTemplate> 
                        <asp:TextBox ID="editTxt_Vendor" runat="server" Text='<%# Bind("Vendor") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="itemLbl_Vendor" runat="server" Text='<%# Bind("Vendor") %>'></asp:Label> 
                    </ItemTemplate>  
                </asp:TemplateField> 

                <asp:TemplateField>
                    <HeaderTemplate>         
                        <br />
                        <asp:Button ID="btnInsert" runat="Server" Text="Insert" OnClick="GvPrize_Insert" />
                    </HeaderTemplate>
                </asp:TemplateField>    
                <asp:CommandField ShowEditButton="True" /> 
                <asp:CommandField ShowDeleteButton="True" /> 
            </Columns>                             
        </asp:GridView>  
        <asp:SqlDataSource ID="DsPrize" runat="server" 
            ConnectionString="<%$ ConnectionStrings:LiveConnectionString %>" 
            SelectCommand="SELECT iID, itemNo, Name, Vendor, cast(Cost as decimal(18, 2)) as Cost
                            FROM dbo.[Prize]
                            WHERE CreateDate = 
                            (SELECT MAX(CreateDate) FROM dbo.[Prize])" 
            UpdateCommand="UPDATE dbo.[Prize] 
                            SET ItemNo = @ItemNo, Name = @Name, Vendor = @Vendor, Cost = @Cost 
                            WHERE iID = @iID"
            insertcommand="INSERT INTO dbo.[Prize]
                            (CustomerID, CreateDate, itemNo, Name, Vendor, Cost ) 
                            VALUES (@CustomerID, @CreateDate, @itemNo, @Name, @Vendor, @Cost )"
            DeleteCommand="DELETE FROM dbo.[Prize] WHERE iID = @iID">
            <UpdateParameters>
                <asp:Parameter Name="iID" Type="Int32" />
                <asp:Parameter Name="ItemNo" Type="Int32" />
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="Vendor" Type="String" />
                <asp:Parameter Name="Cost" Type="Double" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="iID" Type="Int32" />
            </DeleteParameters>
        </asp:SqlDataSource>
    </div>    
</asp:Content>

