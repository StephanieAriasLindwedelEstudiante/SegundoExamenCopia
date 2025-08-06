Imports System.Data.SqlClient

Public Class ClienteAdmin
    Inherits System.Web.UI.Page

    Private helper As New DatabaseHelper()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Verificar que el usuario esté logueado y sea administrador
            If Session("UsuarioId") Is Nothing OrElse Session("UsuarioRol")?.ToString() <> "1" Then
                Response.Redirect("login.aspx")
                Exit Sub
            End If

            CargarClientes()
        End If
    End Sub

    Protected Sub CargarClientes()
        Try
            Dim idAdmin As Integer = Convert.ToInt32(Session("UsuarioId")) ' ← Aquí estaba el error: "UsuarioID" no existe, es "UsuarioId"

            Dim parameters As New List(Of SqlParameter) From {
                New SqlParameter("@IDAdmin", idAdmin)
            }

            ' Solo los clientes activos (Activo = 1) y registrados por este admin
            Dim query As String = "SELECT ClienteID, Nombre, Apellido, Email, Telefono, Direccion, " &
                                  "CONVERT(varchar, FechaRegistro, 23) AS FechaRegistro, " &
                                  "CONVERT(varchar, FechaEliminacion, 23) AS FechaEliminacion " &
                                  "FROM CLIENTES WHERE ID = @IDAdmin AND Activo = 1;"

            Dim dt As DataTable = helper.ExecuteQuery(query, parameters)
            GvClientes.DataSource = dt
            GvClientes.DataBind()
        Catch ex As Exception
            ' Puedes mostrar un mensaje o registrar el error
            LblMensaje.Text = "Error al cargar clientes: " & ex.Message
            LblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Private Sub LimpiarFormulario()
        TxtNombre.Text = ""
        TxtApellido.Text = ""
        TxtEmail.Text = ""
        TxtTelefono.Text = ""
        TxtDireccion.Text = ""
        TxtFechaRegistro.Text = DateTime.Now.ToString("yyyy-MM-dd")
        ClienteID.Value = ""
        LblMensaje.Text = ""
    End Sub

    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs)
        Try
            Dim idAdmin As Integer = Convert.ToInt32(Session("UsuarioId"))
            Dim fechaRegistro As Date
            Dim fechaEliminacion As Date? = Nothing

            If Not Date.TryParse(TxtFechaRegistro.Text, fechaRegistro) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorFecha", "Swal.fire('Fecha de registro inválida.');", True)
                Exit Sub
            End If


            If String.IsNullOrEmpty(ClienteID.Value) Then
                ' Insertar nuevo cliente
                Dim query As String = "INSERT INTO CLIENTES (Nombre, Apellido, Email, Telefono, Direccion, FechaRegistro, ID, FechaEliminacion, Activo) " &
                                      "VALUES (@Nombre, @Apellido, @Email, @Telefono, @Direccion, @FechaRegistro, @IDAdmin, @FechaEliminacion, 1);"

                Dim parameters As New List(Of SqlParameter) From {
                    New SqlParameter("@Nombre", TxtNombre.Text.Trim()),
                    New SqlParameter("@Apellido", TxtApellido.Text.Trim()),
                    New SqlParameter("@Email", TxtEmail.Text.Trim()),
                    New SqlParameter("@Telefono", TxtTelefono.Text.Trim()),
                    New SqlParameter("@Direccion", TxtDireccion.Text.Trim()),
                    New SqlParameter("@FechaRegistro", fechaRegistro),
                    New SqlParameter("@IDAdmin", idAdmin),
                    New SqlParameter("@FechaEliminacion", If(fechaEliminacion.HasValue, CType(fechaEliminacion.Value, Object), DBNull.Value))
                }

                helper.ExecuteNonQuery(query, parameters)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Exito", "Swal.fire('Cliente agregado exitosamente.');", True)
            Else
                ' Actualizar cliente existente
                Dim cliente As Integer = Convert.ToInt32(ClienteID.Value) ' ✅ Corregido aquí

                Dim query As String = "UPDATE CLIENTES SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, " &
                          "Telefono = @Telefono, Direccion = @Direccion, FechaRegistro = @FechaRegistro, " &
                          "FechaEliminacion = @FechaEliminacion WHERE ClienteID = @ClienteID AND ID = @IDAdmin;"

                Dim parameters As New List(Of SqlParameter) From {
        New SqlParameter("@ClienteID", cliente),
        New SqlParameter("@Nombre", TxtNombre.Text.Trim()),
        New SqlParameter("@Apellido", TxtApellido.Text.Trim()),
        New SqlParameter("@Email", TxtEmail.Text.Trim()),
        New SqlParameter("@Telefono", TxtTelefono.Text.Trim()),
        New SqlParameter("@Direccion", TxtDireccion.Text.Trim()),
        New SqlParameter("@FechaRegistro", fechaRegistro),
        New SqlParameter("@FechaEliminacion", If(fechaEliminacion.HasValue, CType(fechaEliminacion.Value, Object), DBNull.Value)),
        New SqlParameter("@IDAdmin", idAdmin)
    }

                helper.ExecuteNonQuery(query, parameters)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Exito", "Swal.fire('Cliente actualizado exitosamente.');", True)
            End If

            LimpiarFormulario()
            CargarClientes()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Error", "Swal.fire('Error al guardar cliente: " & ex.Message.Replace("'", "\'") & "');", True)
        End Try
    End Sub

    Protected Sub BtnCancelar_Click(sender As Object, e As EventArgs)
        LimpiarFormulario()
    End Sub

    Protected Sub GvClientes_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim row As GridViewRow = GvClientes.SelectedRow

            ClienteID.Value = row.Cells(1).Text

            TxtNombre.Text = row.Cells(2).Text

            ' ✅ Manejar APELLIDO NULL
            Dim apellido As String = row.Cells(3).Text
            If apellido Is Nothing OrElse apellido = "&nbsp;" OrElse String.IsNullOrWhiteSpace(apellido) Then
                TxtApellido.Text = ""
            Else
                TxtApellido.Text = apellido
            End If

            ' ✅ Manejar EMAIL
            Dim email As String = row.Cells(4).Text
            TxtEmail.Text = If(email = "&nbsp;", "", email)

            ' ✅ Manejar TELEFONO
            Dim telefono As String = row.Cells(5).Text
            TxtTelefono.Text = If(telefono = "&nbsp;", "", telefono)

            ' ✅ Manejar DIRECCION
            Dim direccion As String = row.Cells(6).Text
            TxtDireccion.Text = If(direccion = "&nbsp;", "", direccion)

            ' ✅ Manejar FECHA REGISTRO
            If Date.TryParse(row.Cells(7).Text, Nothing) Then
                TxtFechaRegistro.Text = Convert.ToDateTime(row.Cells(7).Text).ToString("yyyy-MM-dd")
            Else
                TxtFechaRegistro.Text = ""
            End If

        Catch ex As Exception
            LblMensaje.Text = "Error al seleccionar cliente: " & ex.Message
            LblMensaje.ForeColor = System.Drawing.Color.Red
        End Try
    End Sub

    Protected Sub GvClientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Try
            Dim clienteId As Integer = Convert.ToInt32(GvClientes.DataKeys(e.RowIndex).Value)
            Dim idAdmin As Integer = Convert.ToInt32(Session("UsuarioId"))
            Dim fechaEliminacion As DateTime = DateTime.Now

            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@ClienteID", clienteId),
                New SqlParameter("@FechaEliminacion", fechaEliminacion),
                New SqlParameter("@IDAdmin", idAdmin)
            }

            ' Solo permite eliminar si el cliente pertenece al admin
            Dim query As String = "UPDATE CLIENTES SET Activo = 0, FechaEliminacion = @FechaEliminacion " &
                                  "WHERE ClienteID = @ClienteID AND ID = @IDAdmin"

            helper.ExecuteNonQuery(query, parametros)

            CargarClientes()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Eliminado", "Swal.fire('Cliente eliminado correctamente.');", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ErrorEliminar", "Swal.fire('Error al eliminar cliente.');", True)
        End Try
    End Sub
End Class