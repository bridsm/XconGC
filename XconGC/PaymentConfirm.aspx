<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="PaymentConfirm.aspx.vb" Inherits="PaymentConfirm" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="panel panel-primary">
   <div class="panel-heading">
    <h3 class="panel-title">PaymentConfirm</h3>
  </div>
  <div class="panel-body">
        <fieldset>
            <div>
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        Cust ID / รหัสลูกค้า
                    </div>
                    <div class="col-sm-9">
                        <%--<asp:TextBox ID="txtCustID" runat="server" Enabled="False"></asp:TextBox>--%>
                        <asp:Label ID="lblCustID" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblCustName" runat="server"></asp:Label>
                        <asp:Label ID="lblDocnum" runat="server" CssClass="hidden"></asp:Label>
                        <br />
                        <br />
                    </div>
                </div>
                
                <div class="col-sm-12">
                    <div class="col-sm-3">
                       Project / โปรเจค
                    </div>
                    <div class="col-sm-9">
                        <asp:Label ID="lblProject" runat="server"></asp:Label>
                        <asp:Label ID="lblProjectCode" runat="server" CssClass="hidden"></asp:Label>
                    </div>
                     <br />
                    <br />
                </div>
               
                <div class="col-sm-12">
                    <div class="col-sm-3">
                       PaymentType / ประเภทการจ่าย
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPaymentType" runat="server" Enabled="true"></asp:TextBox>
                    </div>
                    <br />
                    <br />
                </div>
                
                <div class="col-sm-12">
                    <div class="col-sm-3">
                       Bank / ธนาคาร
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtBank" runat="server" Enabled="true"></asp:TextBox>
                    </div>
                    <br />
                    <br />
                </div>
                
                 <div class="col-sm-12">
                    <div class="col-sm-3">
                        PaymentDate / วันที่จ่าย
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPaymentDate" runat="server" Enabled="true"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtPaymentDate_CalendarExtender" runat="server" BehaviorID="txtPaymentDate_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtPaymentDate">
                        </ajaxToolkit:CalendarExtender>
                    </div>
                     <br />
                     <br />
                </div>
                
                 <div class="col-sm-12">
                    <div class="col-sm-3">
                        Amount / จำนวนเงิน
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtAmount" runat="server" Enabled="true" TextMode="Number" min="0.00"></asp:TextBox>
                    </div>
                     <br />
                     <br />
                </div>
                
                 <div class="col-sm-12">
                    <div class="col-sm-3">
                       AttachFile / แทรกเอกสาร
                    </div>
                    <div class="col-sm-9">
                        <asp:FileUpload ID="fldAttach" runat="server" />
                    </div>
                </div>
                
            </div>

         <div class="form-group">
          <div class="col-lg-9 col-lg-offset-3">
          <br />

              <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
              <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Update" />
              <asp:Label ID="lblUpsertStatus" runat="server" CssClass="hidden"></asp:Label>
          </div>
        </div>
        </fieldset>
  </div>
</div>
</asp:Content>

