using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLicense.Pdf.Models
{
    public class ImagePdf : IPdfModel
    {
        public string Titulo { get; set; }
        public Dictionary<string, string> Imagenes { get; set; }

        public ImagePdf()
        {
            Titulo = string.Empty;
            Imagenes = new Dictionary<string, string>();
        }

        public ImagePdf(IEnumerable<ItemImagen> images)
        {
            Titulo = string.Empty;
            Imagenes = new Dictionary<string, string>();

            foreach (var img in images)
            {
                Imagenes.Add(img.Nombre, img.Url ?? string.Empty);
            }
        }

        public static Lazy<List<ItemImagen>> NombresArchivos = new Lazy<List<ItemImagen>>(() =>
        {
            var listaNombres = new List<ItemImagen>();
            listaNombres.Add(new ItemImagen { Position = 1, TipoTramite = 1, Nombre = "Tarjeta de Seguro Social" });
            listaNombres.Add(new ItemImagen { Position = 2, TipoTramite = 1, Nombre = "Foto ID (Expirada o por expirar) - Frontal" });
            listaNombres.Add(new ItemImagen { Position = 5, TipoTramite = 1, Nombre = "Foto ID(Expirada o por expirar) – Posterior" });
            listaNombres.Add(new ItemImagen { Position = 3, TipoTramite = 1, Nombre = "Recibo de Luz o Estado Bancario" });
            listaNombres.Add(new ItemImagen { Position = 4, TipoTramite = 1, Nombre = "Certificado de Nacimiento o Prueba de Residencia" });
            //listaNombres.Add(new ItemImagen { Position = 6, TipoTramite = 1, Nombre = "Copia de Plantilla o W2" });
            listaNombres.Add(new ItemImagen { Position = 10, TipoTramite = 1, Nombre = "Foto Selfie" });
            listaNombres.Add(new ItemImagen { Position = 20, TipoTramite = 1, Nombre = "Certificación de Authorización" });
            listaNombres.Add(new ItemImagen { Position = 30, TipoTramite = 1, Nombre = "Firma" });
            listaNombres.Add(new ItemImagen { Position = 50, TipoTramite = 1, Nombre = "Evaluación Médica" });

            listaNombres.Add(new ItemImagen { Position = 1, TipoTramite = 3, Nombre = "Tarjeta de Seguro Social" });
            listaNombres.Add(new ItemImagen { Position = 2, TipoTramite = 3, Nombre = "Foto ID - Anverso" });
            listaNombres.Add(new ItemImagen { Position = 5, TipoTramite = 3, Nombre = "Foto ID - Reverso" });
            listaNombres.Add(new ItemImagen { Position = 3, TipoTramite = 3, Nombre = "Recibo de Luz o Estado Bancario" });
            listaNombres.Add(new ItemImagen { Position = 4, TipoTramite = 3, Nombre = "Declaración Jurada ante Notario Público" });
            listaNombres.Add(new ItemImagen { Position = 6, TipoTramite = 3, Nombre = "Certificado de Nacimiento o Prueba de Residencia" });
            listaNombres.Add(new ItemImagen { Position = 10, TipoTramite = 3, Nombre = "Foto Selfie" });
            listaNombres.Add(new ItemImagen { Position = 20, TipoTramite = 3, Nombre = "Certificación de Authorización" });
            listaNombres.Add(new ItemImagen { Position = 30, TipoTramite = 3, Nombre = "Firma" });
            listaNombres.Add(new ItemImagen { Position = 50, TipoTramite = 3, Nombre = "Evaluación Médica" });

            listaNombres.Add(new ItemImagen { Position = 1, TipoTramite = 4, Nombre = "Tarjeta de Seguro Social" });
            listaNombres.Add(new ItemImagen { Position = 2, TipoTramite = 4, Nombre = "Certificado de Nacimiento" });
            listaNombres.Add(new ItemImagen { Position = 6, TipoTramite = 4, Nombre = "Record Choferil de su Estado de Procedencia" });
            listaNombres.Add(new ItemImagen { Position = 3, TipoTramite = 4, Nombre = "Licencia vigente de su país – Frontal" });
            listaNombres.Add(new ItemImagen { Position = 5, TipoTramite = 4, Nombre = "Licencia vigente de su país -  Posterior" });
            listaNombres.Add(new ItemImagen { Position = 4, TipoTramite = 4, Nombre = "Recibo de Agua o Estado Bancario" });
            listaNombres.Add(new ItemImagen { Position = 10, TipoTramite = 4, Nombre = "Foto Selfie" });
            listaNombres.Add(new ItemImagen { Position = 20, TipoTramite = 4, Nombre = "Certificación de Authorización" });
            listaNombres.Add(new ItemImagen { Position = 30, TipoTramite = 4, Nombre = "Firma" });
            listaNombres.Add(new ItemImagen { Position = 50, TipoTramite = 4, Nombre = "Evaluación" });

            listaNombres.Add(new ItemImagen { Position = 1, TipoTramite = 5, Nombre = "Copia del licencia del vehículo" });
            listaNombres.Add(new ItemImagen { Position = 2, TipoTramite = 5, Nombre = "Copia de licencia de conducir de vendedor" });
            listaNombres.Add(new ItemImagen { Position = 3, TipoTramite = 5, Nombre = "Copia de licencia de conducir de comprador" });
            listaNombres.Add(new ItemImagen { Position = 4, TipoTramite = 5, Nombre = "Título de propiedad" });
            listaNombres.Add(new ItemImagen { Position = 5, TipoTramite = 5, Nombre = "Contrato de compraventa (Bill of Sale)" });
            listaNombres.Add(new ItemImagen { Position = 6, TipoTramite = 5, Nombre = "Declaración jurada" });

            return listaNombres;
        });


        public static string TituloImg(int trId, int index)
        {
            string titulo = string.Empty;

            if (index < 7)
            {
                var img = NombresArchivos.Value.Find(x => x.Position == index && x.TipoTramite == trId);

                if (img == null)
                    return string.Empty;

                return img.Nombre;
            }
            else
            {
                if (index == 10)
                    titulo = "Foto Selfie";
                if (index == 20)
                    titulo = "Certificado de Autorización";
                if (index == 30)
                    titulo = "Firma";
                if (index == 100)
                    titulo = "FirmaDoctor";
            }

            return titulo;
        }
    }

    public class ItemImagen
    {
        public int Position { get; set; }
        public int TipoTramite { get; set; }
        public string Nombre { get; set; }
        public string? Url { get; set; }
        public int FrmId { get; set; }

        public ItemImagen()
        {
            FrmId = -1;
            Nombre = string.Empty;
        }
    }
}
