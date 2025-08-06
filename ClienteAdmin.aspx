<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ClienteAdmin.aspx.vb" Inherits="SegundoExamenCopia.ClienteAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="ClienteID" runat="server" />

    <div class="row mb-3">

        <div class="col-md-6">
            
            <div class="form-group mb-2">
                <label style="font-family:'Times New Roman'; font-size:larger; color: black; font:bolder" for="TxtNombre">Nombre</label>
                <asp:TextBox ID="TxtNombre" runat="server" CssClass="form-control" MaxLength="50" />
            </div>

            <div class="form-group mb-2">
                <label style="font-family:'Times New Roman'; font-size:larger; color: black; font:bolder" for="TxtApellido">Apellido</label>
                <asp:TextBox ID="TxtApellido" runat="server" CssClass="form-control" MaxLength="50" />
            </div>

            <div class="form-group mb-2">
                <label style="font-family:'Times New Roman'; font-size:larger; color: black; font:bolder" for="TxtEmail">Email</label>
                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" MaxLength="100" TextMode="Email" />
            </div>

            <div class="form-group mb-2">
                <label style="font-family:'Times New Roman'; font-size:larger; color: black; font:bolder" for="TxtTelefono">Teléfono</label>
                <asp:TextBox ID="TxtTelefono" runat="server" CssClass="form-control" MaxLength="15" />
            </div>

            <div class="form-group mb-2">
                <label style="font-family:'Times New Roman'; font-size:larger; color: black; font:bolder" for="TxtDireccion">Dirección</label>
                <asp:TextBox ID="TxtDireccion" runat="server" CssClass="form-control" MaxLength="255" TextMode="MultiLine" Rows="2" />
            </div>

            <div class="form-group mb-2">
                <label style="font-family:'Times New Roman'; font-size:larger; color: black; font:bolder" for="TxtFechaRegistro">Fecha de Registro</label>
                <asp:TextBox ID="TxtFechaRegistro" runat="server" CssClass="form-control" TextMode="Date" />
            </div>

            <div class="form-group mb-3">
                <asp:Button ID="BtnGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar" OnClick="BtnGuardar_Click" />
                <asp:Button ID="BtnCancelar" CssClass="btn btn-secondary ms-2" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
            </div>

            <asp:Label ID="LblMensaje" runat="server" CssClass="text-success fw-bold"></asp:Label>
        </div>
    </div>
    <asp:GridView ID="GvClientes" runat="server" AllowPaging="True"
        OnSelectedIndexChanged="GvClientes_SelectedIndexChanged"
        OnRowDeleting="GvClientes_RowDeleting"
        AutoGenerateColumns="False" DataKeyNames="ClienteID"
        CssClass="table table-bordered table-striped">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="ClienteID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
            <asp:BoundField DataField="FechaRegistro" HeaderText="Registro" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="FechaEliminacion" HeaderText="Eliminación" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

</asp:Content>
