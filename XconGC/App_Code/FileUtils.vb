Imports Microsoft.VisualBasic
Imports System.IO

Public Class FileUtils

    Public Shared Function SaveOrderFile(ByVal code As String, ByVal slot As Integer, ByVal file As HttpPostedFile) As Boolean

        Try

            ' HTTP Context to get access to the submitted data
            Dim context As HttpContext = HttpContext.Current
            Dim severPath As String = context.Server.MapPath("~/UploadFiles/Order")
            Dim targetPath As String = $"{severPath}\{code}\slot{slot}"
            ' prevent save fail when path is not valid.
            Dim dir As New DirectoryInfo(targetPath)
            If dir.Exists = False Then
                dir.Create()
            Else
                dir.Delete(True)
                dir.Create()
            End If

            Dim savePath As String = $"{targetPath}\{file.FileName}"
            file.SaveAs(savePath)

            Return True
        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Shared Function GetOrderFile() As Boolean
        Try
            ' HTTP Context to get access to the submitted data
            Dim postedContext As HttpContext = HttpContext.Current

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
