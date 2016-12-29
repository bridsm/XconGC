<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="LoadCustomer.aspx.vb" Inherits="MasterItem_LoadCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Load Customer Master</h3>
  </div>
  <div class="panel-body">
    <%--<form class="form-horizontal">--%>
  <fieldset>
    <%--<legend>Legend</legend>--%>
    <div class="form-group">
      <label for="inputEmail" class="col-lg-2 control-label">Customer</label>
      <div class="col-lg-10">
        <asp:TextBox ID="txtCustFrom" runat="server"></asp:TextBox>
          <label>-</label>
        <asp:TextBox ID="txtCustTo" runat="server"></asp:TextBox>
      </div>
    </div>
      
    <div class="form-group">
      <div class="col-lg-10 col-lg-offset-2">
          <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
          <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-primary" Text="Load" />
      </div>
    </div>
      <br />
      <br />
      <br />
      <br />
    <div>
        <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" DataKeyNames="CardCode" DataSourceID="LoadCustomerSqlDataSource" Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="White" />
          <Columns>
              <asp:BoundField DataField="CardCode" HeaderText="CustomerCode" ReadOnly="True" SortExpression="CardCode" />
              <asp:BoundField DataField="CardName" HeaderText="CustomerName" SortExpression="CardName" />
              <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
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
    </div>
      
      <asp:SqlDataSource ID="LoadCustomerSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="SELECT [CardCode], [CardName], [Address] FROM [GC_OCRD]"></asp:SqlDataSource>
  </fieldset>
<%--</form>--%>
  </div>
</div>

</asp:Content>

