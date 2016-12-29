<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="gcOrder.aspx.vb" Inherits="gcOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">

        String.prototype.toNumer = function () {
            const text = this.valueOf();
            const result = parseFloat(text.replace(/,/g, ""));
            return isNaN(result) ? 0 : result;
        }

        var txtQtyChanged = (e) => {

            const $txtQty = $(e.target);

            // check is empty string
            if ($.trim($txtQty.val()) === '') {
                $(e.target).val('0');
                return;
            }

            // check is NaN            
            const qty = $txtQty.val().toNumer();
            if (qty === 0) {
                $(e.target).val('0');
                return;
            }

            const row = $txtQty.closest("tr");
            const price = $(".price", row).text().toNumer();
            const total = price * qty;

            $("#lblTotal", row).text(total);

            var grandTotal = 0.00;
            var tax = 0.00;
            var grandTotalTax = 0.00;

            $("[id=lblTotal]").each((idx, elm) => {
                const _total = $(elm).text().toNumer();
                if (!isNaN(_total)) {
                    grandTotal = grandTotal + _total;
                    tax = grandTotal * (7 / 100);
                    grandTotalTax = grandTotal + tax;
                }
            });

            $("#lblGrandTotal").text(grandTotal.toFixed(2).toString());
            $("#lblTax").text(tax.toFixed(2).toString());
            $("#lblGrandTotalTax").text(grandTotalTax.toFixed(2).toString());
        };

        var lnkSelectClick = (e) => {
            e.preventDefault();
            const row = $(e.target).closest("tr");
            const code = $(".itemCode", row).text();
            const name = $(".itemName", row).text();

            const $panel = $("[id*=panelDetailItem]");
            $("[id*=lblItemID]", $panel).text(code);
            $("[id*=lblItemName]", $panel).text(name);

            // open dialog
            $("#btnShowPanel").click();
        };

        var btnCancelClick = (e) => {
            e.preventDefault();
            __doPostBack($(e.currentTarget)[0].id, "");
        };

        $(() => {
            $(document)
                .on("click", "[id=lnkSelect]", this.lnkSelectClick)
                .on("change", "[id=txtQty]", this.txtQtyChanged)
                //.on("click", "[id=btnCancel]", this.btnCancelClick)
            ;
        });

    </script>

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


                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="MyTabStyle">
                </ajaxToolkit:TabContainer>
                <ajaxToolkit:ModalPopupExtender ID="mpeSelect" runat="server" BehaviorID="TabContainer1_ModalPopupExtender" BackgroundCssClass="modalBackground" PopupControlID="panelDetailItem" TargetControlID="btnShowPanel">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Button ID="btnShowPanel" runat="server" Text="" Style="display: none;" ClientIDMode="Static" />

                <asp:Panel ID="panelDetailItem" runat="server" BackColor="transparent">

                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Item Detail</h3>
                        </div>
                        <div class="panel-body">
                            <table style="width: 400px; background-color: #303030; color: white;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblItemID" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblItemName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>ชื่อป้าย
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtLabelName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>สี
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtColor" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>ขนาด
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtSize" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Rem. 1
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtRem1" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Rem. 2
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtRem2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>AttachFiles</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:FileUpload ID="fldAttach1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:FileUpload ID="fldAttach2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:FileUpload ID="fldAttach3" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:FileUpload ID="fldAttach4" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:FileUpload ID="fldAttach5" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblGvRowIndex" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblGvorder" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Exit" CssClass="btn btn-Default" />
                                    </td>
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

