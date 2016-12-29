Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class PaymentConfirm
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")

    Private Sub PaymentConfirm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing = True Then
            Response.Redirect("login.aspx")
        End If

        Dim userID, cardCode, projectCode As String
        userID = Session("UserID")
        cardCode = Session("CardCode")
        projectCode = Session("ProjectCode")

        If Page.IsPostBack = False Then

            If GetOREC(cardCode, projectCode) Then
                lblUpsertStatus.Text = "U"
            Else
                lblUpsertStatus.Text = "I"
                BindData(userID, cardCode)
            End If

        End If

    End Sub

    Private Function GetOREC(ByVal cardcode As String, ByVal projectCode As String) As Boolean
        Dim _sql As String = "SELECT T0.docnum,T0.LineNum, T0.DocStatus, FORMAT(T0.DocDate, 'dd/MM/yyyy') AS DocDate, T0.CardCode, T0.CardName, T0.TypePay, T0.Bank
                                , T0.Amount, T0.Project, T0.U_MainProject, T0.AttachFile, T1.CardName AS ProjectName
                                FROM GC_OREC T0 INNER JOIN GC_OQUT T1 ON T0.Project = T1.CardCode
                                WHERE T0.CardCode = '" & cardcode & "' AND T0.Project = '" & projectCode & "'"

        Dim dt As DataTable = db.getDataTableByQuery(_sql, "GET_OREC", "XCON_GCConnectionString")

        If dt.Rows.Count > 0 Then
            lblDocnum.Text = dt.Rows(0).Item("DocNum")
            lblCustID.Text = dt.Rows(0).Item("CardCode")
            lblCustName.Text = dt.Rows(0).Item("CardName")
            lblProject.Text = dt.Rows(0).Item("ProjectName")
            lblProjectCode.Text = dt.Rows(0).Item("Project")
            txtPaymentType.Text = dt.Rows(0).Item("TypePay")
            txtBank.Text = dt.Rows(0).Item("Bank")
            txtPaymentDate.Text = dt.Rows(0).Item("DocDate")
            txtAmount.Text = dt.Rows(0).Item("Amount")

            GetOREC = True
        Else
            GetOREC = False
        End If

        Return GetOREC
    End Function

    Private Function BindData(ByVal userID As String, ByVal cardCode As String) As Boolean
        Dim _sql As String = "SELECT   T1.CardCode, T1.CardName , T0.CardName As ProjectName, T0.CardCode AS ProjectCode
                                FROM    GC_OQUT T0 INNER JOIN GC_USER T1 ON T0.CardCode = T1.ProjectCode
                                WHERE T1.UserID ='" & userID & "' AND T1.CardCode = '" & cardCode & "'"

        Dim dt As DataTable

        Try
            dt = db.getDataTableByQuery(_sql, "GetData")

            If dt.Rows.Count > 0 Then
                lblCustID.Text = dt.Rows(0)(0)
                lblCustName.Text = "   " + dt.Rows(0)(1)
                lblProject.Text = dt.Rows(0)(2)
                lblProjectCode.Text = dt.Rows(0).Item("ProjectCode")
            End If

            BindData = True
        Catch ex As Exception
            BindData = False
        End Try

        Return BindData
    End Function
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If lblUpsertStatus.Text = "I" Then
            If PostPayment() Then
                lblUpsertStatus.Text = "U"
                'GetOREC(lblCustID.Text, lblProjectCode.Text)
            End If
        Else
            If PutPayment() Then

            End If
        End If
    End Sub

    Private Function PostPayment() As Boolean
        Dim FileName As String = Path.GetFileName(fldAttach.PostedFile.FileName)
        Dim Extension As String = Path.GetExtension(fldAttach.PostedFile.FileName)
        Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

        Try
            Dim params As SqlParameter()
            params = New SqlParameter() {
                        New SqlParameter("@LineNum", SqlDbType.Int) _
                        , New SqlParameter("@DocStatus", SqlDbType.Char, 1) _
                        , New SqlParameter("@DocDate", SqlDbType.DateTime) _
                        , New SqlParameter("@CardCode", SqlDbType.NVarChar, 15) _
                        , New SqlParameter("@CardName", SqlDbType.NVarChar, 100) _
                        , New SqlParameter("@TypePay", SqlDbType.NVarChar, 100) _
                        , New SqlParameter("@Bank", SqlDbType.NVarChar, 254) _
                        , New SqlParameter("@Amount", SqlDbType.Decimal) _
                        , New SqlParameter("@Project", SqlDbType.NVarChar, 20) _
                        , New SqlParameter("@U_MainProject", SqlDbType.NVarChar, 20) _
                        , New SqlParameter("@AttachFile", SqlDbType.NVarChar, 50)
            }

            'Dim _Paydate, Paydate As DateTime
            'If DateTime.TryParseExact(txtPaymentDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _Paydate) Then
            '    Paydate = Convert.ToDateTime(_Paydate).ToString("yyyy-MM-dd")
            'Else
            '    Paydate = ConvertDate(txtPaymentDate.Text.Trim)
            'End If

            params(0).Value = 1
            params(1).Value = "N"
            params(2).Value = Date.Now
            params(3).Value = lblCustID.Text
            params(4).Value = lblCustName.Text
            params(5).Value = txtPaymentType.Text
            params(6).Value = txtBank.Text
            params(7).Value = txtAmount.Text
            params(8).Value = lblProjectCode.Text
            params(9).Value = ""
            params(10).Value = FileName

            If fldAttach.HasFile Then

                Dim FilePath As String = (Server.MapPath(ConfigurationManager.AppSettings("ShowAttachFileFolder2")) + "\Payment\" + FileName)
                fldAttach.SaveAs(FilePath)

            End If

            db.executeCommandStore("POST_PAYMENT", params, "XCON_GCConnectionString")

            PostPayment = True
        Catch ex As Exception
            PostPayment = False
        End Try

        Return PostPayment
    End Function

    Private Function PutPayment() As Boolean
        Dim FileName As String = Path.GetFileName(fldAttach.PostedFile.FileName)
        Dim Extension As String = Path.GetExtension(fldAttach.PostedFile.FileName)
        Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

        Try
            Dim params As SqlParameter()
            params = New SqlParameter() {
                        New SqlParameter("@LineNum", SqlDbType.Int) _
                        , New SqlParameter("@DocStatus", SqlDbType.Char, 1) _
                        , New SqlParameter("@DocDate", SqlDbType.DateTime) _
                        , New SqlParameter("@CardCode", SqlDbType.NVarChar, 15) _
                        , New SqlParameter("@CardName", SqlDbType.NVarChar, 100) _
                        , New SqlParameter("@TypePay", SqlDbType.NVarChar, 100) _
                        , New SqlParameter("@Bank", SqlDbType.NVarChar, 254) _
                        , New SqlParameter("@Amount", SqlDbType.Decimal) _
                        , New SqlParameter("@Project", SqlDbType.NVarChar, 20) _
                        , New SqlParameter("@U_MainProject", SqlDbType.NVarChar, 20) _
                        , New SqlParameter("@AttachFile", SqlDbType.NVarChar, 50) _
                        , New SqlParameter("@DocNum", SqlDbType.Int)
            }

            'Dim _Paydate, Paydate As DateTime
            'If DateTime.TryParseExact(txtPaymentDate.Text.Trim(), "dd/MM/yyyy", New System.Globalization.CultureInfo("en-US"), DateTimeStyles.None, _Paydate) Then
            '    Paydate = Convert.ToDateTime(_Paydate).ToString("yyyy-MM-dd")
            'Else
            '    Paydate = ConvertDate(txtPaymentDate.Text.Trim)
            'End If

            params(0).Value = 1
            params(1).Value = "N"
            params(2).Value = Date.Now
            params(3).Value = lblCustID.Text
            params(4).Value = lblCustName.Text
            params(5).Value = txtPaymentType.Text
            params(6).Value = txtBank.Text
            params(7).Value = txtAmount.Text
            params(8).Value = lblProjectCode.Text
            params(9).Value = ""
            params(10).Value = FileName
            params(11).Value = Convert.ToInt32(lblDocnum.Text)

            If fldAttach.HasFile Then

                Dim FilePath As String = (Server.MapPath(ConfigurationManager.AppSettings("ShowAttachFileFolder")) + "\Payment\" + FileName)
                fldAttach.SaveAs(FilePath)

            End If

            db.executeCommandStore("PUT_PAYMENT", params, "XCON_GCConnectionString")

            PutPayment = True
        Catch ex As Exception
            PutPayment = False
        End Try

        Return PutPayment
    End Function

    Private Function ConvertDate(ByVal _str As String)
        Dim arr() As String
        arr = _str.Split("/")
        Dim Newdate As String = arr(2) & "-" & arr(1) & "-" & arr(0)
        Return CDate(Newdate).ToString("yyyy-MM-dd")
    End Function
End Class
