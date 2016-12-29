Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class MasterItem_LoadQuotation
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")

    Private Sub MasterItem_LoadQuotation_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If LoadQuotation() Then
            Get_XCONQuotation()
        Else

        End If
    End Sub

    Private Function LoadQuotation() As Boolean
        Dim DueDateFrom, _DateFrom, _DateTo, DueDateTo As DateTime

        If DateTime.TryParseExact(txtValidFrom.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DateFrom) Then
            DueDateFrom = Convert.ToDateTime(_DateFrom).ToString("yyyy-MM-dd")
        Else
            DueDateFrom = ConvertDate(txtValidFrom.Text.Trim)
        End If

        If DateTime.TryParseExact(txtValidTo.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DateTo) Then
            DueDateTo = Convert.ToDateTime(_DateTo).ToString("yyyy-MM-dd")
        Else
            DueDateTo = ConvertDate(txtValidTo.Text.Trim)
        End If

        Try
            Dim params As SqlParameter()
            params = New SqlParameter() {
                                New SqlParameter("@DueDateFrom", SqlDbType.DateTime) _
                                , New SqlParameter("@DueDateTo", SqlDbType.DateTime) _
                                , New SqlParameter("@DocnumFrom", SqlDbType.Int) _
                                , New SqlParameter("@DocnumTo", SqlDbType.Int)
                    }


            params(0).Value = DueDateFrom
            params(1).Value = DueDateTo
            params(2).Value = Convert.ToInt32(txtQuotFrom.Text)
            params(3).Value = Convert.ToInt32(txtQuotTo.Text)

            If db.executeCommandStore("UPSERT_OQUT", params, "XCON_GCConnectionString") Then

                Dim params2 As SqlParameter()
                params2 = New SqlParameter() {
                                New SqlParameter("@DueDateFrom", SqlDbType.DateTime) _
                                , New SqlParameter("@DueDateTo", SqlDbType.DateTime) _
                                , New SqlParameter("@DocnumFrom", SqlDbType.Int) _
                                , New SqlParameter("@DocnumTo", SqlDbType.Int)
                    }


                params2(0).Value = DueDateFrom
                params2(1).Value = DueDateTo
                params2(2).Value = Convert.ToInt32(txtQuotFrom.Text)
                params2(3).Value = Convert.ToInt32(txtQuotTo.Text)

                db.executeCommandStore("UPSERT_QUT1", params2, "XCON_GCConnectionString")

                LoadQuotation = True
            Else
                LoadQuotation = False
            End If

        Catch ex As Exception
            LoadQuotation = False
        End Try

    End Function

    Private Sub Get_XCONQuotation()
        Dim dt As DataTable
        Dim DueDateFrom, _DateFrom, _DateTo, DueDateTo As DateTime
        Dim DocNumFrom, DocNumTo As Integer

        If DateTime.TryParseExact(txtValidFrom.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DateFrom) Then
            DueDateFrom = Convert.ToDateTime(_DateFrom).ToString("yyyy-MM-dd")
        Else
            DueDateFrom = ConvertDate(txtValidFrom.Text.Trim)
        End If

        If DateTime.TryParseExact(txtValidTo.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _DateTo) Then
            DueDateTo = Convert.ToDateTime(_DateTo).ToString("yyyy-MM-dd")
        Else
            DueDateTo = ConvertDate(txtValidTo.Text.Trim)
        End If

        DocNumFrom = Convert.ToInt32(txtQuotFrom.Text)
        DocNumTo = Convert.ToInt32(txtQuotTo.Text)

        Try
            LoadQuotationSqlDataSource.SelectCommand = "SELECT T0.DocEntry,T0.DocNum,T0.DocStatus,T0.DocDate,T0.DocDueDate,T0.CardCode,T0.CardName,T0.Address" +
                                ", T0.NumAtCard,T0.Comments,T0.Project,T0.U_MainProject From GC_OQUT T0" +
                                " WHERE" +
                                " T0.DocDueDate >= '" & DueDateFrom & "' AND T0.DocDueDate <= '" & DueDateTo & "'" +
                                " AND T0.DocNum >= '" & DocNumFrom & "' AND T0.DocNum <= '" & DocNumTo & "'"

            gvCustomer.DataBind()

            'dt = db.getDataTableByQuery("_sql", "Get_XCONQuotation")

            'If dt.Rows.Count > 0 Then
            '    gvCustomer.DataSource = dt
            '    gvCustomer.DataBind()
            'Else

            'End If

        Catch ex As Exception

        End Try

    End Sub

    Private Function Post_XCONQuotation() As Boolean

        Try

            Post_XCONQuotation = True
        Catch ex As Exception
            Post_XCONQuotation = False
        End Try

        Return Post_XCONQuotation
    End Function

    Private Function ConvertDate(ByVal _str As String)
        Dim arr() As String
        arr = _str.Split("/")
        Dim Newdate As String = arr(2) & "-" & arr(1) & "-" & arr(0)
        Return CDate(Newdate).ToString("yyyy-MM-dd")
    End Function
End Class
