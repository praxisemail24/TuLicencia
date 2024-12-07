using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLicense.Pdf.Models
{
    public class RecordChoferilPdf : IPdfModel
    {
        public string Titulo { get; set; }
        public string CertifiedAt { get; set; }
        public string CertifiedType { get; set; }

        public RecordChoferilPdf() 
        { 
            CertifiedAt = string.Empty;
            CertifiedType = string.Empty;
            Titulo = "Record Choferil";
        }
    }
}
