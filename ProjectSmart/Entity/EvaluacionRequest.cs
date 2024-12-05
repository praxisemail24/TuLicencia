namespace SmartLicencia.Entity
{
    public class EvaluacionRequest
    {
        public int Id { get; set; }
        public int tr_id { get; set; }
        
        public string OjoDerechoSinLentes { get; set; }
        public string OjoIzquierdoSinLentes { get; set; }
        public string OjoDerechoConLentes { get; set; }
        public string OjoIzquierdoConLentes { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public string NombreMedico { get; set; }
        public string LicenciaMedico { get; set; }

        // Campos adicionales
        public string NombreCliente { get; set; }
        public string SegundoNombreCliente { get; set; }
        public string ApellidoPaternoCliente { get; set; }
        public string ApellidoMaternoCliente { get; set; }
        public string SeguroSocial { get; set; }
        public string LicenciaConducir { get; set; }
        // Campos adicionales para la evaluación médica
        public string Condicion { get; set; }
        public string CondisionConLentes { get; set; }
        public string CondisionSinLentes { get; set; }
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
        public string ColorOjo { get; set; }
        public string ColorPelo { get; set; }
        public string EstaturaPies { get; set; }
        public string EstaturaPulgadas { get; set; }
    }
}
