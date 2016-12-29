Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Imports System.Globalization

Partial Class gcOrder
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")
    Dim dt, dtbind As DataTable
    Dim userID, cardCode, projectCode As String

    Private Sub gcOrder_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing = True Then
            Response.Redirect("login.aspx")
        End If

        userID = Session("UserID")
        cardCode = Session("CardCode")
        projectCode = Session("ProjectCode")

        If Page.IsPostBack = False Then

            lblCustID.Text = Session("CardCode")
            lblUserID.Text = Session("UserID")
            'BindData(userID, cardCode)
            'btnLoad_Click(1, e)
        Else

        End If

        If (Page.IsAsync = False Or Page.IsCallback = False) Then
            Dim client = ScriptManager.GetCurrent(Page)
            client.RegisterPostBackControl(btnLoad)
        End If
        'BindData(userID, cardCode)

        'ddlProject_SelectedIndexChanged(1, e)

        'btn1.Attributes.Add("onClick", "return false;")

    End Sub

    Private Sub BindData(ByVal userID As String, ByVal cardCode As String)
        Dim _rangeDate As String
        Dim _sql As String = "SELECT T0.[DocEntry], T0.[DocNum], FORMAT(GETDATE(),'dd/MM/yyyy') as DocDate
		                        , FORMAT(DATEADD(day,U_ExpireDay,GETDATE()), 'dd/MM/yyyy') AS EndDate
		                        ,'_Range' =
		                        CASE
		                        when GETDATE() >= [U_EarlyFromDate] and GETDATE() <= [U_EarlyToDate] THEN 'E'
		                        When GETDATE() >= [U_StandardFromDate] and GETDATE() <= [U_StandardToDate] THEN 'S'
		                        When GETDATE() >= [U_OnSiteFromDate] and GETDATE() <= [U_OnSiteToDate] THEN 'O'
		                        ELSE 'P'
		                        END
                                ,T1.CardCode, T1.CardName , T0.CardName As ProjectName, T0.CardCode AS ProjectCode, T0.DocCur, T0.DocRate, GC_OCRD.Address
                                FROM  GC_OQUT T0 INNER JOIN GC_USER T1 ON T0.CardCode = T1.ProjectCode LEFT JOIN
                                GC_OCRD ON T1.CardCode = GC_OCRD.CardCode
                                WHERE T1.UserID ='" & userID & "' AND T1.CardCode = '" & cardCode & "'"

        Dim dt As DataTable

        dt = db.getDataTableByQuery(_sql, "GetDocDate")

        If dt.Rows.Count > 0 Then
            txtDocDate.Text = dt.Rows(0)(2)
            txtExpireDate.Text = dt.Rows(0)(3)
            _rangeDate = dt.Rows(0)(4)
            lblProjectName.Text = dt.Rows(0).Item("ProjectName")
            lblProjectCode.Text = dt.Rows(0).Item("ProjectCode")
            If dt.Rows(0).Item("Address") Is DBNull.Value Then
                lblAddress.Text = ""
            Else
                lblAddress.Text = dt.Rows(0).Item("Address")
            End If
            lblDocNum.Text = dt.Rows(0).Item("DocNum")
            lblDocEntry.Text = (dt.Rows(0)(0))

            LoadTab(Convert.ToInt32(lblDocEntry.Text), _rangeDate)
            'GenerateTab(Convert.ToInt32(lblDocEntry.Text), _rangeDate)
        End If

    End Sub

    'Private Function GetFormatDate(ByVal _EarlyFrom As DateTime, ByVal _EarlyTo As DateTime, ByVal _StandardFrom As DateTime _
    '                            , ByVal _StandardTo As DateTime, ByVal _OnSiteFrom As DateTime, ByVal _OnSiteTo As DateTime) As DateTime

    '    Dim EarlyFrom, EarlyFrom_, EarlyTo, EarlyTo_, StandardFrom, StandardFrom_, StandardTo, StandardTo_, OnSiteFrom, OnSiteFrom_, OnSiteTo, OnSiteTo_ As DateTime

    '    If DateTime.TryParseExact(_EarlyFrom, "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, EarlyFrom_) Then
    '        EarlyFrom = Convert.ToDateTime(EarlyFrom_).ToString("yyyy-MM-dd")
    '    Else
    '        EarlyFrom = ConvertDate(_EarlyFrom)
    '    End If

    '    If DateTime.TryParseExact(_EarlyTo, "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, EarlyTo_) Then
    '        EarlyTo = Convert.ToDateTime(EarlyFrom_).ToString("yyyy-MM-dd")
    '    Else
    '        EarlyTo = ConvertDate(_EarlyTo)
    '    End If

    '    If DateTime.TryParseExact(_StandardFrom, "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, StandardFrom_) Then
    '        StandardFrom = Convert.ToDateTime(StandardFrom_).ToString("yyyy-MM-dd")
    '    Else
    '        StandardFrom = ConvertDate(_StandardFrom)
    '    End If

    '    Return (EarlyTo)
    'End Function

    Private Sub LoadTab(ByVal docEntry As Integer, ByVal rangeDate As String)
        Dim _sql As String = "SELECT distinct U_GC_GroupType From GC_QUT1 WHERE DocEntry = '" & docEntry & "'"

        dt = db.getDataTableByQuery(_sql, "GroupType")

        If dt IsNot Nothing Then
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim tp As New TabPanel()
                If dt.Rows(i)(0) Is DBNull.Value Then
                    tp.HeaderText = "NoGroup"
                Else
                    tp.HeaderText = dt.Rows(i)(0)
                End If

                Dim id As String = dt.Rows(i)(0).ToString()
                Dim ctrl1 As Control = Page.LoadControl("~/UserControlQUT1.ascx")    '//user control
                Dim lblID As Label = ctrl1.FindControl("lblID")
                If lblID IsNot Nothing Then
                    lblID.Text = id
                End If
                Dim lblRange As Label = ctrl1.FindControl("lblRange")
                If lblRange IsNot Nothing Then
                    lblRange.Text = id
                End If
                tp.Controls.Add(ctrl1)
                tp.ID = $"TabPanel${i + 1}"
                '"TabPanel" + Convert.ToString(i + 1)
                TabContainer1.Tabs.Add(tp)

            Next

            Dim n As Integer


            n = TabContainer1.Tabs.Count

        Else
            Exit Sub
        End If

    End Sub

    Private Sub ddlProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProject.SelectedIndexChanged
        'If ddlProject.SelectedValue <> -1 Then
        'End If
        'BindData(userID, cardCode)

    End Sub

    Private Function ConvertDate(ByVal _str As String)
        Dim arr() As String
        arr = _str.Split("/")
        Dim Newdate As String = arr(2) & "-" & arr(1) & "-" & arr(0)
        Return CDate(Newdate).ToString("yyyy-MM-dd")
    End Function


    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If PostOrder() Then

        End If
    End Sub

    Private Function PostOrder() As Boolean

        Try
            Dim _sql As String = "	INSERT INTO [dbo].[GC_ORDR]
                               ([DocDate],[DocDueDate],[CardCode],[CardName],[Address],[NumAtCard],[VatSum],[DocTotal],[Doc_Approve]
                               ,[Doc_Date],[Doc_BaseRef],[Doc_Status],[Doc_Remark],[Doc_AppUser],[Doc_AppDate],[DocCur],[DocRate],[ProjectCode]	,[Comment])		
                                VALUES
                               (@DocDate,@DocDueDate,@CardCode ,@CardName,@Address,@NumAtCard,@VatSum,@DocTotal,@Doc_Approve
		                       ,@Doc_Date,@Doc_BaseRef,@Doc_Status,@Doc_Remark,@Doc_AppUser,@Doc_AppDate,@DocCur,@DocRate,@ProjectCode,@Comment)"

            Dim params As SqlParameter()
            params = New SqlParameter() {
                        New SqlParameter("@DocDate", SqlDbType.DateTime) _
                        , New SqlParameter("@DocDueDate", SqlDbType.DateTime) _
                        , New SqlParameter("@CardCode", SqlDbType.NVarChar, 15) _
                        , New SqlParameter("@CardName", SqlDbType.NVarChar, 100) _
                        , New SqlParameter("@Address", SqlDbType.NVarChar, 254) _
                        , New SqlParameter("@NumAtCard", SqlDbType.NVarChar, 100) _
                        , New SqlParameter("@VatSum", SqlDbType.Decimal) _
                        , New SqlParameter("@DocTotal", SqlDbType.Decimal) _
                        , New SqlParameter("@Doc_Approve", SqlDbType.NVarChar, 1) _
                        , New SqlParameter("@Doc_Date", SqlDbType.DateTime) _
                        , New SqlParameter("@Doc_BaseRef", SqlDbType.Int) _
                        , New SqlParameter("@Doc_Status", SqlDbType.NVarChar, 1) _
                        , New SqlParameter("@Doc_Remark", SqlDbType.NVarChar, 250) _
                        , New SqlParameter("@Doc_AppUser", SqlDbType.NVarChar, 50) _
                        , New SqlParameter("@Doc_AppDate", SqlDbType.DateTime) _
                        , New SqlParameter("@DocCur", SqlDbType.NVarChar, 3) _
                        , New SqlParameter("@DocRate", SqlDbType.Decimal) _
                        , New SqlParameter("@ProjectCode", SqlDbType.NVarChar, 15) _
                        , New SqlParameter("@Comment", SqlDbType.NVarChar, 254)
            }

            Dim _Docdate, docDate As DateTime
            If DateTime.TryParseExact(txtDocDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _Docdate) Then
                docDate = Convert.ToDateTime(_Docdate).ToString("yyyy-MM-dd")
            Else
                docDate = ConvertDate(txtDocDate.Text.Trim)
            End If

            Dim _DocDuedate, docDueDate As DateTime
            If DateTime.TryParseExact(txtExpireDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DocDuedate) Then
                docDueDate = Convert.ToDateTime(_DocDuedate).ToString("yyyy-MM-dd")
            Else
                docDueDate = ConvertDate(txtExpireDate.Text.Trim)
            End If

            params(0).Value = docDate
            params(1).Value = docDueDate
            params(2).Value = lblCustID.Text
            params(3).Value = lblCustomerName.Text
            params(4).Value = lblAddress.Text
            params(5).Value = lblProjectCode.Text
            params(6).Value = Convert.ToDecimal(hdTax.Value.ToString())
            params(7).Value = Convert.ToDecimal(hdGrandTotal.Value.ToString())
            params(8).Value = "N"
            params(9).Value = docDate
            params(10).Value = lblDocNum.Text
            params(11).Value = "N"
            params(12).Value = ""
            params(13).Value = ""
            params(14).Value = docDate
            params(15).Value = ""
            params(16).Value = 0
            params(17).Value = ""
            params(18).Value = txtRemark.Text


            Dim docNum As Integer
            docNum = db.executeCommandReturnKey(_sql, params)

            PostRDR1(docNum)

            PostOrder = True
        Catch ex As Exception
            PostOrder = False
        End Try

        Return PostOrder
    End Function

    Private Sub PostRDR1(docNum As Integer)
        'If gvOrder3.Rows.Count > 0 Then
        '    For Each row As GridViewRow In gvOrder3.Rows
        '        Dim qty As TextBox = DirectCast(row.FindControl("txtQty"), TextBox)
        '        Dim slt As LinkButton = DirectCast(row.FindControl("lnkSelect"), LinkButton)
        '        If qty.Text = "" Then
        '            _qty = 0
        '        Else
        '            _qty = Double.Parse(qty.Text)
        '        End If

        '        If _qty > 0 Then
        '            If slt.Text = "SELECT" Then
        '                Try
        '                    Dim _sql As String = "INSERT INTO [dbo].[GC_RDR1]
        '                                           ([DocNum],[ItemCode],[Dscription],[Quantity],[Price],[Currency],[Rate],[WhsCode]
        '                                           ,[PriceBefDi],[DocDate],[OcrCode],[Project],[VatPrcnt]
        '                                     ,[VatGroup],[PriceAfVAT],[VatSum],[TaxCode],[TaxType],[FreeTxt],[unitMsr],[UomCode]
        '                                           ,[FromWhsCod],[Doc_LineDesc],[Doc_SizeColor],[Doc_Status],[Doc_Remark])
        '                                           VALUES
        '                                           (@DocNum,@ItemCode,@Dscription,@Quantity,@Price,@Currency,@Rate,@WhsCode
        '                                           ,@PriceBefDi,@DocDate,@OcrCode,@Project,@VatPrcnt,@VatGroup
        '                                           ,@PriceAfVAT,@VatSum,@TaxCode,@TaxType,@FreeTxt,@unitMsr,@UomCode,@FromWhsCod
        '                                           ,@Doc_LineDesc,@Doc_SizeColor,@Doc_Status,@Doc_Remark)"

        '                    Dim params As SqlParameter()
        '                    params = New SqlParameter() {
        '                                New SqlParameter("@DocNum", SqlDbType.Int) _
        '                                , New SqlParameter("@ItemCode", SqlDbType.NVarChar, 50) _
        '                                , New SqlParameter("@Dscription", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@Quantity", SqlDbType.Decimal) _
        '                                , New SqlParameter("@Price", SqlDbType.Decimal) _
        '                                , New SqlParameter("@Currency", SqlDbType.NVarChar, 3) _
        '                                , New SqlParameter("@Rate", SqlDbType.Decimal) _
        '                                , New SqlParameter("@WhsCode", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@PriceBefDi", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@DocDate", SqlDbType.DateTime) _
        '                                , New SqlParameter("@OcrCode", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@Project", SqlDbType.NVarChar, 20) _
        '                                , New SqlParameter("@VatPrcnt", SqlDbType.Decimal) _
        '                                , New SqlParameter("@VatGroup", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@PriceAfVAT", SqlDbType.Decimal) _
        '                                , New SqlParameter("@VatSum", SqlDbType.Decimal) _
        '                                , New SqlParameter("@TaxCode", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@TaxType", SqlDbType.Char, 1) _
        '                                , New SqlParameter("@FreeTxt", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@unitMsr", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@UomCode", SqlDbType.NVarChar, 20) _
        '                                , New SqlParameter("@FromWhsCod", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@Doc_LineDesc", SqlDbType.NVarChar, 250) _
        '                                , New SqlParameter("@Doc_SizeColor", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@Doc_Status", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@Doc_Remark", SqlDbType.NVarChar, 250)
        '                    }

        '                    Dim _Docdate, docDate As DateTime
        '                    If DateTime.TryParseExact(txtDocDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _Docdate) Then
        '                        docDate = Convert.ToDateTime(_Docdate).ToString("yyyy-MM-dd")
        '                    Else
        '                        docDate = ConvertDate(txtDocDate.Text.Trim)
        '                    End If

        '                    Dim _DocDuedate, docDueDate As DateTime
        '                    If DateTime.TryParseExact(txtExpireDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DocDuedate) Then
        '                        docDueDate = Convert.ToDateTime(_DocDuedate).ToString("yyyy-MM-dd")
        '                    Else
        '                        docDueDate = ConvertDate(txtExpireDate.Text.Trim)
        '                    End If

        '                    params(0).Value = docNum
        '                    params(1).Value = row.Cells(0).Text
        '                    params(2).Value = row.Cells(1).Text
        '                    params(3).Value = _qty
        '                    params(4).Value = Convert.ToDecimal(row.Cells(4).Text.ToString())
        '                    params(5).Value = ""
        '                    params(6).Value = 0
        '                    params(7).Value = ""
        '                    params(8).Value = 0
        '                    params(9).Value = docDate
        '                    params(10).Value = ""
        '                    params(11).Value = ""
        '                    params(12).Value = 0
        '                    params(13).Value = ""
        '                    params(14).Value = 0
        '                    params(15).Value = 0
        '                    params(16).Value = ""
        '                    params(17).Value = ""
        '                    params(18).Value = ""
        '                    params(19).Value = ""
        '                    params(20).Value = ""
        '                    params(21).Value = ""
        '                    params(22).Value = ""
        '                    params(23).Value = ""
        '                    params(24).Value = ""
        '                    params(25).Value = ""

        '                    Dim lineNum As Integer
        '                    lineNum = db.executeCommandReturnKey(_sql, params, "XCON_GCConnectionString")

        '                    Dim params3 As SqlParameter()
        '                    params3 = New SqlParameter() {
        '                                New SqlParameter("@DocNum", SqlDbType.Int) _
        '                                , New SqlParameter("@LineNum", SqlDbType.Int) _
        '                                , New SqlParameter("@Doc_Size", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@Doc_Color", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@Quantity", SqlDbType.Decimal) _
        '                                , New SqlParameter("@Doc_Label", SqlDbType.NVarChar, 50) _
        '                                , New SqlParameter("@Doc_Remark1", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@Doc_Remark2", SqlDbType.NVarChar, 100)
        '                    }

        '                    params3(0).Value = docNum
        '                    params3(1).Value = lineNum
        '                    params3(2).Value = row.Cells(9).Text
        '                    params3(3).Value = row.Cells(10).Text
        '                    params3(4).Value = 0
        '                    params3(5).Value = row.Cells(8).Text
        '                    params3(6).Value = row.Cells(11).Text
        '                    params3(7).Value = row.Cells(12).Text

        '                    db.executeCommandStore("POST_RDR2", params3, "XCON_GCConnectionString")


        '                Catch ex As Exception

        '                End Try
        '            Else
        '                Try
        '                    Dim params As SqlParameter()
        '                    params = New SqlParameter() {
        '                                New SqlParameter("@DocNum", SqlDbType.Int) _
        '                                , New SqlParameter("@ItemCode", SqlDbType.NVarChar, 50) _
        '                                , New SqlParameter("@Dscription", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@Quantity", SqlDbType.Decimal) _
        '                                , New SqlParameter("@Price", SqlDbType.Decimal) _
        '                                , New SqlParameter("@Currency", SqlDbType.NVarChar, 3) _
        '                                , New SqlParameter("@Rate", SqlDbType.Decimal) _
        '                                , New SqlParameter("@WhsCode", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@PriceBefDi", SqlDbType.Decimal) _
        '                                , New SqlParameter("@DocDate", SqlDbType.DateTime) _
        '                                , New SqlParameter("@OcrCode", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@Project", SqlDbType.NVarChar, 20) _
        '                                , New SqlParameter("@VatPrcnt", SqlDbType.Decimal) _
        '                                , New SqlParameter("@VatGroup", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@PriceAfVAT", SqlDbType.Decimal) _
        '                                , New SqlParameter("@VatSum", SqlDbType.Decimal) _
        '                                , New SqlParameter("@TaxCode", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@TaxType", SqlDbType.Char, 1) _
        '                                , New SqlParameter("@FreeTxt", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@unitMsr", SqlDbType.NVarChar, 100) _
        '                                , New SqlParameter("@UomCode", SqlDbType.NVarChar, 20) _
        '                                , New SqlParameter("@FromWhsCod", SqlDbType.NVarChar, 8) _
        '                                , New SqlParameter("@Doc_LineDesc", SqlDbType.NVarChar, 250) _
        '                                , New SqlParameter("@Doc_SizeColor", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@Doc_Status", SqlDbType.NVarChar, 1) _
        '                                , New SqlParameter("@Doc_Remark", SqlDbType.NVarChar, 250)
        '                    }

        '                    Dim _Docdate, docDate As DateTime
        '                    If DateTime.TryParseExact(txtDocDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _Docdate) Then
        '                        docDate = Convert.ToDateTime(_Docdate).ToString("yyyy-MM-dd")
        '                    Else
        '                        docDate = ConvertDate(txtDocDate.Text.Trim)
        '                    End If

        '                    Dim _DocDuedate, docDueDate As DateTime
        '                    If DateTime.TryParseExact(txtExpireDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DocDuedate) Then
        '                        docDueDate = Convert.ToDateTime(_DocDuedate).ToString("yyyy-MM-dd")
        '                    Else
        '                        docDueDate = ConvertDate(txtExpireDate.Text.Trim)
        '                    End If

        '                    params(0).Value = docNum
        '                    params(1).Value = row.Cells(0).Text
        '                    params(2).Value = row.Cells(1).Text
        '                    params(3).Value = _qty
        '                    params(4).Value = Convert.ToDecimal(row.Cells(4).Text.ToString())
        '                    params(5).Value = ""
        '                    params(6).Value = 0
        '                    params(7).Value = ""
        '                    params(8).Value = 0
        '                    params(9).Value = docDate
        '                    params(10).Value = ""
        '                    params(11).Value = ""
        '                    params(12).Value = 0
        '                    params(13).Value = ""
        '                    params(14).Value = 0
        '                    params(15).Value = 0
        '                    params(16).Value = ""
        '                    params(17).Value = ""
        '                    params(18).Value = ""
        '                    params(19).Value = ""
        '                    params(20).Value = ""
        '                    params(21).Value = ""
        '                    params(22).Value = ""
        '                    params(23).Value = ""
        '                    params(24).Value = ""
        '                    params(25).Value = ""

        '                    db.executeCommandStore("POST_RDR1", params, "XCON_GCConnectionString")

        '                Catch ex As Exception

        '                End Try
        '            End If
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub ExecuteRDR1_GV1(ByVal docNum As Integer, ByVal row As Integer)


    End Sub
    Private Sub ExecuteRDR2()
        Try

        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        BindData(lblUserID.Text, lblCustID.Text)
    End Sub

    Private Sub gvOrder3_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        lblGvorder.Text = ""
        ClearMPE()
        If e.CommandName.ToUpper = "SELECT" Then
            Dim gvCurrentRow As GridViewRow = e.CommandSource.Parent.Parent
            lblItemID.Text = gvCurrentRow.Cells(0).Text.ToString()
            lblItemName.Text = gvCurrentRow.Cells(1).Text.ToString()
            lblGvorder.Text = "gvOrder3"
            lblGvRowIndex.Text = gvCurrentRow.RowIndex

            If gvCurrentRow.Cells(6).Text.ToString() = "N" And gvCurrentRow.Cells(7).Text.ToString() = "N" Then
                txtSize.Enabled = False
                txtColor.Enabled = False
            ElseIf gvCurrentRow.Cells(6).Text.ToString() = "N" And gvCurrentRow.Cells(7).Text.ToString() = "Y" Then
                txtSize.Enabled = False
                txtColor.Enabled = True
            ElseIf gvCurrentRow.Cells(6).Text.ToString() = "Y" And gvCurrentRow.Cells(7).Text.ToString() = "N" Then
                txtSize.Enabled = True
                txtColor.Enabled = False
            Else
                txtSize.Enabled = True
                txtColor.Enabled = True
            End If

            'mpeSelect.Show()

            Dim myLinkButton As LinkButton
            myLinkButton = DirectCast(gvCurrentRow.Cells(4).FindControl("lnkSelect"), LinkButton)
            myLinkButton.Attributes.Add("onclick", "showModalPopup('" + gvCurrentRow.Cells(0).Text + "', '" + gvCurrentRow.Cells(0).Text + "');return false;")
        End If
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ClearMPE()
        mpeSelect.Hide()
    End Sub

    Private Sub ClearMPE()
        txtLabelName.Text = String.Empty
        txtColor.Text = String.Empty
        txtSize.Text = String.Empty
        txtRem1.Text = String.Empty
        txtRem2.Text = String.Empty
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'Select Case lblGvorder.Text
        '    Case "gvOrder1"
        '        gvOrder1.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(8).Text = txtLabelName.Text
        '        gvOrder1.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(9).Text = txtColor.Text
        '        gvOrder1.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(10).Text = txtSize.Text
        '        gvOrder1.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(11).Text = txtRem1.Text
        '        gvOrder1.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(12).Text = txtRem2.Text
        '    Case "gvOrder2"
        '        gvOrder2.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(8).Text = txtLabelName.Text
        '        gvOrder2.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(9).Text = txtColor.Text
        '        gvOrder2.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(10).Text = txtSize.Text
        '        gvOrder2.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(11).Text = txtRem1.Text
        '        gvOrder2.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(12).Text = txtRem2.Text
        '    Case "gvOrder3"
        '        gvOrder3.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(8).Text = txtLabelName.Text
        '        gvOrder3.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(9).Text = txtColor.Text
        '        gvOrder3.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(10).Text = txtSize.Text
        '        gvOrder3.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(11).Text = txtRem1.Text
        '        gvOrder3.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(12).Text = txtRem2.Text
        '    Case "gvOrder4"
        '        gvOrder4.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(8).Text = txtLabelName.Text
        '        gvOrder4.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(9).Text = txtColor.Text
        '        gvOrder4.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(10).Text = txtSize.Text
        '        gvOrder4.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(11).Text = txtRem1.Text
        '        gvOrder4.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(12).Text = txtRem2.Text
        '    Case "gvOrder5"
        '        gvOrder5.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(8).Text = txtLabelName.Text
        '        gvOrder5.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(9).Text = txtColor.Text
        '        gvOrder5.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(10).Text = txtSize.Text
        '        gvOrder5.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(11).Text = txtRem1.Text
        '        gvOrder5.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(12).Text = txtRem2.Text
        '    Case "gvOrder6"
        '        gvOrder6.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(8).Text = txtLabelName.Text
        '        gvOrder6.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(9).Text = txtColor.Text
        '        gvOrder6.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(10).Text = txtSize.Text
        '        gvOrder6.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(11).Text = txtRem1.Text
        '        gvOrder6.Rows(Convert.ToInt32(lblGvRowIndex.Text)).Cells(12).Text = txtRem2.Text
        'End Select

        mpeSelect.Hide()
    End Sub


    'Protected Sub txtQty_TextChanged(sender As Object, e As EventArgs)
    '    Dim currentRow As GridViewRow = DirectCast(DirectCast(sender, TextBox).Parent.Parent, GridViewRow)
    '    Dim qty As TextBox = DirectCast(currentRow.FindControl("txtQty"), TextBox)
    '    Dim totalprice As Label = DirectCast(currentRow.FindControl("lblTotal"), Label)

    '    totalprice.Text = Double.Parse(qty.Text) * Double.Parse(currentRow.Cells(4).Text)

    'End Sub

End Class
