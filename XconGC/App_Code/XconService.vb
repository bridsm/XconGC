Imports System.IO
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class XconService
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function UpdateItem() As String
        Try

            ' HTTP Context to get access to the submitted data
            Dim postedContext As HttpContext = HttpContext.Current
            ' File Collection that was submitted with posted data
            Dim params As NameValueCollection = postedContext.Request.Params
            If params.Count > 0 Then

                Dim code As String = params("form[code]")
                Dim name As String = params("form[name]")
                Dim labelName As String = params("form[labelName]")
                Dim size As String = params("form[size]")
                Dim color As String = params("form[color]")
                Dim txtRem1 As String = params("form[txtRem1]")
                Dim txtRem2 As String = params("form[txtRem2]")



                ' get file upload
                Dim severPath As String = postedContext.Server.MapPath("~/UploadFiles/Order")
                Dim dir As New DirectoryInfo($"{severPath}\{code}")
                If dir.Exists Then

                End If


            End If

            Return "COMPLETE"

        Catch ex As Exception
            Return "FAILED"
        End Try

    End Function

End Class

Public Class NameValue
    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _value As String
    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
        End Set
    End Property

End Class