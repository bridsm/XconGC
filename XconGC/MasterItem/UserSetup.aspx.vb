Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Partial Class MasterItem_UserSetup
    Inherits System.Web.UI.Page
    Dim db As New DataManager("XCON_GCConnectionString")
    Dim dtDest As DataTable
    Private Sub MasterItem_UserSetup_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            BindData()
        End If
    End Sub

    Private Sub BindData()
        Dim sql As String = "Select IIF(GC_USER.CardCode Is NULL, GC_OCRD.CardCode, GC_USER.CardCode) As CardCode
                            , GC_USER.CardName AS [Company Name (Sort)], GC_OCRD.CardName AS [Company Name (SAP)], GC_USER.UserID, GC_USER.Password
                            FROM GC_OCRD FULL JOIN
                            GC_USER ON GC_OCRD.CardCode = GC_USER.CardCode
                            WHERE GC_USER.ProjectCode = '" & ddlProject.SelectedValue & "'"

        Dim dtBind As DataTable

        dtBind = db.getDataTableByQuery(sql, "GetUser")
        gvUser.DataSource = dtBind
        gvUser.DataBind()
    End Sub

    Private Function Import_To_Grid(ByVal FilePath As String, ByVal Extension As String, ByVal isHDR As String) As Boolean
        Dim conStr As String = ""
        Select Case Extension
            Case ".xls"
                'Excel 97-03
                conStr = ConfigurationManager.ConnectionStrings("Excel03ConString") _
                           .ConnectionString
                Exit Select
            Case ".xlsx"
                'Excel 07
                conStr = ConfigurationManager.ConnectionStrings("Excel07ConString") _
                          .ConnectionString
                Exit Select
        End Select
        conStr = String.Format(conStr, FilePath, isHDR)

        Dim connExcel As New OleDbConnection(conStr)
        Dim cmdExcel As New OleDbCommand()
        Dim oda As New OleDbDataAdapter()
        Dim dt As New DataTable()

        cmdExcel.Connection = connExcel

        'Get the name of First Sheet
        connExcel.Open()
        Dim dtExcelSchema As DataTable
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim SheetName As String = dtExcelSchema.Rows(0)("TABLE_NAME").ToString()
        connExcel.Close()

        'Read Data from First Sheet
        connExcel.Open()
        cmdExcel.CommandText = "SELECT * From [" & SheetName & "]"
        oda.SelectCommand = cmdExcel
        oda.Fill(dt)
        connExcel.Close()

        'Post and Get USER
        If dt.Rows.Count > 0 Then
            Dim _sql As String = " INSERT INTO dbo.GC_USER(CardCode, CardName, UserID, Password, ProjectCode)" +
                                    " VALUES(@CardCode, @CardName, @UserID, @Password, @ProjectCode)"

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim params As SqlParameter()
                params = New SqlParameter() {
                                        New SqlParameter("@CardCode", SqlDbType.NVarChar, 15) _
                                        , New SqlParameter("@CardName", SqlDbType.NVarChar, 100) _
                                        , New SqlParameter("@UserID", SqlDbType.NVarChar, 15) _
                                        , New SqlParameter("@Password", SqlDbType.NVarChar, 20) _
                                        , New SqlParameter("@ProjectCode", SqlDbType.NVarChar, 15)
                            }

                params(0).Value = dt.Rows(i)(0).ToString()
                params(1).Value = dt.Rows(i)(1).ToString()
                params(2).Value = dt.Rows(i)(2).ToString()
                params(3).Value = dt.Rows(i)(3).ToString()
                params(4).Value = ddlProject.SelectedItem.Text

                Try
                    db.executeCommand(_sql, params, "XCON_GCConnectionString")
                    Import_To_Grid = True
                Catch ex As Exception
                    'lblErrmsg.Text = db.ErrMsg
                    Import_To_Grid = False
                End Try

            Next

            Dim sql As String = "Select IIF(GC_USER.CardCode Is NULL, GC_OCRD.CardCode, GC_USER.CardCode) As CardCode
                                , GC_USER.CardName AS [Company Name (Sort)], GC_OCRD.CardName AS [Company Name (SAP)], GC_USER.UserID, GC_USER.Password
                                FROM GC_OCRD FULL JOIN
                                GC_USER ON GC_OCRD.CardCode = GC_USER.CardCode
                                WHERE GC_USER.ProjectCode = '" & ddlProject.SelectedValue & "'"

            dtDest = db.getDataTableByQuery(sql, "User", "XCON_GCConnectionString")

        End If

        'If dt.Rows.Count > 0 Then
        '    Dim sql As String = "Select CardCode, CardName As [Company Name (Sort)], CardName As [Company Name (SAP)] FROM GC_OCRD"


        '    dtDest = db.getDataTableByQuery(sql, "User", "XCON_GCConnectionString")

        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        For j As Integer = 0 To dtDest.Rows.Count - 1
        '            If dt.Rows(i)(0) = dtDest.Rows(j)(0) Then
        '                dt.Rows(i)(2) = dtDest.Rows(j)(2)
        '            Else
        '                dt.Rows.Add()
        '                dt.Rows(i + 1)(0) = dtDest.Rows(j)(0)
        '                dt.Rows(i + 1)(1) = dtDest.Rows(j)(1)
        '                dt.Rows(i + 1)(2) = dtDest.Rows(j)(2)
        '            End If
        '        Next
        '    Next
        'End If

        'Bind Data to GridView
        gvUser.Caption = Path.GetFileName(FilePath)
        gvUser.DataSource = dtDest
        gvUser.DataBind()

        lblErrmsg.Text = db.ErrMsg

        Return Import_To_Grid

    End Function
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        lblErrmsg.Text = String.Empty

        If fulExcel.HasFile Then
            Dim FileName As String = Path.GetFileName(fulExcel.PostedFile.FileName)
            Dim Extension As String = Path.GetExtension(fulExcel.PostedFile.FileName)
            Dim FolderPath As String = ConfigurationManager.AppSettings("FolderPath")

            'Dim FilePath As String = Server.MapPath(FolderPath + FileName)
            Dim FilePath As String = (Server.MapPath(ConfigurationManager.AppSettings("ShowAttachFileFolder")) + "\Documents\" + FileName)
            fulExcel.SaveAs(FilePath)
            DeleteUserData()
            Import_To_Grid(FilePath, Extension, "Yes")

        End If
    End Sub

    Private Sub DeleteUserData()
        Dim _sql = "DELETE FROM [dbo].[GC_USER] WHERE ProjectCode = @ProjectCode"

        Dim params As SqlParameter()
        params = New SqlParameter() {
                        New SqlParameter("@ProjectCode", SqlDbType.NVarChar, 15)
            }

        params(0).Value = ddlProject.SelectedItem.Text

        db.executeCommand(_sql, params, "XCON_GCConnectionString")

    End Sub
    Protected Sub ddlProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProject.SelectedIndexChanged
        BindData()
    End Sub
End Class
