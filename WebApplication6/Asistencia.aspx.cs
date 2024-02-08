using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace FormularioWeb
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        private readonly DateTime fechaActual = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idcliente = Request.QueryString["idcliente"];
                if (!string.IsNullOrEmpty(idcliente))
                {
                    CargarDatosCliente(idcliente);
                }
                HttpCookie cookie = Request.Cookies.Get("EmpleadoID");

                if (cookie == null)
                {
                    // Si la cookie no existe, crea una nueva
                    cookie = new HttpCookie("EmpleadoID")
                    {
                        Expires = DateTime.Now.AddDays(30), // Expiración de 30 días
                        //Value = "0" // Asigna un valor inicial si es necesario
                    };
                    Response.Cookies.Add(cookie); // Agrega la cookie a la respuesta para que se guarde en el cliente
                }
                // Asigna el valor de la cookie al TextBox
                txtEmpleadoID.Text = !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : "";
            }
        }
        private void CargarDatosCliente(string idcliente)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT id_cliente, nombre FROM tb_cliente WHERE id_cliente = @IDCliente";
                SqlCommand cmd1 = new SqlCommand(query, con);
                cmd1.Parameters.AddWithValue("@IDCliente", idcliente);
                SqlDataReader reader = cmd1.ExecuteReader();
                if (reader.Read())
                {
                    lblidcliente.Text = reader["id_cliente"].ToString();
                    lblidclientenombre.Text = reader["nombre"].ToString();
                }
                con.Close();
            }
        }
        public void BtnConsultarEmpleado_Click(object sender, EventArgs e)
        {

            if (int.TryParse(txtEmpleadoID.Text, out int empleadoID))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "select CONCAT(a.paterno,' ', rtrim(a.materno),' ',a.nombre) as empleado, b.id_cliente as idcliente, c.id_inmueble, c.nombre as " +
                    "sucursal, d.descripcion, c.latitud, c.longitud FROM tb_empleado a inner join tb_cliente b on a.id_cliente = b.id_cliente inner join tb_cliente_inmueble c " +
                    "on a.id_inmueble = c.id_inmueble inner join tb_estado d on c.id_estado = d.id_estado where a.id_empleado = @EmpleadoID and a.id_status=2 ";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nombre = reader["empleado"].ToString();
                                string idcliente = reader["idcliente"].ToString();
                                string sucursal = reader["sucursal"].ToString();
                                string idsucursal = reader["id_inmueble"].ToString();
                                string descripcion = reader["descripcion"].ToString();
                                string latitudbd = reader["latitud"].ToString();
                                string longitudbd = reader["longitud"].ToString();
                                nombreEmpleado.Text = nombre.ToString();
                                Inmueble.Text = sucursal.ToString();
                                idinmuebled.Text = idsucursal.ToString();
                                estado.Text = descripcion.ToString();
                                txtLatitudeCli.Text = latitudbd.ToString();
                                txtLongitudeCli.Text = longitudbd.ToString();
                            }
                        }
                        con.Close();
                    }
                }
            }   
        }
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            string idempleado = txtEmpleadoID.Text;
            string idinmueble = idinmuebled.Text;
            string movimiento = GetSelectedCheckboxOptions();
            string adjuntos = FileUpload1.FileName;
            string respuestaTexto = txtRespuestaTexto.Text;
            double latitude = Double.Parse(txtLatitude.Text, CultureInfo.InvariantCulture);
            double longitude = Double.Parse(txtLongitude.Text, CultureInfo.InvariantCulture);
            double latitudeCli = Double.Parse(txtLatitudeCli.Text, CultureInfo.InvariantCulture);
            double longitudeCli = Double.Parse(txtLongitudeCli.Text, CultureInfo.InvariantCulture);

            double latitudReferencia = latitude; // Latitud
            double longitudReferencia = longitude; // Longitud 

            // Ubicación a verificar en el servidor (latitud y longitud)
            double latitudUbicacion = latitudeCli; // Latitud de la bd
            double longitudUbicacion = longitudeCli; // Longitud la bd

            // Calcular la distancia entre las dos ubicaciones utilizando la fórmula de Haversine
            double distanciaMetros = CalcularDistanciaHaversine(latitudReferencia, longitudReferencia, latitudUbicacion, longitudUbicacion);

            // Validar si la distancia es menor o igual a 400 metros
            if (distanciaMetros <= 400)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "guardadoExitoso", "si400mtrs();", true);
                //return;
                //Console.WriteLine("La ubicación está dentro del radio de 400 metros.");
            }
            else
            {
                //Console.WriteLine("La ubicación está fuera del radio de 400 metros.");
                ScriptManager.RegisterStartupScript(this, GetType(), "guardadoExitoso", "no400mtrs();", true);
                return;
            }
            // Método para calcular la distancia entre dos ubicaciones utilizando la fórmula de Haversine
            double CalcularDistanciaHaversine(double latitud1, double longitud1, double latitud2, double longitud2)
            {
             double radioTierra = 6371; // Radio medio de la Tierra en kilómetros

             double dLat = ToRadians(latitud2 - latitud1);
             double dLon = ToRadians(longitud2 - longitud1);
             double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(latitud1)) * Math.Cos(ToRadians(latitud2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
             double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
             double distancia = radioTierra * c * 1000; // Convertir a metros
             return distancia;
            }
            // Método auxiliar para convertir grados a radianes
            double ToRadians(double grados)
            {
            return grados * Math.PI / 180;
            }       
            if (txtEmpleadoID.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "nombre();", true);
                return;
            }
            if (FileUpload1.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(FileUpload1.FileName);
                    string folderPath = ("C://inetpub//wwwroot//SINGA_APP/Doctos/Asistencia");
                    // Verifica si la carpeta existe, si no, la crea
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string folderName;
                    string monthName = "F" + DateTime.Today.ToString("yyyy_MM");
                    folderName = Path.Combine(folderPath, monthName);
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    string dayName = DateTime.Today.ToString("dd");
                    folderName = Path.Combine(folderPath, monthName, dayName);
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    string filePath = Path.Combine(folderName, fileName);
                    // Guarda el archivo en la carpeta del servidor
                    filePath = filePath.Replace("\\", "/");
                    FileUpload1.SaveAs(filePath);
                    int indiceSINGA_APP = filePath.IndexOf("SINGA_APP");
                    if (indiceSINGA_APP != -1)
                    {
                        int longitudDeseada = indiceSINGA_APP + "SINGA_APP".Length + 1;
                        string rutaDespuesDeSINGA_APP = filePath.Substring(longitudDeseada);
                        rutaDespuesDeSINGA_APP = rutaDespuesDeSINGA_APP.Replace("\\", "/");
                        //Response.Write("Archivo subido con éxito." + rutaDespuesDeSINGA_APP);
                        adjuntos = rutaDespuesDeSINGA_APP;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Error al subir el archivo: " + ex.Message);
                }
            }                   
            InsertarAsistencia(idempleado,idinmueble,movimiento, adjuntos, respuestaTexto, latitude, longitude);      
        }
        private string GetSelectedCheckboxOptions()
        {
            string opcionesSeleccionadas = "";
            if (Radio1.Checked) opcionesSeleccionadas += "A";
            if (Radio9.Checked) opcionesSeleccionadas += "N";

            return opcionesSeleccionadas.TrimEnd(',', ' ');
        }
        protected void InsertarAsistencia(string idempleado, string idinmueble, string movimiento, string adjuntos, string respuestaTexto, double latitude, double longitude)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query3 = "SELECT a.id_empleado, b.id_periodo, CASE WHEN a.formapago = 1 THEN 'Quincenal' when a.formapago=2 then 'Semanal' end as descripcion, b.anio, b.finicio, b.ffin " +
                                "FROM tb_empleado a INNER JOIN tb_periodonomina b ON CASE WHEN a.formapago = 1 THEN 'Quincenal' ELSE 'Semanal' END = b.descripcion " +
                                "WHERE CAST(GETDATE() AS DATE) BETWEEN finicio AND ffin AND a.id_empleado =" + idempleado + ";";
                using (SqlCommand cmd3 = new SqlCommand(query3, con))
                {
                    SqlDataAdapter sdc = new SqlDataAdapter(cmd3);
                    DataTable dtc = new DataTable();
                    sdc.Fill(dtc);

                    foreach (DataRow row in dtc.Rows)
                    {
                        var id = row["id_periodo"].ToString();
                        var descripcion = row["descripcion"].ToString();
                        var anio = row["anio"].ToString();

                        string queryCheck = "SELECT COUNT(*) FROM tb_empleado_asistencia_op WHERE CONVERT(DATE, fecha) = @fecha AND id_empleado = @idempleado";

                        using (SqlCommand cmdCheck = new SqlCommand(queryCheck, con))
                        {
                            cmdCheck.Parameters.AddWithValue("@idempleado", idempleado);
                            cmdCheck.Parameters.AddWithValue("@fecha", fechaActual.Date);
                            cmdCheck.Parameters.AddWithValue("@movi", movimiento);

                            con.Open();
                            int count = (int)cmdCheck.ExecuteScalar();

                            if (count == 0)
                            {
                                string queryInsert = "INSERT INTO tb_empleado_asistencia_op (id_periodo, anio, tipo, id_inmueble, confirma, id_empleado, fecha, movimiento, cubierto, adjuntos, comentarios, latitud, longitud)" +
                                                "VALUES (@idperiodo, @anio, @tipo, @idinmueble, @confirma, @idempleado, @fecha, @movimiento, @cubierto, @adjuntos, @RespuestaTexto, @Latitud, @Longitud)";

                                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, con))
                                {
                                    cmdInsert.Parameters.AddWithValue("@idperiodo", id);
                                    cmdInsert.Parameters.AddWithValue("@anio", anio);
                                    cmdInsert.Parameters.AddWithValue("@tipo", descripcion);
                                    cmdInsert.Parameters.AddWithValue("@idinmueble", idinmueble);
                                    cmdInsert.Parameters.AddWithValue("@confirma", "Web");
                                    cmdInsert.Parameters.AddWithValue("@idempleado", idempleado);
                                    cmdInsert.Parameters.AddWithValue("@fecha", fechaActual);
                                    cmdInsert.Parameters.AddWithValue("@movimiento", movimiento);
                                    cmdInsert.Parameters.AddWithValue("@cubierto", 0);
                                    cmdInsert.Parameters.AddWithValue("@adjuntos", adjuntos);
                                    cmdInsert.Parameters.AddWithValue("@RespuestaTexto", respuestaTexto);
                                    cmdInsert.Parameters.AddWithValue("@Latitud", latitude);
                                    cmdInsert.Parameters.AddWithValue("@Longitud", longitude);
                                    int rowsAffected = cmdInsert.ExecuteNonQuery();
                                }
                                ScriptManager.RegisterStartupScript(this, GetType(), "guardadoExitoso", "nombre();", true);
                                btnEnviar.Enabled = false;
                                Response.Redirect("Default.aspx");
                            }
                           else if (count == 1)
                            {
                                string queryUpdate = "WITH UltimaAsistenciaCTE AS (SELECT confirma,movimiento, fecha, ROW_NUMBER() OVER (ORDER BY fecha DESC) AS Orden FROM tb_empleado_asistencia_op " +
                                                     "WHERE id_empleado =@idempleado) UPDATE UltimaAsistenciaCTE SET confirma='Web', movimiento = @movimiento,fecha =GETDATE()  WHERE Orden = 1";

                                using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@movimiento", movimiento);
                                    cmdUpdate.Parameters.AddWithValue("@idempleado", idempleado);
                                    cmdUpdate.ExecuteScalar();
                                }
                                ScriptManager.RegisterStartupScript(this, GetType(), "guardadoExitoso", "nombre();", true);
                                btnEnviar.Enabled = false;
                                Response.Redirect("Default.aspx");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "guardadoExitoso", "noregistro();", true);
                            }
                        }
                    }
                }
            }
        }
    }
}