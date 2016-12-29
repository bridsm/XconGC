Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class MasterItem_gcItemMaster
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")
    Dim dt As Datatable
    Private Sub MasterItem_gcItemMaster_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub ddlProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProject.SelectedIndexChanged
        If ddlProject.SelectedValue <> -1 Then
            GetExpireDays(Convert.ToInt32(ddlProject.SelectedValue.ToString()))

            lblDocEntry.Text = ddlProject.SelectedValue
        Else
            lblExpireDays.Text = String.Empty
        End If
    End Sub

    Private Sub GetExpireDays(ByVal docEntry As Integer)
        Dim docDueDate, _docDueDate As DateTime
        Dim _sql As String = "SELECT [U_ExpireDay]" +
                                " , IIF([U_OnSiteToDate] Is NULL, Format(DocDueDate,'dd/MM/yyyy') ,FORMAT([U_OnSiteToDate], 'dd/MM/yyyy')) as DocDueDate" +
                                " , IIF([U_OnSiteFromDate] IS NOT NULL, FORMAT([U_EarlyFromDate], 'dd/MM/yyyy'), NULL) as [EarlyFromDate]" +
                                " , IIF([U_EarlyToDate] Is Not NULL, FORMAT([U_EarlyToDate], 'dd/MM/yyyy'), NULL) as [EarlyToDate]" +
                                " , IIF([U_StandardFromDate] IS NOT NULL, FORMAT([U_StandardFromDate], 'dd/MM/yyyy'), NULL) as [StandardFromDate]" +
                                " , IIF([U_StandardToDate] Is Not NULL, FORMAT([U_StandardToDate], 'dd/MM/yyyy'), NULL) as [StandardToDate]" +
                                " , IIF([U_OnSiteFromDate] IS NOT NULL, FORMAT([U_OnSiteFromDate], 'dd/MM/yyyy'), NULL) as [OnSiteFromDate]" +
                                " , IIF([U_OnSiteToDate] IS NOT NULL, FORMAT([U_OnSiteToDate], 'dd/MM/yyyy'), NULL) as [OnSiteToDate]" +
                                " , IIF(U_OnSiteDisc IS NOT NULL, FORMAT(U_OnSiteDisc,'N2'), NULL) as U_OnSiteDisc" +
                                " , IIF(U_StandardDisc IS NOT NULL, FORMAT(U_StandardDisc, 'N2'), NULL) as U_StandardDisc" +
                                " , IIF(U_EarlyDisc IS NOT NULL, FORMAT(U_EarlyDisc, 'N2'), NULL) as U_EarlyDisc" +
                                " FROM [dbo].[GC_OQUT] WHERE DocEntry = '" & docEntry & "'"

        dt = db.getDataTableByQuery(_sql, "GetExpireDays")

        If dt.Rows.Count > 0 Then
            If DateTime.TryParseExact(dt.Rows(0).Item("DocDueDate").ToString(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _docDueDate) Then
                docDueDate = _docDueDate
            Else
                docDueDate = ConvertDate(_docDueDate)

            End If

            'docDueDate = Convert.ToDateTime(dt.Rows(0).Item("DocDueDate")).ToString("dd/MM/yyyy")
            lblExpireDays.Text = dt.Rows(0).Item("U_ExpireDay").ToString()
            txtEOnsite.Text = Convert.ToString(docDueDate.ToString("dd-MM-yyyy"))

            txtDOnsite.Text = dt.Rows(0).Item("U_OnSiteDisc").ToString()
            txtDStandard.Text = dt.Rows(0).Item("U_StandardDisc").ToString()
            txtDEarly.Text = dt.Rows(0).Item("U_EarlyDisc").ToString()

            SetStartEndDate(docDueDate)

        End If
    End Sub


    Private Function SetStartEndDate(ByVal docDueDate As DateTime) As Boolean
        Dim sOnsite, eStandard, sStandard, eEarly, sEarly As DateTime

        sOnsite = docDueDate.AddDays(-7)
        eStandard = sOnsite.AddDays(-1)
        sStandard = eStandard.AddDays(-7)
        eEarly = sStandard.AddDays(-1)
        sEarly = eEarly.AddDays(-7)

        txtSOnsite.Text = Format(sOnsite, "dd-MM-yyyy")
        txtEStandard.Text = Format(eStandard, "dd-MM-yyyy")
        txtSStandard.Text = Format(sStandard, "dd-MM-yyyy")
        txtEEarly.Text = Format(eEarly, "dd-MM-yyyy")
        txtSEarly.Text = Format(sEarly, "dd-MM-yyyy")

        Return True
    End Function

    Private Function ConvertDate(ByVal _str As String)
        Dim arr() As String
        arr = _str.Split("/")
        Dim Newdate As String = arr(2) & "-" & arr(1) & "-" & arr(0)
        Return CDate(Newdate).ToString("yyyy-MM-dd")
    End Function
    Protected Sub txtSOnsite_TextChanged(sender As Object, e As EventArgs) Handles txtSOnsite.TextChanged
        Dim sOnsite, _sOnsite, eStandard As DateTime

        If DateTime.TryParseExact(txtSOnsite.Text.Trim, "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _sOnsite) Then
            sOnsite = _sOnsite
        Else
            sOnsite = ConvertDate(txtSOnsite.Text.Trim)
        End If

        eStandard = sOnsite.AddDays(-1)
        txtEStandard.Text = Format(eStandard, "dd-MM-yyyy")
    End Sub
    Protected Sub txtSStandard_TextChanged(sender As Object, e As EventArgs) Handles txtSStandard.TextChanged
        Dim sStandard, _sStandard, eEarly As DateTime

        If DateTime.TryParseExact(txtSStandard.Text.Trim, "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _sStandard) Then
            sStandard = _sStandard
        Else
            sStandard = ConvertDate(txtSStandard.Text.Trim)
        End If

        eEarly = sStandard.AddDays(-1)
        txtEEarly.Text = Format(eEarly, "dd-MM-yyyy")
    End Sub
    Protected Sub btnCalulate_Click(sender As Object, e As EventArgs) Handles btnCalulate.Click
        If CalculatePrice() Then

        End If
    End Sub

    Private Function CalculatePrice() As Boolean
        Dim _price, _price0, _price1, _price2 As Decimal

        If txtDOnsite.Text = "" Then txtDOnsite.Text = 0

        If txtDStandard.Text = "" Then txtDStandard.Text = 0

        If txtDEarly.Text = "" Then txtDEarly.Text = 0


        If gvUQT1.Rows.Count > 0 Then

            'For i As Integer = 0 To gvUQT1.Rows.Count - 1
            '    _price = Convert.ToDecimal(gvUQT1.Rows(i).Cells(3).Text.ToString())
            '    _price1 = _price - (_price * Convert.ToInt32(txtDStandard.Text))
            '    _price2 = _price - (_price * Convert.ToInt32(txtDEarly.Text))

            '    gvUQT1.Rows(i).Cells(4).Text = _price1
            '    gvUQT1.Rows(i).Cells(5).Text = _price2

            'Next

            For Each row As GridViewRow In gvUQT1.Rows
                Dim txtprice As TextBox = DirectCast(row.FindControl("txtPrice"), TextBox)
                Dim txtprice1 As TextBox = DirectCast(row.FindControl("txtPrice1"), TextBox)
                Dim txtprice2 As TextBox = DirectCast(row.FindControl("txtPrice2"), TextBox)

                '_price = IIf(txt.Text = String.Empty, 0.00, Convert.ToDecimal(txt.Text))
                If row.Cells(11).Text = String.Empty Or row.Cells(11).Text = 0 Then
                    _price = 0.00
                Else
                    _price = Convert.ToDecimal(row.Cells(11).Text)
                End If

                _price0 = _price - (_price * (Convert.ToDecimal(txtDOnsite.Text) / 100))
                _price1 = _price - (_price * (Convert.ToDecimal(txtDStandard.Text) / 100))
                _price2 = _price - (_price * (Convert.ToDecimal(txtDEarly.Text) / 100))


                txtprice.Text = Format(_price0, "N2")
                txtprice1.Text = Format(_price1, "N2")
                txtprice2.Text = Format(_price2, "N2")

            Next


            Return CalculatePrice = True
        Else
            Return CalculatePrice = False
        End If

        Return CalculatePrice
    End Function

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If PutGcItemMaster() Then
            If PutQUT1() Then
                'GetExpireDays(Convert.ToInt32(lblDocEntry.Text))
                gvUQT1.DataBind()
            End If
        Else

        End If
    End Sub

    Private Function PutGcItemMaster() As Boolean
        Try

            Dim params As SqlParameter()
            params = New SqlParameter() {
                                    New SqlParameter("@U_EarlyFromDate", SqlDbType.DateTime) _
                                    , New SqlParameter("@U_EarlyToDate", SqlDbType.DateTime) _
                                    , New SqlParameter("@U_StandardFromDate", SqlDbType.DateTime) _
                                    , New SqlParameter("@U_StandardToDate", SqlDbType.DateTime) _
                                    , New SqlParameter("@U_OnSiteFromDate", SqlDbType.DateTime) _
                                    , New SqlParameter("@U_OnSiteToDate", SqlDbType.DateTime) _
                                    , New SqlParameter("@U_EarlyDisc", SqlDbType.Decimal) _
                                    , New SqlParameter("@U_StandardDisc", SqlDbType.Decimal) _
                                    , New SqlParameter("@U_OnSiteDisc", SqlDbType.Decimal) _
                                    , New SqlParameter("@DocEntry", SqlDbType.Int)
                        }

            Dim _EarlyFromDate, EarlyFromDate As DateTime
            If DateTime.TryParseExact(txtSEarly.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _EarlyFromDate) Then
                EarlyFromDate = Convert.ToDateTime(_EarlyFromDate).ToString("yyyy-MM-dd")
            Else
                EarlyFromDate = ConvertDate(txtSEarly.Text.Trim)
            End If

            Dim _EarlyToDate, EarlyToDate As DateTime
            If DateTime.TryParseExact(txtEEarly.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _EarlyToDate) Then
                EarlyToDate = Convert.ToDateTime(_EarlyToDate).ToString("yyyy-MM-dd")
            Else
                EarlyToDate = ConvertDate(txtEEarly.Text.Trim)
            End If

            Dim _StandardFromDate, StandardFromDate As DateTime
            If DateTime.TryParseExact(txtSStandard.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _StandardFromDate) Then
                StandardFromDate = Convert.ToDateTime(_StandardFromDate).ToString("yyyy-MM-dd")
            Else
                StandardFromDate = ConvertDate(txtSStandard.Text.Trim)
            End If

            Dim _StandardToDate, StandardToDate As DateTime
            If DateTime.TryParseExact(txtEStandard.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _StandardToDate) Then
                StandardToDate = Convert.ToDateTime(_StandardToDate).ToString("yyyy-MM-dd")
            Else
                StandardToDate = ConvertDate(txtEStandard.Text.Trim)
            End If

            Dim _OnSiteFromDate, OnSiteFromDate As DateTime
            If DateTime.TryParseExact(txtSOnsite.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _OnSiteFromDate) Then
                OnSiteFromDate = Convert.ToDateTime(_OnSiteFromDate).ToString("yyyy-MM-dd")
            Else
                OnSiteFromDate = ConvertDate(txtSOnsite.Text.Trim)
            End If

            Dim _OnSiteToDate, OnSiteToDate As DateTime
            If DateTime.TryParseExact(txtEOnsite.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _OnSiteToDate) Then
                OnSiteToDate = Convert.ToDateTime(_OnSiteToDate).ToString("yyyy-MM-dd")
            Else
                OnSiteToDate = ConvertDate(txtEOnsite.Text.Trim)
            End If

            params(0).Value = EarlyFromDate
            params(1).Value = EarlyToDate
            params(2).Value = StandardFromDate
            params(3).Value = StandardToDate
            params(4).Value = OnSiteFromDate
            params(5).Value = OnSiteToDate
            params(6).Value = Convert.ToDecimal(txtDEarly.Text)
            params(7).Value = Convert.ToDecimal(txtDStandard.Text)
            params(8).Value = Convert.ToDecimal(txtDOnsite.Text)
            params(9).Value = Convert.ToInt32(ddlProject.SelectedValue)

            db.executeCommandStore("PUT_OQUT", params, "XCON_GCConnectionString")

            PutGcItemMaster = True

        Catch ex As Exception
            PutGcItemMaster = False
        End Try

        Return PutGcItemMaster
    End Function

    Private Function PutQUT1() As Boolean
        Try
            For Each row As GridViewRow In gvUQT1.Rows
                Dim txtprice As TextBox = DirectCast(row.FindControl("txtPrice"), TextBox)
                Dim txtprice1 As TextBox = DirectCast(row.FindControl("txtPrice1"), TextBox)
                Dim txtprice2 As TextBox = DirectCast(row.FindControl("txtPrice2"), TextBox)
                Dim _size As DropDownList = DirectCast(row.FindControl("ddlSize"), DropDownList)
                Dim _color As DropDownList = DirectCast(row.FindControl("ddlColor"), DropDownList)

                Dim params As SqlParameter()
                params = New SqlParameter() {
                                    New SqlParameter("@U_EarlyPrice", SqlDbType.Decimal) _
                                    , New SqlParameter("@U_StandardPrice", SqlDbType.Decimal) _
                                    , New SqlParameter("@U_OnSitePrice", SqlDbType.Decimal) _
                                    , New SqlParameter("@U_Size", SqlDbType.NVarChar, 1) _
                                    , New SqlParameter("@U_Color", SqlDbType.NVarChar, 1) _
                                    , New SqlParameter("@DocEntry", SqlDbType.Int) _
                                    , New SqlParameter("@LineNum", SqlDbType.Int)
                        }

                params(0).Value = Convert.ToDecimal(txtprice2.Text)
                params(1).Value = Convert.ToDecimal(txtprice1.Text)
                params(2).Value = Convert.ToDecimal(txtprice.Text)
                params(3).Value = _size.SelectedValue
                params(4).Value = _color.SelectedValue
                params(5).Value = Convert.ToInt32(row.Cells(9).Text.ToString())     '// DocEntry
                params(6).Value = Convert.ToInt32(row.Cells(10).Text.ToString())    '// LineNum

                db.executeCommandStore("PUT_QUT1", params, "XCON_GCConnectionString")

            Next

            PutQUT1 = True
        Catch ex As Exception
            PutQUT1 = False
        End Try

        Return PutQUT1
    End Function

End Class
