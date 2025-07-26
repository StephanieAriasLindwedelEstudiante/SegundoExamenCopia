Public Class Usuarios
    Public Property Id As Integer
    Public Property Nombre As String
    Public Property Correo As String
    Public Property Contrasena As String
    Public Property Activo As Boolean
    Public Property Rol_ID As Integer
    Public Sub New()
    End Sub
    Public Function dtToUsuarios(dt As DataTable) As List(Of Usuarios)

    End Function
End Class
