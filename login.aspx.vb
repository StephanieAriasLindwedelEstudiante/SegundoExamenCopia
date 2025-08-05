Imports System.Data.SqlClient
Public Class login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Function verificarUsuario(usuario As Usuarios) As Usuarios
        Try
            Dim helper As New DatabaseHelper()
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Email", usuario.Correo),
                New SqlParameter("@Password", usuario.Contrasena)
            }
            Dim query As String = "SELECT * FROM Usuarios WHERE EMAIL = @Email AND CONTRASENIA = @Password;"
            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)
            If dataTable.Rows.Count > 0 Then
                Dim usuarioCompleto As Usuarios = usuario.dtToUsuarios(dataTable)
                Session("UsuarioId") = usuarioCompleto.Id.ToString()
                Session("UsuarioNombre") = usuarioCompleto.Nombre.ToString()
                Session("UsuarioApellido") = usuarioCompleto.Apellido.ToString()
                Session("UsuarioEmail") = usuarioCompleto.Correo.ToString()
                Session("UsuarioRol") = usuarioCompleto.Rol_ID.ToString()
                Return usuarioCompleto
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Protected Function ObtenerClientePorUsuarioID(usuarioID As Integer) As Clientes
        Try
            Dim helper As New DatabaseHelper()
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@UsuarioID", usuarioID)
            }
            Dim query As String = "SELECT * FROM CLIENTES WHERE USUARIO_ID = @UsuarioID AND Activo = 1;"
            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)
            Dim ClienteObj As New Clientes()
            Return ClienteObj.dtToClientes(dataTable)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim usuarioIn As New Usuarios() With {
            .Correo = txtEmail.Text,
            .Contrasena = txtPass.Text
        }
        Dim UsuarioCompleto As Usuarios = verificarUsuario(usuarioIn)
        If UsuarioCompleto IsNot Nothing Then
            Dim cliente As Clientes = ObtenerClientePorUsuarioID(UsuarioCompleto.Id)
            If cliente IsNot Nothing Then
                Session("ClienteId") = cliente.ClienteId
                Session("ClienteNombre") = cliente.Nombre
            End If
            Select Case UsuarioCompleto.Rol_ID
                Case 1 ' Administrador
                    ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistrarUsuarioOk",
                "Swal.fire('Acceso Exitoso').then((result) => {
                If (result.isConfirmed)Then {
                        window.location.href = 'ClienteAdmin.aspx';
                    }
                });",
                True)
                Case 2 ' Usuario normal
                    ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistrarUsuarioOk",
                "Swal.fire('Acceso Exitoso').then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'ClienteUsuarioEstandar.aspx';
                    }
                });",
                True)
                Case Else
                    lblError.Text = "Rol de usuario no reconocido."
                    lblError.Visible = True
            End Select
        Else
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistrarUsuarioError",
                "Swal.fire('Usuario o contraseña incorrectos.');",
                True)
        End If
    End Sub
End Class