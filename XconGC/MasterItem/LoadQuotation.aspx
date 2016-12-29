<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="LoadQuotation.aspx.vb" Inherits="MasterItem_LoadQuotation" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Load SalesQuotation</h3>
  </div>
  <div class="panel-body">
    <%--<form class="form-horizontal">--%>
  <fieldset>
    <%--<legend>Legend</legend>--%>
    <div class="form-group">
      <label for="inputEmail" class="col-lg-2 control-label">Quotation No.</label>
      <div class="col-lg-10">
        <asp:TextBox ID="txtQuotFrom" runat="server" TextMode="Number"></asp:TextBox>
          <label>-</label>
        <asp:TextBox ID="txtQuotTo" runat="server" TextMode="Number"></asp:TextBox>
      </div>
    </div>
    <div class="form-group">
      <label for="inputEmail" class="col-lg-2 control-label">Valid Date.</label>
      <div class="col-lg-10">
        <asp:TextBox ID="txtValidFrom" runat="server"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtValidFrom_CalendarExtender" runat="server" BehaviorID="txtValidFrom_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtValidFrom">
          </ajaxToolkit:CalendarExtender>
          <label>-</label>
        <asp:TextBox ID="txtValidTo" runat="server"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtValidTo_CalendarExtender" runat="server" BehaviorID="txtValidTo_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtValidTo">
          </ajaxToolkit:CalendarExtender>
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
      <br />
      <br />
    <div>
        <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" DataSourceID="LoadQuotationSqlDataSource">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" SortExpression="DocEntry" Visible="False" />
                <asp:BoundField DataField="DocNum" HeaderText="DocNum" SortExpression="DocNum" />
                <asp:BoundField DataField="DocStatus" HeaderText="DocStatus" SortExpression="DocStatus" Visible="False" />
                <asp:BoundField DataField="DocDate" HeaderText="DocDate" SortExpression="DocDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="DocDueDate" HeaderText="DocDueDate" SortExpression="DocDueDate" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="CardCode" HeaderText="CardCode" SortExpression="CardCode" />
                <asp:BoundField DataField="CardName" HeaderText="CardName" SortExpression="CardName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" Visible="False" />
                <asp:BoundField DataField="NumAtCard" HeaderText="NumAtCard" SortExpression="NumAtCard" Visible="False" />
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                <asp:BoundField DataField="Project" HeaderText="Project" SortExpression="Project" />
                <asp:BoundField DataField="U_MainProject" HeaderText="MainProject" SortExpression="U_MainProject" />
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
      
      <asp:SqlDataSource ID="LoadQuotationSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="SELECT T0.DocEntry,T0.DocNum,T0.DocStatus,T0.DocDate,T0.DocDueDate,T0.CardCode,T0.CardName,T0.Address
, T0.NumAtCard,T0.Comments,T0.Project,T0.U_MainProject From GC_OQUT T0"></asp:SqlDataSource>
  </fieldset>
<%--</form>--%>
  </div>
</div>
</asp:Content>

