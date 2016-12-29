Imports AjaxControlToolkit
Imports System.Data
Partial Class WebUserControl
    Inherits System.Web.UI.UserControl
    Private Sub WebUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then

        Else

        End If


    End Sub

    'Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    'mpeSelect.Hide()
    'End Sub

End Class
