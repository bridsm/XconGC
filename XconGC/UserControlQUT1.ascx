﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserControlQUT1.ascx.vb" Inherits="WebUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
        //grandTotal.toFixed(2)
        //tax.toFixed(2)

        $("[id*=lblTotal]").each(function () {
            grandTotal = grandTotal + parseFloat($(this).html());
            tax = grandTotal * (7 / 100);
            grandTotalTax = grandTotal + tax;
       });
        $("[id*=lblGrandTotal]").html(grandTotal.toFixed(2).toString());
        $("[id*=lblTax]").html(tax.toFixed(2).toString());
        $("[id*=lblGrandTotalTax]").html(grandTotal.toFixed(2).toString());
    });
</script>
<div>
</div>

<asp:GridView ID="gvGcOrder" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSourceQUT1" ForeColor="Black" GridLines="Vertical" Width="100%">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" SortExpression="ItemCode" />
        <asp:BoundField DataField="Dscription" HeaderText="Dscription" SortExpression="Dscription" />
        <asp:TemplateField HeaderText="ระบุรายละเอียด" SortExpression="Select">
            <ItemTemplate>
                <asp:LinkButton ID="lnkSelect" runat="server" Text='<%# Eval("Select") %>' CommandName="SELECT" OnClick="lnkSelect_Click" OnCommand="lnkSelect_Command"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="หมายเหตุ"></asp:TemplateField>
        <asp:TemplateField HeaderText="Qty.">
            <ItemTemplate>
                <asp:TextBox ID="txtQty" runat="server" Width="50px" TextMode="Number" min="0.00"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Price" ItemStyle-CssClass="price" HeaderText="Price/Unit" SortExpression="Price" DataFormatString="{0:N2}">
        <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="Total">
            <ItemTemplate>
                <asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label>
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



<%--<div style="text-align:right;">
    

</div>--%>

<%--<div>
    <ajaxToolkit:ModalPopupExtender ID="mpeSelect" runat="server" TargetControlID="btnHidden" BackgroundCssClass="modalBackground" Drag="True" PopupControlID="panel1"></ajaxToolkit:ModalPopupExtender>
    <asp:Button ID="btnHidden" runat="server" Style="display: none" />
    <asp:Panel ID="panel1" runat="server" BackColor="transparent">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
                <div class="panel panel-primary">
                  <div class="panel-heading">
                    <h3 class="panel-title">Item Detail</h3>
                  </div>
                  <div class="panel-body">
                 <table style="width: 400px; background-color:black; color:white;">
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
                                <td>
                                    ชื่อป้าย
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
                                <td>
                                    สี
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
                                <td>
                                    ขนาด
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
                                <td>
                                    Rem. 1
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
                                <td>
                                    Rem. 2
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtRem2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>&nbsp;</td>
                                <td style="text-align:right;">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Exit" CssClass="btn btn-Default" />
                                </td>
                            </tr>
                           
                        </table>
                    
                  </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
                
</div>--%>


