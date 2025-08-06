<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ClienteUsuarioEstandar.aspx.vb" Inherits="SegundoExamenCopia.ClienteUsuarioEstandar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GvClientes" runat="server"
        AllowPaging="True"
        AutoGenerateColumns="False"
        CssClass="table table-bordered table-striped"
        EmptyDataText="No hay clientes activos disponibles.">
        <Columns>
            <asp:BoundField DataField="ClienteID" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
            <asp:BoundField DataField="FechaRegistro" HeaderText="Registro" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
</asp:Content>
