Public Class Clientes
    Public Property ClienteId As Integer
    Public Property Nombre As String
    Public Property Apellido As String
    Public Property Correo As String
    Public Property Telefono As String
    Public Property Direccion As String
    Public Property FechaRegistro As DateTime
    Public Property ID As Integer
    Public Property FechaEliminacion As DateTime?
    Public Property Eliminado As Boolean
    Public Sub New()
    End Sub
    Public Function dtToClientes(dataTable As DataTable) As Clientes
        If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
            Return Nothing
        End If
        Dim row As DataRow = dataTable.Rows(0)
        Dim cliente As New Clientes With {
            .ClienteId = Convert.ToInt32(row("ClienteID")),
            .Nombre = Convert.ToString(row("Nombre")),
            .Apellido = Convert.ToString(row("Apellido")),
            .Correo = Convert.ToString(row("Email")),
            .Telefono = Convert.ToString(row("Telefono")),
            .Direccion = Convert.ToString(row("Direccion")),
            .FechaRegistro = Convert.ToDateTime(row("FechaRegistro")),
            .ID = Convert.ToInt32(row("ID")),
            .FechaEliminacion = If(row("FechaEliminacion") IsNot DBNull.Value, Convert.ToDateTime(row("FechaEliminacion")), Nothing),
            .Eliminado = Not Convert.ToBoolean(row("Activo"))
        }
        Return cliente
    End Function
End Class
