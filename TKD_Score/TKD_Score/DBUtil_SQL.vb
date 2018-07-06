Option Strict On
Option Explicit On

Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Namespace Util
    Public Class DBUtil_SQL

#Region " ENUM TYPE "
        Public Enum eErrorDB
            TransportError = 233
            ClosedByRemoteHost = 10054
        End Enum
#End Region

#Region " Private Valiable "
        Private MydocPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Private m_Part As String
        Private m_ConnFirst As Boolean = False
        Private SQL_OK As Integer = 0
        Private SQL_ERR As Integer = -1
        Private COMMAND_TIMEOUT As Integer = 150
        Private iConn As Integer = 3
        Private bShowError As Boolean = True
#End Region

#Region " Private Shared Valiable "
        Private Shared m_Conn As SqlConnection
        Private Shared m_Trans As SqlTransaction
        Private Shared m_ConnStr As String
        Private Shared m_ConnStr1 As String
        Private Shared m_ConnStr2 As String
#End Region

#Region " Public Valiable "
        Public Shared ProjectName As String = ""
#End Region

#Region " Propery "
        Protected ReadOnly Property getTransaction() As Object
            Get
                Return m_Trans
            End Get
        End Property

        Public ReadOnly Property getConnection() As Object
            Get
                Return m_Conn
            End Get
        End Property

        Public ReadOnly Property m_SQL_OK() As Integer
            Get
                m_SQL_OK = SQL_OK
            End Get
        End Property

        Public ReadOnly Property m_SQL_ERR() As Integer
            Get
                m_SQL_ERR = SQL_ERR
            End Get
        End Property

        Public ReadOnly Property ConnectionString() As String
            Get
                Return m_ConnStr
            End Get
        End Property

#End Region

