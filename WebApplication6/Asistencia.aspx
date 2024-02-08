 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asistencia.aspx.cs" Inherits="FormularioWeb.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Formulario de Asistencia</title>

   <%-- <link rel="icon" type="image/png" href="img/logo.png" />--%>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function nombre() {
            Swal.fire({
                icon: 'error',
                title: '¡Error!',
                text: 'Agrega tu ID de Empleado.',
                showConfirmButton: true
            });
        }
        function noregistro() {
            Swal.fire({
                icon: 'warning',
                title: '¡Ups!',
                text: 'Ya estas registrado.',
                timer: 1200, // Mostrará el mensaje durante 5 segundos
                timerProgressBar: true,
                showConfirmButton: false
            });
        }
        function si400mtrs() {
            Swal.fire({
                icon: 'success',
                title: 'Ok',
                text: 'La ubicación está dentro del radio de 400 metros',
                timer: 2800, // Mostrará el mensaje durante 5 segundos
                timerProgressBar: true,
                showConfirmButton: false
            });
        }
        function no400mtrs() {
            //Swal.fire({
            //    icon: 'error',
            //    title: 'error',
            //    text: 'La ubicación no está dentro del radio de 400 metros',
            //    //timer: 2800, // Mostrará el mensaje durante 5 segundos
            //    //timerProgressBar: true,
            //    showConfirmButton: true
            //});
            alert("Ok");
            $("#FacNew").modal('show');

        }
    </script>
    <style>
        header {
            position: relative;
            width: 100%;
            top: 0;
            left: 400px;
            height: 200%;
        }
        body {
            background-image: url(img/fondo.png);
            background-size: cover; 
            color: #2E3192;
            font-family: sans-serif;
        }
    </style>
    <script>

        $(document).ready(function () {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var latitude = position.coords.latitude; 
                    var longitude = position.coords.longitude; 
                    $("#txtLatitude").val(latitude);
                    $("#txtLongitude").val(longitude);
                    //alert("Ubicación obtenida con éxito:\nLatitud: " + latitude + "\nLongitud: " + longitude);
                });
            } else {
                alert("Geolocalización no es compatible con este navegador.");
            }
        });
        $(document).ready(function () {
            $('#<%= btnEnviar.ClientID %>').click(function () {
                 // Verificar si Label4, Label5 y Label6 tienen algún valor
                 var nombreEmpleado = $('#<%= nombreEmpleado.ClientID %>').text().trim();
            var Inmueble = $('#<%= Inmueble.ClientID %>').text().trim();
            var estado = $('#<%= estado.ClientID %>').text().trim();

            if (nombreEmpleado.length === 0 || Inmueble.length === 0 || estado.length === 0) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Atencion',
                    text: 'Por favor, completa la información antes de continuar.',
                    timer: 2800, // Mostrará el mensaje durante 5 segundos
                    timerProgressBar: true,
                    showConfirmButton: false
                });
                // Detener la ejecución si algún campo está vacío
                return false;
            }
        });
         });
    </script>
    <script type="C#">
         void SubmitBtn_Click(Object Sender, EventArgs e) 
            {
                if (Radio1.Checked) {
                    Label1.Text = "A Asistencia" + Radio1.Text;
                }
                else if (Radio2.Checked) {
                    Label1.Text = "D Doblete " + Radio2.Text;
                }
                else if (Radio3.Checked) {
                    Label1.Text = "Tiempo Extra" + Radio3.Text;
                }
                else if (Radio4.Checked) {
                    Label1.Text = "Salida" + Radio4.Text;
                }
                else if (Radio5.Checked) {
                    Label1.Text = "F Falta" + Radio5.Text;
                }
                else if (Radio6.Checked) {
                    Label1.Text = "V Vacaciones" + Radio6.Text;
                }
                else if (Radio7.Checked) {
                    Label1.Text = "IEG Incapacidad General" + Radio7.Text;
                }
                else if (Radio8.Checked) {
                    Label1.Text = "IRT Incapcidad Riesgo de trabajo" + Radio8.Text;
                }
                else if (Radio9.Checked) {
                     Label1.Text = "N Descanso" + Radio8.Text;
                }
           }
    </script>
