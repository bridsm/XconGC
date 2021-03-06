﻿Imports System.Data
Imports System.Data.SqlClient
Imports AjaxControlToolkit
Imports System.Globalization
Imports System.IO

Partial Class gcOrder
    Inherits System.Web.UI.Page
    Implements ICallbackEventHandler

    Dim db As New DataManager("XCON_GCConnectionString")
    Dim dt, dtbind As DataTable
    Dim userID, cardCode, projectCode As String

    Private Sub gcOrder_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Session("UserID") Is Nothing = True Then
        '    Response.Redirect("login.aspx")
        'End If

        cardCode = Session("CardCode")
        projectCode = Session("ProjectCode")
        lblCustID.Text = "C005" 'Session("CardCode")
        lblUserID.Text = "C05" 'Session("UserID")

        If Page.IsPostBack = False Then


        Else

        End If


        If Page.IsCallback = False Then

            Page.ClientScript.GetCallbackEventReference(Me, "", "", "")

            If Page.ClientScript.IsClientScriptIncludeRegistered("JQuery") = False Then
                Page.ClientScript.RegisterClientScriptInclude("JQuery", Page.ResolveUrl("~/Scripts/jquery-1.10.2.js"))
            End If

            If Page.ClientScript.IsClientScriptIncludeRegistered("Default") = False Then
                Page.ClientScript.RegisterClientScriptInclude("Default", Page.ResolveUrl("~/Scripts/xconapp.js"))
            End If


        End If



        'Dim client = ScriptManager.GetCurrent(Page)
        'client.RegisterAsyncPostBackControl(btnLoad)
        'client.RegisterAsyncPostBackControl(btnUpdate)
        'client.RegisterAsyncPostBackControl(btnCancel)

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
        End If

    End Sub

    Private Sub LoadTab(ByVal docEntry As Integer, ByVal rangeDate As String)
        Dim _sql As String = "SELECT distinct U_GC_GroupType From GC_QUT1 WHERE DocEntry = '" & docEntry & "'"

        dt = db.getDataTableByQuery(_sql, "GroupType")

        If dt IsNot Nothing Then
            If TabContainer1.Tabs.Count > 1 Then
                TabContainer1.Tabs.Clear()
            End If
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
                tp.ID = $"TabPanel{i + 1}"
                TabContainer1.Tabs.Add(tp)

            Next

            Dim n As Integer
            n = TabContainer1.Tabs.Count

        Else
            Exit Sub
        End If

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

    Public Shared Sub updateItem(doii As Object)
        Dim birdNood As String
        birdNood = "Doii++"
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


    End Sub

    Protected Sub AsyncFileUpload_UploadedComplete(sender As Object, e As AsyncFileUploadEventArgs) Handles _
        AsyncFileUpload1.UploadedComplete _
        , AsyncFileUpload2.UploadedComplete _
        , AsyncFileUpload3.UploadedComplete _
        , AsyncFileUpload4.UploadedComplete _
        , AsyncFileUpload5.UploadedComplete

        Dim context As AsyncFileUpload = DirectCast(sender, AsyncFileUpload)
        If context IsNot Nothing Then

            Dim slot As Integer = 5

            If context.ClientID.Contains("AsyncFileUpload1") Then
                slot = 1
            ElseIf context.ClientID.Contains("AsyncFileUpload2") Then
                slot = 2
            ElseIf context.ClientID.Contains("AsyncFileUpload3") Then
                slot = 3
            ElseIf context.ClientID.Contains("AsyncFileUpload4") Then
                slot = 4
            End If

            FileUtils.SaveOrderFile(hidItemCode.Value, slot, context.PostedFile)

        End If

    End Sub

    Public Sub RaiseCallbackEvent(eventArgument As String) Implements ICallbackEventHandler.RaiseCallbackEvent
        If eventArgument = "UPDATE_ITEM" Then

        End If
    End Sub

    Public Function GetCallbackResult() As String Implements ICallbackEventHandler.GetCallbackResult
        Return "Return from Nood world"
    End Function
End Class