#Region " Private Function "
        Private Function SaveErrorToFile(ByVal Header As String, ByVal Body As String) As String

            If Not Directory.Exists(MydocPath & "\" & ProjectName & "\" & ProjectName & "_ERROR") Then
                Directory.CreateDirectory(MydocPath & "\" & ProjectName & "\" & ProjectName & "_ERROR")
            End If
            Dim fileName As String = MydocPath & "\" & ProjectName & "\" & ProjectName & "_ERROR\E" & Format(Now, "yyyyMMddHHmmss") & ".txt"
            Dim File As New FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
            Dim data As New StreamWriter(File)
            data.WriteLine(Header)
            data.WriteLine("     " & Body)
            data.Close()
            File.Close()
            Return fileName
        End Function
        Private Function GetSqlServerError(ByVal pErr As System.Data.SqlClient.SqlErrorCollection) As String
            Dim ret As String = ""
            Dim i As Integer
            Dim errorMessages As New System.Text.StringBuilder()

            For i = 0 To pErr.Count - 1
                errorMessages.Append("Index :" & i.ToString() & " " _
                    & "    ERROR ID : " & pErr(i).Number & ControlChars.NewLine _
                    & "    Message: " & pErr(i).Message & ControlChars.NewLine _
                    & "    LineNumber: " & pErr(i).LineNumber & ControlChars.NewLine _
                    & "    Source: " & pErr(i).Source & ControlChars.NewLine _
                    & "    Procedure: " & pErr(i).Procedure & ControlChars.NewLine)
            Next i
            ret = errorMessages.ToString()
            Return ret
        End Function
#End Region

#Region " Public Function "

#Region " NEW "
        Public Sub New(Optional ByVal Project As String = "DRP System")
            ProjectName = Application.ProductName
        End Sub
#End Region

#Region " Check OPEN Connection "
        ''' <summary>
        ''' Check Open Database Status
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isOpen() As Boolean
            If Not m_Conn Is Nothing Then
                If m_ConnFirst Then
                    If m_Conn.State = ConnectionState.Open Then
                        m_ConnFirst = False
                        Return True
                    End If
                Else
                    If m_ConnStr = m_ConnStr1 Then
                        If m_Conn.State = ConnectionState.Open Then
                            m_ConnFirst = False
                            Return True
                        Else
                            openDB()
                        End If
                    Else
                        closeDB()
                        If openDB(1) Then
                            Return True
                        End If
                    End If
                End If
            End If

            Return False

        End Function
#End Region

#Region " Check Start Transaction "
        Public Function isInTrans() As Boolean
            If Not m_Trans Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function
#End Region

#Region " Set Connection String "
        Public Function setConnString(ByVal sStr1 As String, Optional ByVal sStr2 As String = Nothing, Optional ByVal sPartErr As String = "c:\KTCErrorLog.txt") As Boolean
            Try
                m_ConnStr1 = sStr1
                m_ConnStr2 = sStr2
                m_Part = sPartErr
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

#Region " Open Database "
        Public Function openDB(Optional ByVal nConn As Integer = 3, Optional ByVal ShowError As Boolean = True) As Boolean
            iConn = nConn
            bShowError = ShowError

            Dim strEx As String = ""
            Dim i As Integer
            m_ConnFirst = True
            If isOpen() Then
                closeDB()
            End If

            m_Trans = Nothing
            For i = 1 To nConn
                Try
                    strEx = ""
                    m_ConnStr = m_ConnStr1
                    m_Conn = New SqlConnection(m_ConnStr)
                    m_Conn.Open()
                    Exit For
                Catch ex As SqlException
                    MessageBox.Show(ex.Message)
                End Try
            Next
            If strEx <> "" Then
                Try
                    m_ConnStr = m_ConnStr2
                    m_Conn = New SqlConnection(m_ConnStr2)
                    m_Conn.Open()
                Catch ex As SqlException
                    MessageBox.Show(ex.Message)
                End Try

                Dim MyFileStream As New System.IO.FileStream(m_Part, IO.FileMode.Append, IO.FileAccess.Write, IO.FileShare.Read)
                Dim MyStreamWriter As New System.IO.StreamWriter(MyFileStream)
                Try
                    MyStreamWriter.WriteLine(String.Format("DateTime: {0}, ServerName: {1}, Error: {2}", Now.ToString, m_ConnStr, strEx))
                Catch x As Exception
                    If ShowError Then
                        MessageBox.Show("Can't write error to error log!")
                    End If

                Finally
                    MyStreamWriter.Close()
                    MyFileStream.Close()
                End Try

            End If

            Return isOpen()
        End Function
#End Region

#Region " Close Database "
        ''' <summary>
        ''' Close connection
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub closeDB()

            m_Trans = Nothing
            Try
                m_Conn.Close()
            Catch ex As SqlException

            End Try

        End Sub
#End Region

#Region " Run SQL "
        '**************************************************************************************
        '
        '   Function Name:   runSQL
        '   Description:
        '       SQL Execute wrapper (Add, insert, update, delete Sql statement)
        '   IN/Out Arg:         sSQL    =   SQL Statement
        '                       cmd = Sql Command             
        '   Return Value:   
        '           SQL_ERR: if error
        '           others : the number of effected rows
        '**************************************************************************************
        Public Overloads Function runSQL(ByVal sSQL As String) As Boolean
            Dim cmd As SqlCommand
            Dim ret As Boolean = True

            ' Make sure valid data

            If isOpen() Then
                Try
                    cmd = New SqlCommand(sSQL, m_Conn)

                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If

                    cmd.ExecuteNonQuery()

                Catch ex As SqlException
                    MessageBox.Show(ex.Message)
                    ret = False
                End Try
            End If
            Return ret
        End Function

        ''' <summary>
        ''' ใช้ในการ Insert OR Update Data Base
        ''' </summary>
        ''' <param name="sSQL">Quiry ในการ Insert Or Update</param>
        ''' <param name="pParameter">Parameter ที่ใช้แทน ตัว @ddd ใน Quiry</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function runSQL(ByVal sSQL As String, ByVal pParameter As Hashtable, Optional ByRef pErr As String = "") As Boolean

            Dim cmd As SqlCommand
            Dim ret As Boolean = True

            ' Make sure valid data
            If isOpen() Then
                Try
                    cmd = New SqlCommand(sSQL, m_Conn)
                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If

                    With cmd
                        .Parameters.Clear()

                        For Each p As Object In pParameter.Keys
                            .Parameters.Add(p.ToString, ConvertType(pParameter(p).GetType.Name)).Value = pParameter(p)
                        Next
                    End With

                    cmd.ExecuteNonQuery()

                Catch ex As SqlException
                    MessageBox.Show(ex.Message)
                    ret = False
                End Try
            End If

            Return ret

        End Function
#End Region

#Region " Get Data From Database "
        ''' <summary>
        ''' ใช้ในการ หาข้อมูลจาก Data Base
        ''' </summary>
        ''' <param name="pSql">Quiry สำหรับหาใน Data Base </param>
        ''' <param name="pParameter">ตัวแปลที่ใช้แทน @ddd ใน pSql </param>
        ''' <param name="pDataSet">คืนค่า ที่ค้นหาได้</param>
        ''' <param name="ShowError">ให้แสดง Error หรือเปล่า</param>
        ''' <param name="pErr">ค่า Error ที่ส่งกลับมา</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function getData(ByVal pSql As String, ByVal pParameter As Hashtable, ByRef pDataSet As DataSet, Optional ByVal ShowError As Boolean = True, Optional ByRef pErr As String = "") As Integer
            Dim ret As Integer = SQL_ERR
            Dim cmd As SqlCommand
            Dim adapter As New SqlDataAdapter

            If isOpen() Then
                Try
                    cmd = New SqlCommand(pSql, m_Conn)
                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If

                    With cmd
                        .Parameters.Clear()

                        For Each p As Object In pParameter.Keys
                            .Parameters.Add(p.ToString, ConvertType(pParameter(p).GetType.Name)).Value = pParameter(p)

                        Next


                    End With

                    adapter = New SqlDataAdapter(cmd)
                    ' Fill the dataset
                    If pDataSet Is Nothing Then

                    End If
                    pDataSet = New DataSet
                    adapter.Fill(pDataSet)
                    ret = SQL_OK

                Catch ex As SqlException
                    debugInfo(pSql)
                Finally
                    adapter.Dispose()
                End Try
            End If
            Return ret
        End Function
        '**************************************************************************************
        '
        '   Function Name:  getData
        '   Description:
        '       SQL Execute wrapper 
        '   IN/Out Arg:         sSQL    =   SQL Statement
        '                       dataSet :   DataSet
        '   Return Value:   SQL_ERR/ SQL_OK
        '
        '**************************************************************************************
        Public Overloads Function getData(ByVal sSQL As String, ByRef dataTable As DataTable, Optional ByVal ShowError As Boolean = True, Optional ByRef pErr As String = "") As Integer
            ' Set up the SQL command object
            Dim adapter As New SqlDataAdapter
            Dim ds As New DataSet
            Dim cmd As SqlCommand
            Dim ret As Integer = SQL_ERR

            If isOpen() Then
                Try
                    ' Retreive the data
                    cmd = New SqlCommand(sSQL, m_Conn)

                    ''default value is 30
                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If
                    adapter = New SqlDataAdapter(cmd)
                    ' Fill the dataset
                    adapter.Fill(ds)
                    dataTable = ds.Tables(0)
                    ret = SQL_OK

                Catch ex As SqlException
                    MessageBox.Show(ex.Message)
                    debugInfo(sSQL)
                Finally
                    adapter.Dispose()
                End Try
            End If
            Return ret

        End Function

#End Region

        '**************************************************************************************
        '
        '   Function Name:   exeScalar
        '   Description:
        '       SQL ExecuteScalar wrapper (select Sql statement)
        '   IN/Out Arg:         sSQL    =   Select SQL Statement
        '                       objResult = the result object
        '   Return Value:
        '           SQL_ERR/SQL_OK    
        '**************************************************************************************
        Public Function exeScalar(ByVal sSQL As String, ByRef objResult As Object) As Integer
            Dim cmd As SqlCommand
            Dim ret As Integer = SQL_ERR

            ' Make sure valid data
            If isOpen() Then
                Try
                    cmd = New SqlCommand(sSQL, m_Conn)

                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If

                    objResult = cmd.ExecuteScalar()
                    ret = SQL_OK

                Catch ex As SqlException
                    MsgBox("exeScalar:" & ex.Message, MsgBoxStyle.Critical)
                    debugInfo(sSQL)
                End Try
            End If
            Return ret
        End Function

        Public Function beginTrans(Optional ByVal iso As IsolationLevel = IsolationLevel.Unspecified) As Integer
            Dim ret As Integer = SQL_ERR
            If isOpen() Then
                If m_Trans Is Nothing Then
                    Try
                        If iso <> IsolationLevel.Unspecified Then
                            m_Trans = m_Conn.BeginTransaction(IsolationLevel.ReadUncommitted)
                        Else
                            m_Trans = m_Conn.BeginTransaction()
                        End If
                        ret = SQL_OK
                    Catch ex As SqlException
                        MsgBox("beginTrans:" & ex.Message, MsgBoxStyle.Critical)
                    End Try
                End If
            End If
            Return ret
        End Function

        Public Function commitTrans() As Integer
            Dim ret As Integer = SQL_ERR

            If isOpen() Then
                If Not m_Trans Is Nothing Then
                    Try
                        m_Trans.Commit()
                        m_Trans = Nothing
                        ret = SQL_OK

                    Catch ex As SqlException
                        MsgBox("commitTrans:" & ex.Message, MsgBoxStyle.Critical)
                    End Try
                End If
            End If
            Return ret
        End Function

        Public Function rollbackTrans() As Integer
            Dim ret As Integer = SQL_ERR
            If isOpen() Then
                If Not m_Trans Is Nothing Then
                    Try
                        m_Trans.Rollback()
                        m_Trans = Nothing
                        ret = SQL_OK

                    Catch ex As SqlException
                        MsgBox("rollbackTrans:" & ex.Message, MsgBoxStyle.Critical)
                    End Try
                End If
            End If
            Return ret
        End Function

        Public Sub debugInfo(ByVal sInfo As String)
            Debug.WriteLine(Now().ToString() & ":" & sInfo)
        End Sub

        Public Function getSchema(ByVal sSQL As String, ByRef pDataTable As DataTable) As Integer
            ' Set up the SQL command object
            Dim adapter As New SqlDataAdapter
            Dim cmd As SqlCommand
            Dim ret As Integer = SQL_ERR

            If isOpen() Then
                Try
                    ' Retreive the data
                    cmd = New SqlCommand(sSQL, m_Conn)

                    ''default value is 30
                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If
                    adapter = New SqlDataAdapter(cmd)
                    ' Fill the schema

                    adapter.FillSchema(pDataTable, SchemaType.Source)

                    ret = SQL_OK

                Catch ex As SqlException
                    MsgBox("getSchema:" & ex.Message, MsgBoxStyle.Critical)

                Finally
                    adapter.Dispose()
                End Try
            End If
            Return ret

        End Function

#Region " Create Primary Key For Datatable "
        ''' <summary>
        ''' ใช้สำหรับ สร้าง Primary Key ให้กับ Data Table เพื่อเพิ่มความเร็วในการ Search
        ''' </summary>
        ''' <param name="dt">Data Table ที่ต้องการสร้าง Primary Key</param>
        ''' <param name="Columns">Columns ที่จะสร้างเป็น Primary Key</param>
        ''' <param name="pErr">Error Meesage</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreatePrimaryKey(ByRef dt As DataTable, ByVal Columns() As DataColumn, Optional ByRef pErr As String = "") As Boolean
            Dim ret As Boolean = True
            Try
                dt.PrimaryKey = Columns
            Catch ex As Exception
                ret = False
                pErr = ex.Message
            End Try

            Return ret
        End Function

        ''' <summary>
        ''' ใช้สำหรับ สร้าง Primary Key ให้กับ Data Table เพื่อเพิ่มความเร็วในการ Search
        ''' </summary>
        ''' <param name="dt">Data Table ที่ต้องการสร้าง Primary Key</param>
        ''' <param name="Columns">Columns ที่จะสร้างเป็น Primary Key Ex. Col1,Col2,Col3</param>
        ''' <param name="pErr">Error Meesage</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreatePrimaryKey(ByRef dt As DataTable, ByVal Columns As String, Optional ByRef pErr As String = "") As Boolean
            Dim ret As Boolean = True
            Try
                Dim str() As String = Columns.Split(CChar(","))
                Dim col(str.Length) As System.Data.DataColumn
                For i As Integer = 0 To str.Length - 1
                    col(i) = dt.Columns(str(i))
                Next
                dt.PrimaryKey = col
            Catch ex As Exception
                ret = False
                pErr = ex.Message
            End Try

            Return ret
        End Function
#End Region

#End Region

#Region " Function For SQL SERVER "
        Public Function runStored(ByVal pDbType As DbType, ByVal sSTRORED As String, Optional ByVal sDateFrom As String = "", Optional ByVal sDateTo As String = "", Optional ByVal sDaliveFrom As String = "", Optional ByVal sDaliveTo As String = "") As Integer

            Dim cmd As SqlCommand
            Dim ret As Integer = SQL_ERR

            ' Make sure valid data

            If isOpen() Then
                Try
                    cmd = New SqlCommand(sSTRORED, m_Conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    ret = cmd.ExecuteNonQuery()

                Catch ex As SqlException
                    MsgBox("runSTROED:" & ex.Message, MsgBoxStyle.Critical)
                    debugInfo(sSTRORED)
                End Try
            End If

            Return ret

        End Function

        ''' <summary>
        ''' ใช้ในการดึงข้อมูลจาก Database
        ''' </summary>
        ''' <param name="sSQL">Query ที่ใช้ดึงข้อมูล</param>
        ''' <param name="pDataSet">ข้อมูลที่ได้จาก Database</param>
        ''' <param name="ShowError">แสดงหรือไม่แสดง Error</param>
        ''' <param name="pErr">String Error</param>
        ''' <param name="CountError">Error Count</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function getData(ByVal sSQL As String, ByRef pDataSet As DataSet, _
                                                    Optional ByVal ShowError As Boolean = True, Optional ByRef pErr As String = "", _
                                                    Optional ByRef CountError As Integer = 0) As Integer
            ' Set up the SQL command object
            Dim adapter As New SqlDataAdapter
            Dim cmd As SqlCommand
            Dim ret As Integer = SQL_ERR

            If isOpen() Then
                Try
                    ' Retreive the data
                    cmd = New SqlCommand(sSQL, m_Conn)

                    ''default value is 30
                    cmd.CommandTimeout = COMMAND_TIMEOUT

                    If Not m_Trans Is Nothing Then
                        cmd.Transaction = m_Trans
                    End If
                    adapter = New SqlDataAdapter(cmd)
                    ' Fill the dataset
                    If pDataSet Is Nothing Then

                    End If
                    pDataSet = New DataSet
                    adapter.Fill(pDataSet)
                    ret = SQL_OK

                Catch ex As SqlException
                    MessageBox.Show(ex.Message)
                    debugInfo(sSQL)
                Finally
                    adapter.Dispose()
                End Try
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Convert VB Type TO DB Type
        ''' </summary>
        ''' <param name="Type">Type From VB</param>
        ''' <returns>DB Type</returns>
        ''' <remarks></remarks>
        Private Function ConvertType(ByVal Type As String) As SqlDbType
            Dim ret As SqlDbType = SqlDbType.NVarChar
            Select Case Type
                Case "String"
                    ret = SqlDbType.NVarChar
                Case "Int32"
                    ret = SqlDbType.Int
                Case "Double"
                    ret = SqlDbType.Decimal
                Case "Date", "DateTime"
                    ret = SqlDbType.DateTime
            End Select
            Return ret
        End Function


#End Region

    End Class
End Namespace
