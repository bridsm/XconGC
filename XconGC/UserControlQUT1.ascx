<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserControlQUT1.ascx.vb" Inherits="WebUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:GridView ID="gvGcOrder" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSourceQUT1" ForeColor="Black" GridLines="Vertical" Width="100%">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" SortExpression="ItemCode" >
        <ItemStyle CssClass="itemCode" />
        </asp:BoundField>
        <asp:BoundField DataField="Dscription" HeaderText="Dscription" SortExpression="Dscription" >
        <ItemStyle CssClass="itemName" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="ระบุรายละเอียด" SortExpression="Select">
            <ItemTemplate>
                <asp:LinkButton ID="lnkSelect" runat="server" Text='<%# Eval("Select") %>' ClientIDMode="Static" ></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="หมายเหตุ"></asp:TemplateField>
        <asp:TemplateField HeaderText="Qty.">
            <ItemTemplate>
                <asp:TextBox ID="txtQty" runat="server" Width="50px" TextMode="Number" min="0.00" ClientIDMode="Static" Text="0"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Price" ItemStyle-CssClass="price" HeaderText="Price/Unit" SortExpression="Price" DataFormatString="{0:N2}">
        <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="Total">
            <ItemTemplate>
                <asp:Label ID="lblTotal" runat="server" Text="0.00" ClientIDMode="Static"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:BoundField DataField="U_Size" HeaderText="Size" SortExpression="U_Size">
        <HeaderStyle CssClass="hidden" />
        <ItemStyle CssClass="hidden" />
        </asp:BoundField>
        <asp:BoundField DataField="U_Color" HeaderText="Color" SortExpression="U_Color">
        <HeaderStyle CssClass="hidden" />
        <ItemStyle CssClass="hidden" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="AttachFile" Visible="False">
            <ItemTemplate>
                <asp:FileUpload ID="fldAttach" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle BackColor="#CCCC99" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
    <RowStyle BackColor="#F7F7DE" />
    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
    <SortedAscendingCellStyle BackColor="#FBFBF2" />
    <SortedAscendingHeaderStyle BackColor="#848384" />
    <SortedDescendingCellStyle BackColor="#EAEAD3" />
    <SortedDescendingHeaderStyle BackColor="#575357" />
</asp:GridView>
<asp:SqlDataSource  ID="SqlDataSourceQUT1" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="Select [ItemCode], [Dscription]
                            , 'Select' = IIF(U_Size = 'N' and U_Color = 'N', '', 'SELECT') 
                            , [Price] = 
		                            Case
		                            when @_range = 'E' then [U_EarlyPrice]
		                            When @_range = 'S' then [U_StandardPrice]
		                            When @_range = 'O' then [U_OnSitePrice]
		                            Else Price
		                            End
                            ,U_Size, U_Color
                            From [GC_QUT1]
                            Where U_GC_GroupType = @_id">
    <SelectParameters>
        <asp:ControlParameter  ControlID="lblRange" Name="_range" PropertyName="Text"  />
        <asp:ControlParameter ControlID="lblID" Name="_id" PropertyName="Text" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:Label ID="lblID" runat="server" CssClass="hidden"></asp:Label>

<asp:Label ID="lblRange" runat="server" CssClass="hidden"></asp:Label>
<br />
<br />