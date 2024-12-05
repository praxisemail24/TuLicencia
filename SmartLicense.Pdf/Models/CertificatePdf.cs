namespace SmartLicense.Pdf.Models
{
    public class CertificatePdf : IPdfModel
    {
        public const int MOTIVO_RENOVACION = 1;
        public const int MOTIVO_DUPLICADO = 2;
        public const int MOTIVO_CAMBIO_NOMBRE = 3;
        public const int MOTIVO_CAMBIO_DIRECCION = 4;
        public const int MOTIVO_DETERIORO = 5;

        public string Titulo { get; set; }
        public string Motivo { get; set; }
        public string NroLicencia { get; set; }
        public string Categoria { get; set; }
        public string VehiculoPesado { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Identificacion { get; set; }
        public string Numero { get; set; }
        public string EstadoLegal { get; set; }
        public string Genero { get; set; }
        public string Donante { get; set; }
        public string TipoSangre { get; set; }
        public string FechaNacDia { get; set; }
        public string FechaNacMes { get; set; }
        public string FechaNacAnio { get; set; }
        public string EstaturaMtrs { get; set; }
        public string EstaturaCm { get; set; }
        public string Peso { get; set; }
        public string NroTelefono { get; set; }
        public string Tez { get; set; }
        public string Pelo { get; set; }
        public string Ojos { get; set; }
        public string DirReferencialUrbanicacion { get; set; }
        public string DirReferencialCalle { get; set; }
        public string DirReferencialPueblo { get; set; }
        public string DirReferencialCodPostal { get; set; }
        public string DirPostalBarrio { get; set; }
        public string DirPostalPueblo { get; set; }
        public string DirPostalCodPostal { get; set; }
        public string LicSuspendida { get; set; }
        public string SuspensionTipo { get; set; }
        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta2Fecha { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta3Fecha { get; set; }
        public string Respuesta4 { get; set; }
        public string Respuesta5 { get; set; }
        public string FechaReg { get; set; }
        public string FechaFin { get; set; }
        public string PaisProcede { get; set; }
        public string EstadoProcede { get; set; }
        public string Var1 { get; set; }
        public string Var2 { get; set; }
        public string Firma { get; set; }

        public CertificatePdf()
        {
            Titulo = string.Empty;
            Motivo = string.Empty;
            NroLicencia = string.Empty;
            Categoria = string.Empty;
            VehiculoPesado = string.Empty;
            PrimerNombre = string.Empty;
            SegundoNombre = string.Empty;
            ApellidoMaterno = string.Empty;
            ApellidoPaterno = string.Empty;
            Identificacion = string.Empty;
            Numero = string.Empty;
            EstadoLegal = string.Empty;
            Genero = string.Empty;
            Donante = string.Empty;
            TipoSangre = string.Empty;
            FechaNacDia = string.Empty;
            FechaNacMes = string.Empty;
            FechaNacAnio = string.Empty;
            EstaturaMtrs = string.Empty;
            EstaturaCm = string.Empty;
            Peso = string.Empty;
            NroTelefono = string.Empty;
            Tez = string.Empty;
            Pelo = string.Empty;
            Ojos = string.Empty;
            DirReferencialUrbanicacion = string.Empty;
            DirReferencialCalle = string.Empty;
            DirReferencialPueblo = string.Empty;
            DirReferencialCodPostal = string.Empty;
            DirPostalBarrio = string.Empty;
            DirPostalPueblo = string.Empty;
            DirPostalCodPostal = string.Empty;
            LicSuspendida = string.Empty;
            SuspensionTipo = string.Empty;
            Respuesta1 = string.Empty;
            Respuesta2 = string.Empty;
            Respuesta2Fecha = string.Empty;
            Respuesta3 = string.Empty;
            Respuesta3Fecha = string.Empty;
            Respuesta4 = string.Empty;
            Respuesta5 = string.Empty;
            FechaReg = string.Empty;
            FechaFin = string.Empty;
            PaisProcede = string.Empty;
            EstadoProcede = string.Empty;
            Var1 = string.Empty;
            Var2 = string.Empty;
            Firma = "https://img001.prntscr.com/file/img001/zyGTfjQOTyKp-9eqgUiUSw.png";
        }
    }
}
