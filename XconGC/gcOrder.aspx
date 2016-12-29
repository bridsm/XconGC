<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="gcOrder.aspx.vb" Inherits="gcOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <%-- <ul class="nav nav-tabs">
  <li class="active"><a href="#home" data-toggle="tab" aria-expanded="true">Home</a></li>
  <li class=""><a href="#profile" data-toggle="tab" aria-expanded="false">Profile</a></li>
 
</ul>
            <div id="myTabContent" class="tab-content">
  <div class="tab-pane fade active in" id="home">
    <p>Raw denim you probably haven't heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua, retro synth master cleanse. Mustache cliche tempor, williamsburg carles vegan helvetica. Reprehenderit butcher retro keffiyeh dreamcatcher synth. Cosby sweater eu banh mi, qui irure terry richardson ex squid. Aliquip placeat salvia cillum iphone. Seitan aliquip quis cardigan american apparel, butcher voluptate nisi qui.</p>
  </div>
  <div class="tab-pane fade" id="profile">
    <p>Food truck fixie locavore, accusamus mcsweeney's marfa nulla single-origin coffee squid. Exercitation +1 labore velit, blog sartorial PBR leggings next level wes anderson artisan four loko farm-to-table craft beer twee. Qui photo booth letterpress, commodo enim craft beer mlkshk aliquip jean shorts ullamco ad vinyl cillum PBR. Homo nostrud organic, assumenda labore aesthetic magna delectus mollit.</p>
  </div>

</div>--%>


    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=txtQty]").val("0.00");
        });
        $("[id*=txtQty]").live("change", function () {
            if (isNaN(parseInt($(this).val()))) {
                $(this).val('0.00');
            } else {
                $(this).val(parseInt($(this).val()).toString());
            }
        });
        $("[id*=txtQty]").live("change", function () {
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    $("[id*=lblTotal]", row).html(parseFloat($(".price", row).html()) * parseFloat($(this).val()));
                }
            } else {
                $(this).val('');
            }
            var grandTotal = 0.00;
            var tax = 0.00;
            var grandTotalTax = 0.00;

            $("[id*=lblTotal]").each(function () {
                grandTotal = grandTotal + parseFloat($(this).html());
                tax = grandTotal * (7 / 100);
                grandTotalTax = grandTotal + tax;
            });
            $("[id*=lblGrandTotal]").html(grandTotal.toFixed(2).toString());
            $("[id*=lblTax]").html(tax.toFixed(2).toString());
            $("[id*=lblGrandTotalTax]").html(grandTotalTax.toFixed(2).toString());

        <%--$('#<%= lblGrandTotal.ClientID %>').val(grandTotal);
        $('#<%= lblTax.ClientID %>').val(tax);
        $('#<%= lblGrandTotalTax.ClientID %>').val(grandTotalTax);--%>

            $('#<%= hdTotal.ClientID%>').val(grandTotal);
            $('#<%= hdTax.ClientID%>').val(tax);
            $('#<%= hdGrandTotal.ClientID%>').val(grandTotalTax);
        });

        $("[id*=lnkSelect]").live("click", function () {
            var row = $(this).closest("tr");
            $("[id*=lblItemID]", row).html($(".itemCode", row).html());
            $("[id*=lblItemName]", row).html($(".itemName", row).html());
        });

    </script>

    <%--   <script type="text/javascript">
        function showModalPopup(itemCode, itemName) {
            //show the ModalPopupExtender
            $get("<%=lblItemID.ClientID %>").value = itemCode;
            $get("<%=lblItemName.ClientID %>").value = itemName;
            $find("mpe").show();
        }
    </script>--%>

    <%--<script type="text/javascript">
        function ShowModalPopup() {
            $find("mpe").show();

        return false;
    }
    function HideModalPopup() {
        $find("mpe").hide();
        return false;
    }
</script>--%>

    <%-- <input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />
