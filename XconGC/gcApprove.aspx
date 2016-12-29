<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="gcApprove.aspx.vb" Inherits="gcApprove" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
     <style type="text/css">
        .MyTabStyle .ajax__tab_header
        {
            font-family: "Helvetica Neue" , Arial, Sans-Serif;
            font-size: 14px;
            font-weight:bold;
            display: block;

        }
        .MyTabStyle .ajax__tab_header .ajax__tab_outer
        {
            border-color: #222;
            color: #222;
            padding-left: 10px;
            margin-right: 3px;
            border:solid 1px #d7d7d7;
        }
        .MyTabStyle .ajax__tab_header .ajax__tab_inner
        {
            border-color: #666;
            color: #666;
            padding: 3px 10px 2px 0px;
        }
        .MyTabStyle .ajax__tab_hover .ajax__tab_outer
        {
            background-color:#424242;
        }
        .MyTabStyle .ajax__tab_hover .ajax__tab_inner
        {
            color: #fff;
        }
        .MyTabStyle .ajax__tab_active .ajax__tab_outer
        {
            border-bottom-color: #ffffff;
            background-color: #d7d7d7;
        }
        .MyTabStyle .ajax__tab_active .ajax__tab_inner
        {
            color: #000;
            border-color: #333;
        }
        .MyTabStyle .ajax__tab_body
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            background-color: #303030;
            border-top-width: 0;
            border: solid 1px #d7d7d7;
            border-top-color: #ffffff;
        }
    </style>

    <script>
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
    </script>

<div class="panel panel-primary">
   <div class="panel-heading">
    <h3 class="panel-title">Approve</h3>
  </div>
  <div class="panel-body">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td>Cust ID</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblCustID" runat="server"></asp:Label>
                        <asp:Label ID="lblDocEntry" runat="server" CssClass="hidden"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>Doc. Date</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtDocDate" runat="server" Enabled="False"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtDocDate_CalendarExtender" runat="server" BehaviorID="txtDocDate_CalendarExtender" TargetControlID="txtDocDate" />
                    </td>
                </tr>
                <tr style="height:10px;">
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
                    <td>Project</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblProjectName" runat="server" CssClass=""></asp:Label>
                        <asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="hidden" DataSourceID="SqlDataSourceProject" DataTextField="ProjectName" DataValueField="CardCode">
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
                    <td>Expire Date</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtExpireDate" runat="server" Enabled="False"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtExpireDate_CalendarExtender" runat="server" BehaviorID="txtExpireDate_CalendarExtender" TargetControlID="txtExpireDate" />
                    </td>
                </tr>
                <tr style="height:15px;">
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
                        <asp:Button ID="Button2" runat="server" Text="Load Data" CssClass="btn btn-default btn-sm" />
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

            <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="MyTabStyle" >
              
            </ajaxToolkit:TabContainer>
          
          <div>
              <asp:GridView ID="gvGcOrder" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDataSourceQUT1" ForeColor="Black" GridLines="Vertical" Width="100%" Visible="False">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" SortExpression="ItemCode" />
        <asp:BoundField DataField="Dscription" HeaderText="Dscription" SortExpression="Dscription" />
        <asp:TemplateField HeaderText="ระบุรายละเอียด" SortExpression="Select">
            <EditItemTemplate>
                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Select") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="lnkSelect" runat="server" Text='<%# Eval("Select") %>' CommandName="SELECT" OnClientClick="return false;"></asp:LinkButton>
                <asp:Button ID="Button1" runat="server" Text="Button" />
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
        <asp:BoundField DataField="U_Size" HeaderText="U_Size" SortExpression="U_Size">
        <HeaderStyle CssClass="hidden" />
        <ItemStyle CssClass="hidden" />
        </asp:BoundField>
        <asp:BoundField DataField="U_Color" HeaderText="U_Color" SortExpression="U_Color">
        <HeaderStyle CssClass="hidden" />
        <ItemStyle CssClass="hidden" />
        </asp:BoundField>
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
<asp:SqlDataSource ID="SqlDataSourceQUT1" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="Select [ItemCode], [Dscription]
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
        <asp:ControlParameter ControlID="lblRange" Name="_range" PropertyName="Text" />
        <asp:ControlParameter ControlID="lblID" Name="_id" PropertyName="Text" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:Label ID="lblID" runat="server" CssClass="hidden"></asp:Label>
<asp:Label ID="lblRange" runat="server" CssClass="hidden"></asp:Label>

          </div>

        <div>
            <ajaxToolkit:ModalPopupExtender ID="mpeSelect" runat="server" TargetControlID="btnHidden" BackgroundCssClass="modalBackground" Drag="True" PopupControlID="panel1"></ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="btnHidden" runat="server" Style="display: none" />
            <asp:Panel ID="panel1" runat="server" BackColor="transparent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                    <ContentTemplate>
                        
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
                
        </div>
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
    <div class="col-sm-2" style="text-align:right;">
        <asp:Label ID="lblGrandTotal" runat="server"></asp:Label><br />
        <asp:Label ID="lblTax" runat="server"></asp:Label><br />
        <asp:Label ID="lblGrandTotalTax" runat="server"></asp:Label>
        <br />
        <br />
    </div>
</div>
            <div style="text-align:right;">
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
</asp:Content>