</head>
<body>
   <%-- <header>
        <img src="img/logonew.png" align="left" width="100" height="120" />
    </header>--%>
    <br />
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="row">
                <div class="col-md-6 offset-md-3">
                    <h1 align="center">Biometa Grupo Batia</h1>
                    <br />
                    <div class="form-group">
                        <strong>
                            <asp:Label ID="lblClienteID" runat="server" Text="Id Cliente: "></asp:Label>
                            <asp:Label ID="lblidcliente" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:Label ID="lblClienteNombre" runat="server" Text="Nombre del Cliente:"></asp:Label>
                            <asp:Label ID="lblidclientenombre" runat="server" Text=""></asp:Label>
                        </strong><br />
                        <hr />
                        <p>Ingresa tu ID de Empleado:</p>
                        <div class="row g-3 align-items-center">
                           <div class="col-auto">
                                <asp:TextBox ID="txtEmpleadoID" runat="server" class="col-form-label"></asp:TextBox>
                                <asp:Button ID="btnMostrarInfo" runat="server" class="btn btn-info" Text="Consultar" OnClick="BtnConsultarEmpleado_Click" />
                           </div>
                        </div>
                        <strong>
                        <asp:Label ID="Label4" runat="server" Text="Nombre: "></asp:Label>
                        <asp:Label ID="nombreEmpleado" runat="server"> </asp:Label><br />
                        <asp:Label ID="Label5" runat="server" Text="Sucursal o Area: "></asp:Label>
                        <asp:Label ID="Inmueble" runat="server"></asp:Label><br />
                        <asp:TextBox ID="idinmuebled" runat="server" type="hidden"></asp:TextBox>
                        <asp:Label ID="Label6" runat="server" Text="Ciudad o Estado: "></asp:Label>
                        <asp:Label ID="estado" runat="server"></asp:Label>
                        </strong>
                        <br /> <hr />
                        <label>Selecciona solo una opción de envio:</label><br />
                        <asp:RadioButton ID="Radio1" Text="A Asistencia" Checked="True" GroupName="RadioGroup1" runat="server" /><br />
                        <asp:RadioButton ID="Radio9" Text="N Descanso" GroupName="RadioGroup1" runat="server" /><br />
                        <br />
                    </div>
                    <asp:TextBox ID="txtLatitude" runat="server" type="hidden"></asp:TextBox>
                    <asp:TextBox ID="txtLongitude" runat="server" type="hidden"></asp:TextBox>
                    <asp:TextBox ID="txtLatitudeCli" runat="server" type="hidden"></asp:TextBox>
                    <asp:TextBox ID="txtLongitudeCli" runat="server" type="hidden"></asp:TextBox>
                    <div class="mb-3">
                        <label for="fileAdjunto" class="form-label">Adjunta: Lista de Asistencia,Incapacidad,formato de vacaciones,justificantes,etc.   </label>
                        <asp:FileUpload ID="FileUpload1" runat="server" class="form-control" type="file" />
                        <br />
                    </div>
                    <div>
                        <asp:TextBox ID="txtRespuestaTexto" class="form-control" placeholder="Captura informacion adicional" runat="server" Style="height: 50px"></asp:TextBox><br />
                        <br />
                    </div>
                    
                    <div class="mb-3" align="center">
                        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" class="btn btn-info" OnClick="BtnEnviar_Click" />
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="FacNew" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
          <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">
              <div class="modal-header ui-state-hover">
                  <table><tr><td><input id="txtIdUsuTicket" type="text" disabled="disabled" style="border-style: none; width:50px; text-align:center; font-size: medium;" /></td>
                      <td><h5 class="modal-title text-secondary" id="staticBackdropLabel"><strong><span id="UsuTicket">&nbsp; UsuTicket </span></strong></h5>
                      </td></tr></table>
                
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
                <div class="modal-body ui-widget-content">
                            <div class="container row mb-2">
                                <div class="col-md" id="EmpleNewTicket" style="border: thin solid silver; display:none">
                                    <table>
                                        <tr>
                                            <td >Ingresar Nombre o #Empleado :</td>
                                            <td ><input id="txtValBusca" type="text"  style="width:98%;font-size:11px;" class="MaxLen" maxlength="50" /></td><td>
                                                <span class="ui-icon ui-icon-search" id="BtnBuscarAgent" style="cursor: pointer" title="Configurar tema"></span></td>
                                        </tr>
                                    </table>
                                    <table style="width:100%; font-size: 11px; display:none" id="tblListEmple" >
                                        <tr>
                                            <td>
                                                <table id="GridAgentH" style="width:100%;border-collapse:collapse;" >
                                                    <tr class="ui-state-active">
                                                    <th style="width:95px">Num.</th>
                                                    <th >Nombre</th>
                                                    </tr>
                                                </table>
                                                <div id="ContenedorListaAgent" style="overflow:auto;height:80px;width:100%; vertical-align:top">
                                                    <table id="GridAgent" class="tablesorter" border="0" style="width:100%;margin: 0 auto;">
                                                        <tr><td style="text-align:center; width:90px"></td><td style="text-align:center;"></td></tr>
                                                    </table>
                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:right"> <div id="HelpTitle" style="color:silver ; font-size: 0.8em; display: table-row none" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                        </div>
              <div class="modal-footer ui-state-hover">
                  <table style="width:100%"><tr><td style="width:40%;" > &nbsp; <div style="cursor:pointer; display:none" id="cmdUpdAgent"><strong>Buscar empleado... </strong><span class="ui-icon ui-icon-person"  title="Click para ingresar Agente"></span></div></td>
                      <td style="width:20%;text-align:center"><button type="button" class="btn btn-secondary"  id="cmdCerrar">Cancelar</button></td>
                      <td  style="width:20%;text-align:center"><button type="button" class="btn btn-primary" id="btnGuardar">Enviar</button></td>
                      <td  style="width:20%;text-align:center;display:none"><button type="button" class="btn btn-light text-muted" id="btnCerrar">Cerrar Ticket</button></td>
                                            </tr>
                  </table>
                  
              </div>
            </div>
          </div>
        </div>

    </form>
</body>
</html>
