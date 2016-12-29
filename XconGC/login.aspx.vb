Imports System.Data
Partial Class login
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim strSQL As String = "SELECT [CardCode],[CardName],[UserID],[Password],[ProjectCode],[UserLevel]" +
                                " FROM [dbo].[GC_USER]" +
                                " where UserID ='" & Me.txtUserName.Text.Trim & "' and Password = '" & txtPassword.Text.Trim & "'"

        Dim dt As DataTable = db.getDataTableByQuery(strSQL, "UserLogin")

        If dt.Rows.Count > 0 Then
            Session.RemoveAll()
            Session("CardCode") = dt.Rows(0).Item("CardCode")
            Session("CardName") = dt.Rows(0).Item("CardName")
            Session("UserID") = dt.Rows(0).Item("UserID")
            Session("ProjectCode") = dt.Rows(0).Item("ProjectCode")
            Session("UserLevel") = dt.Rows(0).Item("UserLevel")

            'If dt.Rows(0).Item("AdminLevelKey") = 5 Or dt.Rows(0).Item("AdminLevelKey") = 6 Then
            Response.Redirect("gcOrder.aspx")
        Else
            lblError.Visible = True
            lblError.Text = "Username or password is valid. Please try again."
            txtUserName.Text = ""
        End If
    End Sub
End Class
