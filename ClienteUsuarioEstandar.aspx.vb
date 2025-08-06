Imports System.Data.SqlClient
Imports System.Runtime.Remoting.Messaging

Public Class ClienteUsuarioEstandar
    Inherits System.Web.UI.Page
    Private helper As New DatabaseHelper()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CargarClientes()
    End Sub
    Protected Sub GvClientes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GvClientes.PageIndexChanging
        GvClientes.PageIndex = e.NewPageIndex
        CargarClientes() ' Vuelve a cargar los datos
    End Sub
    Protected Sub CargarClientes()
        Try
            Dim query As String = "
            SELECT ClienteID, Nombre, Apellido, Email, Telefono, Direccion, 
                   CONVERT(varchar, FechaRegistro, 23) AS FechaRegistro
            FROM CLIENTES 
            WHERE Activo = 1
            ORDER BY Nombre, Apellido"

            Dim dt As DataTable = helper.ExecuteQuery(query)

            If dt.Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Sin datos", "Swal.fire('No hay clientes activos disponibles.');", True)
            End If

            GvClientes.DataSource = dt
            GvClientes.DataBind()

        Catch ex As Exception
            ' 🔥 Muestra el error para depurar
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Error", $"Swal.fire('Error: {ex.Message}');", True)
        End Try
    End Sub
End Class