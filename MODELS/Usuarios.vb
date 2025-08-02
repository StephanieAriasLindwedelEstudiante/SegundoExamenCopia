Public Class Usuarios
    Public Property Id As Integer
    Public Property Nombre As String
    Public Property Apellido As String
    Public Property Correo As String
    Public Property Contrasena As String
    Public Property Rol_ID As Integer
    Public Sub New()
    End Sub
    Public Function Validar() As Boolean
        Return Not String.IsNullOrEmpty(Correo) AndAlso Not String.IsNullOrEmpty(Contrasena)
    End Function
    Public Function dtToUsuarios(dataTable As DataTable) As Usuarios
        If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
            Return Nothing
        End If

        Dim row As DataRow = dataTable.Rows(0)
        Dim usuario As New Usuarios With {
            .Id = Convert.ToInt32(row("ID")),
            .Nombre = Convert.ToString(row("NOMBRE")),
            .Apellido = Convert.ToString(row("APELLIDOS")),
            .Correo = Convert.ToString(row("EMAIL")),
            .Contrasena = Convert.ToString(row("CONTRASENIA")),
            .Rol_ID = Convert.ToInt32(row("ROL_ID"))
        }
        Return usuario
    End Function
End Class
