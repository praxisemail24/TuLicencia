using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLicense.Pdf.Models
{
    public class IndexPdf : IPdfModel
    {
        public string Titulo { get; set; }
        public string Caso { get; set; }
        public string Tramite { get; set; }
        public string Fecha { get; set; }
        public string Formulario { get; set; }
        public List<string> Archivos { get; set; }

        public IndexPdf()
        {
            Titulo = string.Empty;
            Caso = string.Empty;
            Tramite = string.Empty;
            Fecha = string.Empty;
            Formulario = string.Empty;
            Archivos = new List<string>();
        }
    }
}
