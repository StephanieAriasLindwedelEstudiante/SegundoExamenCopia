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
                ' Usuario encontrado, puedes redirigir o realizar otra acción
                Session("UsuarioId") = usuarioCompleto.Id.ToString()
                Session("UsuarioNombre") = usuarioCompleto.Nombre.ToString()
                Session("UsuarioApellido") = usuarioCompleto.Apellido.ToString()
                Session("UsuarioEmail") = usuarioCompleto.Correo.ToString()
                Session("UsuarioRol") = usuarioCompleto.Rol_ID.ToString()
                Return usuarioCompleto
            Else
                ' Usuario no encontrado, manejar el error
                Return Nothing
            End If
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
            Select Case UsuarioCompleto.Rol_ID
                Case 1 ' Administrador
                    Response.Redirect("ClienteAdmin.aspx")
                Case 2 ' Usuario normal
                    Response.Redirect("ClienteUsuarioEstandar.aspx")
                Case Else
                    lblError.Text = "Rol de usuario no reconocido."
                    lblError.Visible = True
            End Select

            lblError.Text = "Usuario o contraseña incorrectos."
            lblError.Visible = True
        End If
    End Sub
End Class