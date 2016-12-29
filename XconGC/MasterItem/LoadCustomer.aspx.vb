Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Linq

Partial Class MasterItem_LoadCustomer
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")
    Private dt As DataTable

    Private Sub btnLoad_Load(sender As Object, e As EventArgs) Handles btnLoad.Load
        If Page.IsPostBack = False Then
            BindData()
        End If
    End Sub
    Private Function PostCustomerXconGC() As Boolean

        Try
            Dim params As SqlParameter()
            params = New SqlParameter() {
                                New SqlParameter("@CodeFrom", SqlDbType.NVarChar, 15) _
                                , New SqlParameter("@CodeTo", SqlDbType.NVarChar, 15)
                    }


            params(0).Value = txtCustFrom.Text
            params(1).Value = txtCustTo.Text

            db.executeCommandStore("UPSERT_OCRD", params, "GetCustomerXcon")

            PostCustomerXconGC = True
        Catch ex As Exception
            PostCustomerXconGC = False
        End Try

        Return PostCustomerXconGC
    End Function
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If PostCustomerXconGC() Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        LoadCustomerSqlDataSource.SelectCommand = "SELECT [CardCode], [CardName], [Address] FROM [GC_OCRD]"
        gvCustomer.DataBind()
    End Sub

    Private Sub ClearData()
        txtCustFrom.Text = String.Empty
        txtCustTo.Text = String.Empty
    End Sub
End Class
