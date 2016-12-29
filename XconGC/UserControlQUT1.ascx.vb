Imports AjaxControlToolkit
Imports System.Data
Partial Class WebUserControl
    Inherits System.Web.UI.UserControl
    Private Sub WebUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim groupID As String = Request.QueryString("ID")
        If Page.IsPostBack = False Then




        Else

        End If

    End Sub

    'Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    'mpeSelect.Hide()
    'End Sub
    Protected Sub gvGcOrder_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvGcOrder.RowDataBound
        Dim client = ScriptManager.GetCurrent(Page)

        Dim lnkSelect As LinkButton = e.Row.FindControl("lnkSelect")
        If lnkSelect IsNot Nothing Then
            client.RegisterPostBackControl(lnkSelect)
        End If
    End Sub
    Protected Sub lnkSelect_Click(sender As Object, e As EventArgs)

        Dim popup As ModalPopupExtender = Page.FindControl("mpeSelect")
        If popup IsNot Nothing Then
            popup.Show()
        End If
    End Sub
    Protected Sub lnkSelect_Command(sender As Object, e As CommandEventArgs)

    End Sub
End Class