<script type="text/javascript">
        var isPostback =document.getElementById('_ispostback').value;
        if (!isPostback)
        {
            
        }  //populate jquery dialog
        else
        {
            
        }//
</script>--%>

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

    <%--    <script>
        $(function () {
            var tabs = $('#myTab a');
            var counter = 0;
            window.setInterval(activateTab, 5000);
            function activateTab() {
                // remove active class from all the tabs
                tabs.removeClass('active');
                var currentLink = tabs[counter];

                $('.tab-pane').removeClass('.active').hide();
                currentLink.addClass('active');
                $(currentLink.attr('href')).addClass('active').show();
                if (counter < tabs.length)
                    counter = counter + 1;
                else
                    counter = 0;
            }
        });
    </script>--%>



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

                            <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="btn btn-default" DataSourceID="SqlDataSourceProject" DataTextField="ProjectName" DataValueField="CardCode">
                                <asp:ListItem Value="-1">SELECT</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSourceProject" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="SELECT T1.CardCode, T1.CardName, T0.CardName AS ProjectName, T0.CardCode AS ProjectCode FROM GC_OQUT AS T0 INNER JOIN GC_USER AS T1 ON T0.CardCode = T1.ProjectCode WHERE (T1.UserID = @UserID) AND (T1.CardCode = @CardCode)">
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserID" SessionField="UserID" />
                                    <asp:SessionParameter Name="CardCode" SessionField="CardCode" />
                                </SelectParameters>
                            </asp:SqlDataSource>

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
                <ajaxToolkit:ModalPopupExtender ID="mpeSelect" runat="server" BehaviorID="TabContainer1_ModalPopupExtender" BackgroundCssClass="modalBackground" PopupControlID="panel1" TargetControlID="btnShowPanel">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Button ID="btnShowPanel" runat="server" Text="" Style="display: none;" />

                <asp:Panel ID="panel1" runat="server" BackColor="transparent">

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
                        <asp:Label ID="lblGrandTotal" runat="server"></asp:Label><br />
                        <asp:Label ID="lblTax" runat="server"></asp:Label><br />
                        <asp:Label ID="lblGrandTotalTax" runat="server"></asp:Label>
                        <br />
                        <br />
                    </div>
                </div>
                <div style="text-align: right;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Update" CssClass="btn btn-primary" />
                </div>
                <br />
                <br />

                <br />

                <%-- <ul class="nav nav-tabs">
  <li class="active"><a href="#home" data-toggle="tab" aria-expanded="true">Home</a></li>
  <li class=""><a href="#profile" data-toggle="tab" aria-expanded="false">Profile</a></li>
 
</ul>
            <div id="myTabContent" class="tab-content">
  <div class="tab-pane fade active in" id="home">
    <p>Raw denim you probably haven't heard of them jean shorts Austin. Nesciunt tofu stumptown aliqua, retro synth master cleanse. Mustache cliche tempor, williamsburg carles vegan helvetica. Reprehenderit butcher retro keffiyeh dreamcatcher synth. Cosby sweater eu banh mi, qui irure terry richardson ex squid. Aliquip placeat salvia cillum iphone. Seitan aliquip quis cardigan american apparel, butcher voluptate nisi qui.</p>
  </div>
  <div class="tab-pane fade" id="profile">
    <p>Food truck fixie locavore, accusamus mcsweeney's marfa nulla single-origin coffee squid. Exercitation +1 labore velit, blog sartorial PBR leggings next level wes anderson artisan four loko farm-to-table craft beer twee. Qui photo booth letterpress, commodo enim craft beer mlkshk aliquip jean shorts ullamco ad vinyl cillum PBR. Homo nostrud organic, assumenda labore aesthetic magna delectus mollit.</p>
  </div>

</div>--%>
            </fieldset>
        </div>
    </div>

    <asp:HiddenField ID="lblRange" runat="server" />

</asp:Content>

