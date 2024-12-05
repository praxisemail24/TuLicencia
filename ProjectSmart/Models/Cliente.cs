namespace SmartLicencia.Models
{
    public class Cliente 
    {
        public Pueblos cl_pueblo { get; set; }
        public int cl_id { get; set; }
        public string cl_nombre { get; set; }
        public string cl_segundoNombre { get; set; }
        public string cl_primerApellido { get; set; }
        public string cl_segundoApellido { get; set; }
        public string cl_nombreCompleto { get; set; }
        public string cl_zip { get; set; }
        public string cl_direccion { get; set; }
        public string cl_numeroLicencia { get; set; }
        public string cl_numeroSeguro { get; set; }
        public DateTime cl_fechaNacimiento { get; set; }
        public string cl_numeroTelefono { get; set; }
        public string cl_correo { get; set; }
        public string cl_nombreUsuario { get; set; } //login
        public string cl_contrasena { get; set; }
        public string cl_keyTemporal { get; set; }
        public DateTime cl_fechaRegistro { get; set; }
        public int cl_estado { get; set; }

        // Agregado
        public string cl_genero { get; set; }
        public string cl_talla { get; set; }
        public string cl_peso { get; set; }
        public string cl_tez { get; set; }
        public string cl_colorPelo { get; set; }
        public string cl_colorOjo { get; set; }
        public string cl_codigoPostal { get; set; }
        public string cl_puebloA { get; set; }
        public string token { get; set; }


        public Cliente()
        {
            token = string.Empty;
        }
    }
}
