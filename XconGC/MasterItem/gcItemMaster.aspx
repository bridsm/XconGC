<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="gcItemMaster.aspx.vb" Inherits="MasterItem_gcItemMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
       <div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">GC Item Master</h3>
  </div>
  <div class="panel-body">
      <%--<thead>
    <tr>
      <th></th>
      <th></th>
      <th>Promo</th>
      <th>On-Si</th>
      <th>Stand</th>
      <th>Early</th>
      <th></th>
      <th></th>
      <th></th>
      <th></th>
    </tr>
  </thead>--%>
  <fieldset>
      <%--</form>--%>
    <div class="form-group">
        <%--</form>--%>
      <div class="col-lg-10">
          <asp:DropDownList ID="ddlProject" runat="server" DataSourceID="SqlDataSourceProject" DataTextField="CardName" DataValueField="DocEntry" CssClass="btn btn-default" AppendDataBoundItems="True" AutoPostBack="True" >
              <asp:ListItem Value="-1">Select Project</asp:ListItem>
          </asp:DropDownList>
          <asp:SqlDataSource ID="SqlDataSourceProject" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="SELECT [DocEntry], [CardName] FROM [GC_OQUT]"></asp:SqlDataSource>
           &nbsp&nbsp
          <label style="color:red;">Expire Days</label>
          &nbsp
          <asp:Label ID="lblExpireDays" runat="server" Text=""></asp:Label>
      </div>
        <asp:Label ID="lblDocEntry" runat="server" Visible="False"></asp:Label>
    </div>
    <br />
    <br />
      <div class="col-lg-12">
         
        <div class="col-lg-5"></div>
         <div class="col-lg-7">
          <table class="table table-striped table-hover ">
  <%--<thead>
    <tr>
      <th></th>
      <th></th>
      <th>Promo</th>
      <th>On-Si</th>
      <th>Stand</th>
      <th>Early</th>
      <th></th>
      <th></th>
      <th></th>
      <th></th>
    </tr>
  </thead>--%>
  <tbody>
    <tr>
     
      <td>Promotion</td>
      <td>On-Site</td>
      <td>Standard</td>
      <td>Early</td>
      <td></td>
    </tr>
    <tr class="warning">
     
      <td>Start Date</td>
      <td>
          <asp:TextBox ID="txtSOnsite" runat="server" Width="100px" AutoPostBack="True"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtSOnsite_CalendarExtender" runat="server" BehaviorID="txtSOnsite_CalendarExtender" TargetControlID="txtSOnsite" Format="dd-MM-yyyy">
          </ajaxToolkit:CalendarExtender>
        </td>
      <td>
          <asp:TextBox ID="txtSStandard" runat="server" Width="100px" AutoPostBack="True"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtSStandard_CalendarExtender" runat="server" BehaviorID="txtSStandard_CalendarExtender" TargetControlID="txtSStandard" Format="dd-MM-yyyy">
          </ajaxToolkit:CalendarExtender>
        </td>
      <td>
          <asp:TextBox ID="txtSEarly" runat="server" Width="100px"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtSEarly_CalendarExtender" runat="server" BehaviorID="txtSEarly_CalendarExtender" TargetControlID="txtSEarly" Format="dd-MM-yyyy">
          </ajaxToolkit:CalendarExtender>
        </td>
      <td></td>
    </tr>
      <tr class="warning">
     
      <td>End Date</td>
      <td>
          <asp:TextBox ID="txtEOnsite" runat="server" Width="100px"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtEOnsite_CalendarExtender" runat="server" BehaviorID="txtEOnsite_CalendarExtender" TargetControlID="txtEOnsite" Format="dd-MM-yyyy">
          </ajaxToolkit:CalendarExtender>
          </td>
      <td>
          <asp:TextBox ID="txtEStandard" runat="server" Width="100px" Enabled="False"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtEStandard_CalendarExtender" runat="server" BehaviorID="txtEStandard_CalendarExtender" TargetControlID="txtEStandard" Format="dd-MM-yyyy">
          </ajaxToolkit:CalendarExtender>
          </td>
      <td>
          <asp:TextBox ID="txtEEarly" runat="server" Width="100px" Enabled="False"></asp:TextBox>
          <ajaxToolkit:CalendarExtender ID="txtEEarly_CalendarExtender" runat="server" BehaviorID="txtEEarly_CalendarExtender" TargetControlID="txtEEarly" Format="dd-MM-yyyy">
          </ajaxToolkit:CalendarExtender>
          </td>
      <td></td>
    
    </tr>
      <tr class="warning">
      <td>Discount %</td>
      <td>
          <asp:TextBox ID="txtDOnsite" runat="server" TextMode="Number" min="0" Width="100px">0</asp:TextBox>
          </td>
      <td>
          <asp:TextBox ID="txtDStandard" runat="server" TextMode="Number" min="0" Width="100px">0</asp:TextBox>
          </td>
      <td>
          <asp:TextBox ID="txtDEarly" runat="server" TextMode="Number" min="0" Width="100px">0</asp:TextBox>
          </td>
      <td>
          <asp:Button ID="btnCalulate" runat="server" CssClass="btn btn-primary btn-xs" Text="CAL" />
          </td>
    </tr>
  </tbody>
