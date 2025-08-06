Imports System.Data.SqlClient
Public Class login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Si ya está autenticado, redirigir según rol
        If Not Session("UsuarioId") Is Nothing Then
            Dim rol As String = Session("UsuarioRol")?.ToString()
            If rol = "1" Then
                Response.Redirect("ClienteAdmin.aspx")
            ElseIf rol = "2" Then
                Response.Redirect("ClienteUsuarioEstandar.aspx")
            End If
        End If

    End Sub

    Protected Function verificarUsuario(usuario As Usuarios) As Usuarios
        Try
            Dim helper As New DatabaseHelper()
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@Email", usuario.Correo),
                New SqlParameter("@Password", usuario.Contrasena) ' ❌ Inseguro si no es hash
            }

            ' ✅ Consulta explícita con nombres de columna correctos
            Dim query As String = "
                SELECT ID, NOMBRE, APELLIDOS, EMAIL, CONTRASENIA, ROL_ID 
                FROM Usuarios 
                WHERE EMAIL = @Email AND CONTRASENIA = @Password;"

            Dim dataTable As DataTable = helper.ExecuteQuery(query, parametros)

            If dataTable.Rows.Count > 0 Then
                Dim usuarioCompleto As Usuarios = usuario.dtToUsuarios(dataTable)

                ' ✅ Guardar datos en sesión
                Session("UsuarioId") = usuarioCompleto.Id.ToString()
                Session("UsuarioNombre") = usuarioCompleto.Nombre
                Session("UsuarioApellido") = usuarioCompleto.Apellido
                Session("UsuarioEmail") = usuarioCompleto.Correo
                Session("UsuarioRol") = usuarioCompleto.Rol_ID.ToString()

                Return usuarioCompleto
            Else
                Return Nothing
            End If

        Catch ex As Exception
            ' Opcional: loggear el error (ej: escribir en archivo o evento)
            System.Diagnostics.Debug.WriteLine("Error en login: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        ' Limpiar mensaje previo
        lblError.Visible = False

        ' Leer credenciales
        Dim usuarioIn As New Usuarios() With {
            .Correo = txtEmail.Text.Trim(),
            .Contrasena = txtPass.Text
        }

        ' Validar campos
        If String.IsNullOrEmpty(usuarioIn.Correo) OrElse String.IsNullOrEmpty(usuarioIn.Contrasena) Then
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "CamposVacios",
                "Swal.fire('Por favor, ingrese email y contraseña.');",
                True)
            Exit Sub
        End If

        ' Verificar usuario
        Dim usuarioCompleto As Usuarios = verificarUsuario(usuarioIn)

        If usuarioCompleto IsNot Nothing Then
            ' Redirigir según rol
            Select Case usuarioCompleto.Rol_ID
                Case 1 ' Administrador
                    ScriptManager.RegisterStartupScript(
                        Me, Me.GetType(),
                        "AccesoExitoso",
                        "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'ClienteAdmin.aspx'; });",
                        True)

                Case 2 ' Usuario normal
                    ScriptManager.RegisterStartupScript(
                        Me, Me.GetType(),
                        "AccesoExitoso",
                        "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'ClienteUsuarioEstandar.aspx'; });",
                        True)

                Case Else
                    lblError.Text = "Rol no reconocido."
                    lblError.Visible = True
            End Select
        Else
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "ErrorLogin",
                "Swal.fire('Usuario o contraseña incorrectos.');",
                True)
        End If
    End Sub
End Class