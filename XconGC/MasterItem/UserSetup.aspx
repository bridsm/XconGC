<%@ Page Title="User Setup" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="UserSetup.aspx.vb" Inherits="MasterItem_UserSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">USER Setup</h3>
  </div>
  <div class="panel-body">
    <%--<form class="form-horizontal">--%>
  <fieldset>
    <%--<legend>Legend</legend>--%>
    <div class="form-group">
        <div class="col-sm-9">
            <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="btn btn-default" DataSourceID="SqlDataSourceProject" DataTextField="CardName" DataValueField="CardCode">
            <asp:ListItem Value="-1">Select Project</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:FileUpload ID="fulExcel" runat="server" />
            <br />
            <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-primary" Text="Load" />
            <asp:SqlDataSource ID="SqlDataSourceProject" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="SELECT [CardCode], [CardName] FROM [GC_OQUT]"></asp:SqlDataSource>
        <br />
        <br />
        </div>
        <div class="col-sm-3"></div>
    </div>
    
<br />
      <div>
          <asp:Label ID="lblErrmsg" runat="server" ForeColor="Red"></asp:Label>
      </div>

    <div>
        <asp:GridView ID="gvUser" runat="server" Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="White" />
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
    </div>
      
  </fieldset>
<%--</form>--%>
  </div>
</div>
</asp:Content>

