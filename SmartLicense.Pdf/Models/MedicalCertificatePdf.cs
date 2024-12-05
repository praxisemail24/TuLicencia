using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLicense.Pdf.Models
{
    public class MedicalCertificatePdf : IPdfModel
    {
        public int TrId { get; set; }
        public int FrmId { get; set; }
        public string Titulo { get; set; }
        public string NombreCliente { get; set; }
        public string SegundoNombreCliente { get; set; }
        public string ApellidoPaternoCliente { get; set; }
        public string ApellidoMaternoCliente { get; set; }
        public string SeguroSocial { get; set; }
        public string LicenciaConducir { get; set; }
        public string OjoDerechoSinLentes { get; set; }
        public string OjoIzquierdoSinLentes { get; set; }
        public string OjoDerechoConLentes { get; set; }
        public string OjoIzquierdoConLentes { get; set; }
        public string AmbosOjos { get; set; }
        public string Espejuelos { get; set; }
        public string UsaLentes { get; set; }
        public string Observacion { get; set; }
        public string CondicionOido { get; set; }
        public string CondicionBrazo { get; set; }
        public string CondicionPierna { get; set; }
        public string CondicionFisica { get; set; }
        public string EstadoInconciencia { get; set; }
        public string PadeceCorazon { get; set; }
        public string Marcapaso { get; set; }
        public string Protesis { get; set; }
        public string Peso { get; set; }
        public string EstaturaPies { get; set; }
        public string EstaturaPulgadas { get; set; }
        public string Estado { get; set; }
        public string FechaEvaluacion { get; set; }
        public string NombreMedico { get; set; }
        public string ColorOjos { get; set; }
        public string ColorPelo { get; set; }
        public string CondicionConLentes { get; set; }
        public string CondicionSinLentes { get; set; }
        public string Firma { get; set; }
        public string FirmaDoctor { get; set; }

        public MedicalCertificatePdf()
        {
            Titulo = string.Empty;
            NombreCliente = string.Empty;
            SegundoNombreCliente = string.Empty;
            ApellidoPaternoCliente = string.Empty;
            ApellidoMaternoCliente = string.Empty;
            SeguroSocial = string.Empty;
            LicenciaConducir = string.Empty;
            OjoDerechoSinLentes = string.Empty;
            OjoIzquierdoSinLentes = string.Empty;
            OjoDerechoConLentes = string.Empty;
            OjoIzquierdoConLentes = string.Empty;
            AmbosOjos = string.Empty;
            Espejuelos = string.Empty;
            UsaLentes = string.Empty;
            Observacion = string.Empty;
            CondicionOido = string.Empty;
            CondicionBrazo = string.Empty;
            CondicionPierna = string.Empty;
            CondicionFisica = string.Empty;
            EstadoInconciencia = string.Empty;
            PadeceCorazon = string.Empty;
            Marcapaso = string.Empty;
            Protesis = string.Empty;
            Peso = string.Empty;
            EstaturaPies = string.Empty;
            EstaturaPulgadas = string.Empty;
            Estado = string.Empty;
            FechaEvaluacion = string.Empty;
            NombreMedico = string.Empty;
            ColorOjos = string.Empty;
            ColorPelo = string.Empty;
            CondicionConLentes = string.Empty;
            CondicionSinLentes = string.Empty;
            Firma = "https://img001.prntscr.com/file/img001/zyGTfjQOTyKp-9eqgUiUSw.png";
            FirmaDoctor = "https://img001.prntscr.com/file/img001/zyGTfjQOTyKp-9eqgUiUSw.png";
        }
    }
}