</table> 
        </div>
      </div>
     
   
      <asp:GridView ID="gvUQT1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceGetQUT1" Width="100%">
          <Columns>
              <asp:BoundField DataField="ItemCode" HeaderText="ItemCode" SortExpression="ItemCode" />
              <asp:BoundField DataField="Dscription" HeaderText="Dscription" SortExpression="Dscription" />
              <asp:BoundField DataField="Group" HeaderText="Group" ReadOnly="True" SortExpression="Group" />
              <asp:TemplateField HeaderText="Price/Unit" SortExpression="Price">
                  <%--<EditItemTemplate>
                      <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("OnSitePrice") %>'></asp:TextBox>
                  </EditItemTemplate>--%>
                  <ItemTemplate>
                      
                      <%--<asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("OnSitePrice") %>' Width="100px"  Style="text-align: right" TextMode="Number" min="0.00"  step="0.01"></asp:TextBox>--%>
                      <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("OnSitePrice") %>' Width="100px"  Style="text-align: right" ></asp:TextBox>
                      <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtPrice" MinimumValue=".00" MaximumValue="999999999999999.99"></asp:RangeValidator>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Price/Unit" SortExpression="Price1">
                 <%-- <EditItemTemplate>
                      <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("StandardPrice") %>' ></asp:TextBox>
                  </EditItemTemplate>--%>
                  <ItemTemplate>
                      <%--<asp:TextBox ID="txtPrice1" runat="server" Text='<%# Bind("StandardPrice") %>' Width="100px"  Style="text-align: right" TextMode="Number" min="0"  step="0.01"></asp:TextBox>--%>
                      <asp:TextBox ID="txtPrice1" runat="server" Text='<%# Bind("StandardPrice") %>' Width="100px"  Style="text-align: right" ></asp:TextBox>
                      <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtPrice1" MinimumValue=".00" MaximumValue="999999999999999.99"></asp:RangeValidator>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Price/Unit" SortExpression="Price2">
                <%--  <EditItemTemplate>
                      <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("EarlyPrice") %>'></asp:TextBox>
                  </EditItemTemplate>--%>
                  <ItemTemplate>
                      <%--<asp:TextBox ID="txtPrice2" runat="server" Text='<%# Bind("EarlyPrice") %>' Width="100px"  Style="text-align: right" TextMode="Number" min="0"  step="0.01"></asp:TextBox>--%>
                      <asp:TextBox ID="txtPrice2" runat="server" Text='<%# Bind("EarlyPrice") %>' Width="100px"  Style="text-align: right" ></asp:TextBox>
                      <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtPrice2" MinimumValue=".00" MaximumValue="999999999999999.99"></asp:RangeValidator>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Size" SortExpression="U_Size">
                  <EditItemTemplate>
                      <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("U_Size") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:DropDownList ID="ddlSize" runat="server" CssClass="btn btn-default" SelectedValue='<%# Bind("U_Size") %>'>
                          <asp:ListItem>N</asp:ListItem>
                          <asp:ListItem>Y</asp:ListItem>
                      </asp:DropDownList>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Color" SortExpression="U_Color">
                  <EditItemTemplate>
                      <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("U_Color") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:DropDownList ID="ddlColor" runat="server" CssClass="btn btn-default" SelectedValue='<%# Bind("U_Color") %>'>
                          <asp:ListItem>N</asp:ListItem>
                          <asp:ListItem>Y</asp:ListItem>
                      </asp:DropDownList>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Photo" SortExpression="Photo">
                  <EditItemTemplate>
                      <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Photo") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Image ID="imgPhoto" runat="server" Height="70px" ImageUrl='<%# Eval("Photo") %>' Width="70px" />
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" SortExpression="DocEntry">
              <HeaderStyle CssClass="hidden" />
              <ItemStyle CssClass="hidden" />
              </asp:BoundField>
              <asp:BoundField DataField="LineNum" HeaderText="LineNum" SortExpression="LineNum">
              <HeaderStyle CssClass="hidden" />
              <ItemStyle CssClass="hidden" />
              </asp:BoundField>
              <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price">
              <HeaderStyle CssClass="hidden" />
              <ItemStyle CssClass="hidden" />
              </asp:BoundField>
          </Columns>
      </asp:GridView>
      <asp:SqlDataSource ID="SqlDataSourceGetQUT1" runat="server" ConnectionString="<%$ ConnectionStrings:XCON_GCConnectionString %>" SelectCommand="SELECT [DocEntry],[LineNum]
      ,[ItemCode]
      ,[Dscription]
      ,U_GC_GroupType AS [Group]
      ,FORMAT(IIF(U_EarlyPrice IS NULL OR U_EarlyPrice = 0, [Price], U_EarlyPrice),'#,###.00') AS EarlyPrice
      ,FORMAT(IIF(U_StandardPrice IS NULL OR U_StandardPrice = 0, [Price], U_StandardPrice), '#,###.00') AS StandardPrice
      ,FORMAT(IIF(U_OnSitePrice IS NULL OR U_OnSitePrice = 0, [Price], U_OnSitePrice), '#,###.00') AS OnSitePrice
      ,[U_Size]
      ,[U_Color]
	  ,[Price]
      ,'~/imgs/' + ItemCode + '.PNG' AS Photo
  FROM [dbo].[GC_QUT1]
WHERE  DocEntry = @DocEntry">
          <SelectParameters>
              <asp:ControlParameter ControlID="ddlProject" ConvertEmptyStringToNull="False" Name="DocEntry" PropertyName="SelectedValue" />
          </SelectParameters>
      </asp:SqlDataSource>
   
      <br />
      <br />
      <br />
      <br />
    <div class="form-group">
      <div class="col-lg-10 col-lg-offset-2" style="text-align: right;">
          <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
          <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" Text="Update" />
      </div>
    </div>
  </fieldset>
<%--</form>--%>
  </div>
</div>
</asp:Content>

