<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="gcOrder.aspx.vb" Inherits="gcOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <style type="text/css">
        .MyTabStyle .ajax__tab_header {
            font-family: "Helvetica Neue", Arial, Sans-Serif;
            font-size: 14px;
            font-weight: bold;
            display: block;
        }

            .MyTabStyle .ajax__tab_header .ajax__tab_outer {
                border-color: #222;
                color: #222;
                padding-left: 10px;
                margin-right: 3px;
                border: solid 1px #d7d7d7;
            }

            .MyTabStyle .ajax__tab_header .ajax__tab_inner {
                border-color: #666;
                color: #666;
                padding: 3px 10px 2px 0px;
            }

        .MyTabStyle .ajax__tab_hover .ajax__tab_outer {
            background-color: #424242;
        }

        .MyTabStyle .ajax__tab_hover .ajax__tab_inner {
            color: #fff;
        }

        .MyTabStyle .ajax__tab_active .ajax__tab_outer {
            border-bottom-color: #ffffff;
            background-color: #d7d7d7;
        }

        .MyTabStyle .ajax__tab_active .ajax__tab_inner {
            color: #000;
            border-color: #333;
        }

        .MyTabStyle .ajax__tab_body {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            background-color: #303030;
            border-top-width: 0;
            border: solid 1px #d7d7d7;
            border-top-color: #ffffff;
        }
    </style>

    <div class="panel panel-primary">
        <div class="panel-heading">
            <h3 class="panel-title">GC-Order</h3>
        </div>
        <div class="panel-body">
            <fieldset>
                <table style="width: 100%;">
                    <tr>
                        <td>Cust ID / รหัสลูกค้า</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lblCustID" runat="server"></asp:Label>
                            <asp:Label ID="lblDocEntry" runat="server" CssClass="hidden"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label></td>
                        <td>&nbsp;</td>
                        <td>Doc.Date / วันที่เอกสาร</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtDocDate" runat="server" Enabled="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtDocDate_CalendarExtender" runat="server" BehaviorID="txtDocDate_CalendarExtender" TargetControlID="txtDocDate" />
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lblUserID" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblProjectCode" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblAddress" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblDocNum" runat="server" Text="" Visible="false"></asp:Label>
                            <input id="hdTotal" type="hidden" runat="server" />
                            <input id="hdTax" type="hidden" runat="server" />
                            <input id="hdGrandTotal" type="hidden" runat="server" />
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Project / โปรเจค</td>
                        <td>&nbsp;</td>
                        <td>

                            <asp:Label ID="lblProjectName" runat="server" CssClass=""></asp:Label>

                            <br />
                        </td>
                        <td>&nbsp;</td>
                        <td></td>
                        <td>&nbsp;</td>
                        <td>Expire Date / วันที่หมดอายุ</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtExpireDate" runat="server" Enabled="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtExpireDate_CalendarExtender" runat="server" BehaviorID="txtExpireDate_CalendarExtender" TargetControlID="txtExpireDate" />
                        </td>
                    </tr>
                    <tr style="height: 15px;">
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnLoad" runat="server" Text="Load Data" CssClass="btn btn-warning btn-sm" />
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <br />
                <br />

                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="MyTabStyle" ViewStateMode="Enabled">
                </ajaxToolkit:TabContainer>
                <ajaxToolkit:ModalPopupExtender ID="modalDetailItem" runat="server" BehaviorID="TabContainer1_ModalPopupExtender" BackgroundCssClass="modalBackground" PopupControlID="panelDetailItem" TargetControlID="btnShowPanel" CancelControlID="btnCancel" OkControlID="btnUpdate">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Button ID="btnShowPanel" runat="server" Text="" Style="display: none;" ClientIDMode="Static" />
                <asp:Panel ID="panelDetailItem" runat="server" BackColor="transparent">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Item Detail</h3>
                        </div>
                        <div class="panel-body">
                            <asp:HiddenField ID="hidItemCode" runat="server" ClientIDMode="Static" />
                            <table id="formDetailItem" style="width: 450px; background-color: #303030; color: white;">
                                <tr>
                                    <td>
                                        <asp:Label ID="pop_lblItemID" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td style="width: 320px;">
                                        <asp:Label ID="pop_lblItemName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 35px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>ชื่อป้าย
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="pop_txtLabelName" runat="server"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>สี
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="pop_txtColor" runat="server"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>ขนาด
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="pop_txtSize" runat="server"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Rem. 1
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="pop_txtRem1" runat="server"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Rem. 2
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="pop_txtRem2" runat="server"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>AttachFiles</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload1" runat="server" ThrobberID="myThrobber1" OnClientUploadComplete="uploadComplete" OnClientUploadError="uploadError" />

                                    </td>

                                    <td><span id="myThrobber1" runat="server">
                                        <img alt="" src="imgs/uploading.gif"></span></td>

                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload2" runat="server" ThrobberID="myThrobber2" />

                                    </td>
                                    <td><span id="myThrobber2" runat="server">
                                        <img alt="" src="imgs/uploading.gif"></span></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload3" runat="server" ThrobberID="myThrobber3" />

                                    </td>
                                    <td><span id="myThrobber3" runat="server">
                                        <img alt="" src="imgs/uploading.gif"></span></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload4" runat="server" ThrobberID="myThrobber4" />

                                    </td>
                                    <td><span id="myThrobber4" runat="server">
                                        <img alt="" src="imgs/uploading.gif"></span></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload5" runat="server" ThrobberID="myThrobber5" />

                                    </td>
                                    <td><span id="myThrobber5" runat="server">
                                        <img alt="" src="imgs/uploading.gif"></span></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" ClientIDMode="Static" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Exit" CssClass="btn btn-Default" ClientIDMode="Static" />
                                    </td>
                                    <td style="text-align: right;">&nbsp;</td>
                                </tr>

                            </table>

                        </div>
                    </div>

                </asp:Panel>
                <br />
                <div class="col-sm-12">
                    <div class="col-sm-8">
                        Remark
        <asp:TextBox ID="txtRemark" runat="server" Height="49px" Width="398px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label1" runat="server" Text="Total Bef. Tax"></asp:Label><br />
                        <asp:Label ID="Label2" runat="server" Text="Tax"></asp:Label><br />
                        <asp:Label ID="Label3" runat="server" Text="Total Aft. Tax"></asp:Label>
                    </div>
                    <div class="col-sm-2" style="text-align: right;">
                        <asp:Label ID="lblGrandTotal" runat="server" ClientIDMode="Static"></asp:Label><br />
                        <asp:Label ID="lblTax" runat="server" ClientIDMode="Static"></asp:Label><br />
                        <asp:Label ID="lblGrandTotalTax" runat="server" ClientIDMode="Static"></asp:Label>
                        <br />
                        <br />
                    </div>
                </div>
                <div style="text-align: right;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Update" CssClass="btn btn-primary" />
                </div>
                <br />
                <br />
            </fieldset>
        </div>
    </div>

    <asp:HiddenField ID="lblRange" runat="server" />

</asp:Content>

