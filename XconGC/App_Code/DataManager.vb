Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text
Imports System.Data
Imports Microsoft.SqlServer.Server

Public Class DataManager
    Private con As SqlConnection
    Public ErrMsg As String
    '***********
    'constructor
    '***********
    Public Sub New(ByVal configStr As String)
        'con = New SqlConnection(ConfigurationManager.ConnectionStrings(configStr).ConnectionString)
        Dim conString As New String(String.Empty)
        conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString

        If con Is Nothing Then
            con = New SqlConnection(conString)
        Else
            con.ConnectionString = configStr
        End If
    End Sub

    '*********************
    ' Get Data Set
    '*********************
    Public Function getDataSet(ByVal sqlstr As String, ByVal name As String) As DataSet
        Try
            Dim adap As New SqlDataAdapter
            Dim rtnDS As New DataSet
            adap.SelectCommand = New SqlCommand(sqlstr, con)
            adap.Fill(rtnDS, name)

            Return rtnDS
        Catch exp As Exception
            Throw exp
        End Try
    End Function

    Public Function addDataset(ByVal sqlstr As String, ByVal ds As DataSet, ByVal name As String) As DataSet
        Try
            Dim adap As New SqlDataAdapter
            adap.SelectCommand = New SqlCommand(sqlstr, con)
            adap.Fill(ds, name)
            Return ds
        Catch exp As Exception
            Throw exp
        End Try
    End Function

    ''*************
    ''get datatable
    ''*************

    Public Function getDataTable(ByVal tableName As String) As DataTable
        Dim localAdap As New SqlDataAdapter
        Dim rtntable As New DataTable(tableName)
        localAdap.SelectCommand = New SqlCommand("SELECT * FROM " & tableName, con)
        localAdap.Fill(rtntable)
        localAdap.Dispose()
        Return rtntable

    End Function

    Public Function getDataTable(ByVal tableName As String, ByVal criteria As String) As DataTable
        Dim localAdap As New SqlDataAdapter
        Dim rtntable As New DataTable(tableName)
        localAdap.SelectCommand = New SqlCommand("SELECT * FROM " & tableName & " WHERE " & criteria, con)
        localAdap.Fill(rtntable)
        localAdap.Dispose()
        Return rtntable

    End Function

    Public Function getDataTableByQuery(ByVal qStr As String, ByVal tableName As String) As DataTable
        Dim localAdap As New SqlDataAdapter
        Dim rtntable As New DataTable(tableName)
        localAdap.SelectCommand = New SqlCommand(qStr, con)
        localAdap.Fill(rtntable)
        localAdap.Dispose()
        Return rtntable

    End Function

    Public Function getDataTableByQuery(ByVal qStr As String, ByVal tableName As String, ByVal configStr As String) As DataTable
        ' // Check con first
        If con.ConnectionString = "" Then
            Dim conString As New String(String.Empty)
            conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString
            con = New SqlConnection(conString)

        End If
        '// ----------------- //

        Dim localAdap As New SqlDataAdapter
        Dim rtntable As New DataTable(tableName)
        localAdap.SelectCommand = New SqlCommand(qStr, con)
        localAdap.Fill(rtntable)
        localAdap.Dispose()
        Return rtntable

    End Function

    Public Function getDataTableByQueryStore(ByVal qStr As String, ByVal tableName As String) As DataTable
        Dim localAdap As New SqlDataAdapter
        Dim rtntable As New DataTable(tableName)
        localAdap.SelectCommand = New SqlCommand(qStr, con)
        localAdap.Fill(rtntable)
        localAdap.Dispose()
        Return rtntable

    End Function

    Public Function getDataTableByQueryContunue(ByVal qStr As String, ByVal tableName As String, ByVal configStr As String) As DataTable

        ' // Check con first
        If con.ConnectionString = "" Then
            Dim conString As New String(String.Empty)
            conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString
            con = New SqlConnection(conString)

        End If
        '// ----------------- //

        Dim localAdap As New SqlDataAdapter
        Dim rtntable As New DataTable(tableName)
        localAdap.SelectCommand = New SqlCommand(qStr, con)
        localAdap.Fill(rtntable)
        localAdap.Dispose()
        Return rtntable

    End Function
    '**************
    'get datareader
    '**************
    Public Function getDataReader(ByVal sqlStr As String) As SqlDataReader

        Try
            Dim rtnDR As SqlDataReader
            Dim cmd As New SqlCommand
            cmd.CommandText = sqlStr
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            rtnDR = cmd.ExecuteReader()

            Return rtnDR
        Catch ex As Exception

            Throw ex
        Finally
            con.Dispose()
            con.Close()
        End Try
    End Function

    '***************
    'execute DML SQL
    '***************
    Public Function executeCommand(ByVal dmlStr As String) As Integer
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text

            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommand(ByVal dmlStr As String, ByVal params As SqlParameter()) As Integer
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next

            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommand(ByVal dmlStr As String, ByVal params As SqlParameter(), ByVal cParam As SqlParameter) As Integer
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next
            cmd.Parameters.Add(cParam)
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommand(ByVal dmlStr As String, ByVal params As SqlParameter(), ByVal configStr As String) As Integer
        Try
            ' // Check con first
            If con.ConnectionString = "" Then
                Dim conString As New String(String.Empty)
                conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString
                con = New SqlConnection(conString)

            End If
            '// ----------------- //

            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next
            'cmd.Parameters.Add(cParam)
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            'Throw ex
            ErrMsg = "***Some record duplicated."
            Exit Function
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommandStore(ByVal dmlStr As String, ByVal params As SqlParameter(), ByVal configStr As String) As Integer
        Try
            ' // Check con first
            If con.ConnectionString = "" Then
                Dim conString As New String(String.Empty)
                conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString
                con = New SqlConnection(conString)

            End If
            '// ----------------- //

            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.StoredProcedure
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next

            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommandStoreReturnKey(ByVal dmlStr As String, ByVal params As SqlParameter(), ByVal configStr As String) As Integer
        Try
            ' // Check con first
            If con.ConnectionString = "" Then
                Dim conString As New String(String.Empty)
                conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString
                con = New SqlConnection(conString)

            End If
            '// ----------------- //

            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.StoredProcedure
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next

            cmd.ExecuteNonQuery()
            cmd.CommandText = "SELECT @@IDENTITY"
            Return CInt(cmd.ExecuteScalar())
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function
    Public Function executeCommandReturnKey(ByVal dmlStr As String, ByVal params As SqlParameter()) As Integer
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next
            cmd.ExecuteNonQuery()
            cmd.CommandText = "SELECT @@IDENTITY"
            Return CInt(cmd.ExecuteScalar())
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommandReturnKey(ByVal dmlStr As String) As Integer
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text

            cmd.ExecuteNonQuery()
            cmd.CommandText = "SELECT @@IDENTITY"
            Return CInt(cmd.ExecuteScalar())
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeCommandReturnKey(ByVal dmlStr As String, ByVal params As SqlParameter(), ByVal configStr As String) As Integer
        Try
            ' // Check con first
            If con.ConnectionString = "" Then
                Dim conString As New String(String.Empty)
                conString = System.Configuration.ConfigurationManager.ConnectionStrings(configStr).ConnectionString
                con = New SqlConnection(conString)

            End If
            '// ----------------- //

            Dim cmd As New SqlCommand
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text
            Dim param As SqlParameter
            For Each param In params
                cmd.Parameters.Add(param)
            Next
            cmd.ExecuteNonQuery()
            cmd.CommandText = "SELECT @@IDENTITY"
            Return CInt(cmd.ExecuteScalar())
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeScalar(ByVal sqlStr As String) As Double
        Try
            'Dim rtnDR As sqlDataReader
            Dim cmd As New SqlCommand
            cmd.CommandText = sqlStr
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If
            Dim rtn As Double = cmd.ExecuteScalar()

            Return rtn
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function executeScalarString(ByVal sqlStr As String) As String
        Try
            'Dim rtnDR As sqlDataReader
            Dim cmd As New SqlCommand
            cmd.CommandText = sqlStr
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            If Not con.State = ConnectionState.Open Then
                con.Open()
            End If

            Return cmd.ExecuteScalar()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    Public Function insertCommandAuto(ByVal dmlStr As String) As Integer
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = con
            con.Open()
            cmd.CommandText = dmlStr
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()
            cmd.CommandText = "SELECT @@IDENTITY"
            Return CInt(cmd.ExecuteScalar())
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function

    '****************
    'close connection
    '****************
    Public Sub close()
        con.Close()
    End Sub

End Class

