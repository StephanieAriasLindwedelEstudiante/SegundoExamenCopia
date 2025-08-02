Imports System.Data.SqlClient
Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Function verificarUsuario(usuario As Usuarios) As Boolean
        Try

            Dim helper As New DatabaseHelper()
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Email", usuario.Correo),
                New SqlParameter("@Password", usuario.Contrasena)
            }

            Dim query As String = "SELECT * FROM Usuarios WHERE EMAIL = @Email AND CONTRASENIA = @Password;"
            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)
            If dataTable.Rows.Count > 0 Then
                usuario = usuario.dtToUsuarios(dataTable)
                ' Usuario encontrado, puedes redirigir o realizar otra acción
                Session.Add("UsuarioId", usuario.Id.ToString())
                Session.Add("UsuarioNombre", usuario.Nombre.ToString())
                Session.Add("UsuarioApellido", usuario.Apellido.ToString())
                Session.Add("UsuarioEmail", usuario.Correo.ToString())
                Session.Add("UsuarioRol", usuario.Rol_ID.ToString())
                Return True
            Else
                ' Usuario no encontrado, manejar el error
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim usuario As New Usuarios() With {
            .Correo = txtEmail.Text,
            .Contrasena = txtPass.Text
        }
        If verificarUsuario(usuario) Then
            Response.Redirect("Default.aspx")
        Else

            lblError.Text = "Usuario o contraseña incorrectos."
            lblError.Visible = True
        End If
    End Sub
End Class