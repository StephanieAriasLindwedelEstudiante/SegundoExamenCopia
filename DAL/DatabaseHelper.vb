Imports System.Data.SqlClient

Public Class DatabaseHelper
    Private ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("Login").ConnectionString

    Public Sub New()
        EnsureErrorLogTableExists() ' Asegúrate de que la tabla exista al crear una instancia.
    End Sub

    ' Método para obtener la conexión
    Public Function GetConnection() As SqlConnection
        Dim conn As New SqlConnection(connectionString)
        Try
            conn.Open()
        Catch ex As SqlException
            Throw New Exception("Error al abrir la conexión a la base de datos: " & ex.Message)
        End Try
        Return conn
    End Function

    ' Método para ejecutar un comando SQL (INSERT, UPDATE, DELETE)
    Public Sub ExecuteNonQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing)
        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters.ToArray())
                End If
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    LogError(ex, query) ' Registrar el error en caso de excepción
                    Throw New Exception("Error al ejecutar el comando: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    ' Método para ejecutar una consulta SQL y devolver un DataTable
    Public Function ExecuteQuery(query As String, Optional parameters As List(Of SqlParameter) = Nothing) As DataTable
        Dim dt As New DataTable()
        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters.ToArray())
                End If
                Try
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                Catch ex As Exception
                    LogError(ex, query) ' Registrar el error en caso de excepción
                    Throw New Exception("Error al ejecutar la consulta: " & ex.Message)
                End Try
            End Using
        End Using
        Return dt
    End Function

    ' Método para asegurar que la tabla ErrorLog existe
    Private Sub EnsureErrorLogTableExists()
        Dim query As String = "
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ErrorLog')
            BEGIN
                CREATE TABLE ErrorLog (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    ErrorMessage NVARCHAR(4000),
                    ErrorSeverity INT,
                    ErrorState INT,
                    ErrorProcedure NVARCHAR(200),
                    ErrorLine INT,
                    ErrorDateTime DATETIME DEFAULT GETDATE()
                )
            END"

        ExecuteNonQuery(query)
    End Sub

    ' Método para registrar errores en una tabla de auditoría.
    Private Sub LogError(ex As Exception, Optional query As String = "")
        Dim errorMessage As String = ex.Message.Replace("'", "''") ' Escapar comillas simples.
        Dim severity As Integer = 16 ' Puedes ajustar según el tipo de error.
        Dim state As Integer = 1     ' Estado del error.
        Dim procedureName As String = If(ex.TargetSite IsNot Nothing, ex.TargetSite.Name, DBNull.Value)
        Dim lineNumber As Integer = If(ex.StackTrace IsNot Nothing AndAlso ex.StackTrace.Contains(":line "), Integer.Parse(ex.StackTrace.Split(":line ").Last().Split(" "c)(0)), 0)

        Dim logQuery As String = "
            INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState, ErrorProcedure, ErrorLine, ErrorDateTime) 
            VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, GETDATE())"

        Dim parameters As New List(Of SqlParameter) From {
            New SqlParameter("@ErrorMessage", errorMessage),
            New SqlParameter("@ErrorSeverity", severity),
            New SqlParameter("@ErrorState", state),
            New SqlParameter("@ErrorProcedure", procedureName),
            New SqlParameter("@ErrorLine", lineNumber)
        }

        ExecuteNonQuery(logQuery, parameters) ' Llamar a ExecuteNonQuery para registrar el error.
    End Sub

End Class