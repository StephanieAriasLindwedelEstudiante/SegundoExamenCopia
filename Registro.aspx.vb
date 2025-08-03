Imports System.Data.SqlClient

Public Class Registro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LimpiarCampos()
        txtNombre.Text = ""
        txtApellido.Text = ""
        txtEmail.Text = ""
        txtPass.Text = ""
        ddlRol.SelectedValue = "0"
    End Sub
    Protected Function RegistrarUsuario(usuario As Usuarios) As Boolean
        Try
            Dim helper As New DatabaseHelper()
            Dim query As String = "INSERT INTO Usuarios (NOMBRE, APELLIDOS, EMAIL, CONTRASENIA, ROL_ID) VALUES (@Nombre, @Apellido, @Email, @Password, @Rol_ID);"
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Nombre", usuario.Nombre),
                New SqlParameter("@Apellido", usuario.Apellido),
                New SqlParameter("@Email", usuario.Correo),
                New SqlParameter("@Password", usuario.Contrasena),
                New SqlParameter("@Rol_ID", usuario.Rol_ID)
            }
            Dim resultado As Boolean = helper.ExecuteNonQuery(query, parametros)
            Return resultado
        Catch ex As Exception
            ' Manejar el error adecuadamente (registrar en log, mostrar mensaje al usuario, etc.)

            Return False
        End Try
    End Function
    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse
       String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
       String.IsNullOrWhiteSpace(txtPass.Text) OrElse
       ddlRol.SelectedValue = "0" Then
            LimpiarCampos()
            ' Mostrar mensaje de error si los campos obligatorios no están completos
            ScriptManager.RegisterStartupScript(
            Me, Me.GetType(),
            "CamposObligatorios",
            "Swal.fire('Faltan campos obligatorios');",
            True)
            Return
        End If
        Dim rolSeleccionado As Integer = Integer.Parse(ddlRol.SelectedValue)
        Dim usuarioNuevo As New Usuarios() With {
            .Nombre = txtNombre.Text,
            .Apellido = txtApellido.Text,
            .Correo = txtEmail.Text,
            .Contrasena = txtPass.Text,
            .Rol_ID = rolSeleccionado
        }
        If RegistrarUsuario(usuarioNuevo) Then
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistrarUsuarioOk",
                "Swal.fire('Usuario Registrado').then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'Login.aspx';
                    }
                });",
                True)
        Else
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistrarUsuarioError",
                "Swal.fire('Error al registrar el usuario. Inténtalo de nuevo.');",
                True)
        End If
    End Sub
End Class