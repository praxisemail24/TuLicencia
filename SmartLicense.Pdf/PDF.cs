using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tunaqui.Utils.PdfConvert;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SmartLicense.Pdf.Models;
using System.Text.RegularExpressions;
using System.Text;

namespace SmartLicense.Pdf
{
    public class PDF
    {
        public class PdfConfig
        {
            public bool Debug { get; set; }
            public string? Bin { get; set; }
            public string? TempFolderPath { get; set; }
            public string? CombinerBin { get; set; }
        }

        public class Parameter
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public Parameter(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        private static Lazy<PdfConfig>? _config;

        public static void Initialize(PdfConfig config)
        {
            _config = new Lazy<PdfConfig>(() => config);
        }

        public static async Task<string?> BuilderHtml(string template, params Parameter[] parameters)
        {
            Generator generatorInstance = new Generator(template);
            
            foreach (var param in parameters)
                generatorInstance.SetParameter(param.Name, param.Value);

            return await generatorInstance.Html();
        }

        public static async Task<string?> BuilderHtml(string template, IPdfModel param)
        {
            Generator generatorInstance = new Generator(template);

            generatorInstance.SetParameter("Titulo", param.Titulo);

            if (param.GetType() == typeof(CertificatePdf))
            {
                var certified = (CertificatePdf) Convert.ChangeType(param, typeof(CertificatePdf));

                generatorInstance.SetParameter("Motivo", certified.Motivo);
                generatorInstance.SetParameter("NroLicencia", certified.NroLicencia);
                generatorInstance.SetParameter("Categoria", certified.Categoria);
                generatorInstance.SetParameter("VehiculoPesado", certified.VehiculoPesado);
                generatorInstance.SetParameter("PrimerNombre", certified.PrimerNombre);
                generatorInstance.SetParameter("SegundoNombre", certified.SegundoNombre);
                generatorInstance.SetParameter("ApellidoPaterno", certified.ApellidoPaterno);
                generatorInstance.SetParameter("ApellidoMaterno", certified.ApellidoMaterno);
                generatorInstance.SetParameter("Identificacion", certified.Identificacion);
                generatorInstance.SetParameter("Numero", certified.Numero);
                generatorInstance.SetParameter("EstadoLegal", certified.EstadoLegal);
                generatorInstance.SetParameter("Genero", certified.Genero);
                generatorInstance.SetParameter("Donante", certified.Donante);
                generatorInstance.SetParameter("TipoSangre", certified.TipoSangre);
                generatorInstance.SetParameter("FechaNacDia", certified.FechaNacDia);
                generatorInstance.SetParameter("FechaNacMes", certified.FechaNacMes);
                generatorInstance.SetParameter("FechaNacAnio", certified.FechaNacAnio);
                generatorInstance.SetParameter("EstaturaMtrs", certified.EstaturaMtrs);
                generatorInstance.SetParameter("EstaturaCm", certified.EstaturaCm);
                generatorInstance.SetParameter("Peso", certified.Peso);
                generatorInstance.SetParameter("NroTelefono", certified.NroTelefono);
                generatorInstance.SetParameter("Tez", certified.Tez);
                generatorInstance.SetParameter("Pelo", certified.Pelo);
                generatorInstance.SetParameter("Ojos", certified.Ojos);
                generatorInstance.SetParameter("DirReferencialUrbanicacion", certified.DirReferencialUrbanicacion);
                generatorInstance.SetParameter("DirReferencialCalle", certified.DirReferencialCalle);
                generatorInstance.SetParameter("DirReferencialPueblo", certified.DirReferencialPueblo);
                generatorInstance.SetParameter("DirReferencialCodPostal", certified.DirReferencialCodPostal);
                generatorInstance.SetParameter("DirPostalBarrio", certified.DirPostalBarrio);
                generatorInstance.SetParameter("DirPostalPueblo", certified.DirPostalPueblo);
                generatorInstance.SetParameter("DirPostalCodPostal", certified.DirPostalCodPostal);
                generatorInstance.SetParameter("LicSuspendida", certified.LicSuspendida);
                generatorInstance.SetParameter("SuspensionTipo", certified.SuspensionTipo);
                generatorInstance.SetParameter("Respuesta1", certified.Respuesta1);
                generatorInstance.SetParameter("Respuesta2", certified.Respuesta2);
                generatorInstance.SetParameter("Respuesta2Fecha", certified.Respuesta2Fecha);
                generatorInstance.SetParameter("Respuesta3", certified.Respuesta3);
                generatorInstance.SetParameter("Respuesta3Fecha", certified.Respuesta3Fecha);
                generatorInstance.SetParameter("Respuesta4", certified.Respuesta4);
                generatorInstance.SetParameter("Respuesta5", certified.Respuesta5);
                generatorInstance.SetParameter("PaisProcede", certified.PaisProcede);
                generatorInstance.SetParameter("EstadoProcede", certified.EstadoProcede);
                generatorInstance.SetParameter("Var1", certified.Var1);
                generatorInstance.SetParameter("Var2", certified.Var2);
                generatorInstance.SetParameter("Firma", certified.Firma);
            } else if(param.GetType() == typeof(IndexPdf)) {
                var indice = (IndexPdf)Convert.ChangeType(param, typeof(IndexPdf));

                generatorInstance.SetParameter("Caso", indice.Caso);
                generatorInstance.SetParameter("Tramite", indice.Tramite);
                generatorInstance.SetParameter("Fecha", indice.Fecha);
                generatorInstance.SetParameter("Formulario", indice.Formulario);
                generatorInstance.SetParameter("Archivos", indice.Archivos);
            } else if (param.GetType() == typeof(ImagePdf)) {
                var imagenes = (ImagePdf)Convert.ChangeType(param, typeof(ImagePdf));

                generatorInstance.SetParameter("Imagenes", imagenes.Imagenes);
            } else if(param.GetType() == typeof(MedicalCertificatePdf)) {
                var certMed = (MedicalCertificatePdf)Convert.ChangeType(param, typeof(MedicalCertificatePdf));

                generatorInstance.SetParameter("nombreCliente", certMed.NombreCliente);
                generatorInstance.SetParameter("segundoNombreCliente", certMed.SegundoNombreCliente);
                generatorInstance.SetParameter("apellidoPaternoCliente", certMed.ApellidoPaternoCliente);
                generatorInstance.SetParameter("apellidoMaternoCliente", certMed.ApellidoMaternoCliente);
                generatorInstance.SetParameter("seguroSocial", certMed.SeguroSocial);
                generatorInstance.SetParameter("licenciaConducir", certMed.LicenciaConducir);
                generatorInstance.SetParameter("ojoDerechoSinLentes", certMed.OjoDerechoSinLentes);
                generatorInstance.SetParameter("ojoIzquierdoSinLentes", certMed.OjoIzquierdoSinLentes);
                generatorInstance.SetParameter("ojoDerechoConLentes", certMed.OjoDerechoConLentes);
                generatorInstance.SetParameter("ojoIzquierdoConLentes", certMed.OjoIzquierdoConLentes);
                generatorInstance.SetParameter("ambosOjos", certMed.AmbosOjos);
                generatorInstance.SetParameter("espejuelos", certMed.Espejuelos);
                generatorInstance.SetParameter("usaLentes", certMed.UsaLentes);
                generatorInstance.SetParameter("observacion", certMed.Observacion);
                generatorInstance.SetParameter("condicionOido", certMed.CondicionOido);
                generatorInstance.SetParameter("condicionBrazo", certMed.CondicionBrazo);
                generatorInstance.SetParameter("condicionPierna", certMed.CondicionPierna);
                generatorInstance.SetParameter("condicionFisica", certMed.CondicionFisica);
                generatorInstance.SetParameter("estadoInconciencia", certMed.EstadoInconciencia);
                generatorInstance.SetParameter("padeceCorazon", certMed.PadeceCorazon);
                generatorInstance.SetParameter("marcapaso", certMed.Marcapaso);
                generatorInstance.SetParameter("protesis", certMed.Protesis);
                generatorInstance.SetParameter("peso", certMed.Peso);
                generatorInstance.SetParameter("estaturaPies", certMed.EstaturaPies);
                generatorInstance.SetParameter("estaturaPulgadas", certMed.EstaturaPulgadas);
                generatorInstance.SetParameter("estado", certMed.Estado);
                generatorInstance.SetParameter("fechaEvaluacion", certMed.FechaEvaluacion);
                generatorInstance.SetParameter("nombreMedico", certMed.NombreMedico);
                generatorInstance.SetParameter("colorOjos", certMed.ColorOjos);
                generatorInstance.SetParameter("colorPelo", certMed.ColorPelo);
                generatorInstance.SetParameter("condicionSinLentes", certMed.CondicionSinLentes);
                generatorInstance.SetParameter("condicionConLentes", certMed.CondicionConLentes);
                generatorInstance.SetParameter("Firma", certMed.Firma);
                generatorInstance.SetParameter("FirmaDoctor", certMed.FirmaDoctor);
            } else if (param.GetType() == typeof(BillOfSalePdf))
            {
                var billOfSale = (BillOfSalePdf)Convert.ChangeType(param, typeof(BillOfSalePdf));

                generatorInstance.SetParameter("SellerName", billOfSale.SellerName);
                generatorInstance.SetParameter("SellerAddress", billOfSale.SellerAddress);
                generatorInstance.SetParameter("SellerCity", billOfSale.SellerCity);
                generatorInstance.SetParameter("SellerSignature", billOfSale.SellerSignature);
                generatorInstance.SetParameter("SellerSignatureAt", billOfSale.SellerSignatureAt);
                generatorInstance.SetParameter("BuyerName", billOfSale.BuyerName);
                generatorInstance.SetParameter("BuyerAddress", billOfSale.BuyerAddress);
                generatorInstance.SetParameter("BuyerCity", billOfSale.BuyerCity);
                generatorInstance.SetParameter("BuyerSignature", billOfSale.BuyerSignature);
                generatorInstance.SetParameter("BuyerSignatureAt", billOfSale.BuyerSignatureAt);
                generatorInstance.SetParameter("SerialNumber", billOfSale.SerialNumber);
                generatorInstance.SetParameter("Description", billOfSale.Description);
                generatorInstance.SetParameter("Make", billOfSale.Make);
                generatorInstance.SetParameter("Model", billOfSale.Model);
                generatorInstance.SetParameter("OtherInfo", billOfSale.OtherInfo);
                generatorInstance.SetParameter("PaymentAmount", billOfSale.PaymentAmount);
                generatorInstance.SetParameter("PaymentDate", billOfSale.PaymentDate);
                generatorInstance.SetParameter("NroItem", billOfSale.NroItem);
            }

            return await generatorInstance.Html();
        }

        public static async Task<string> ImageToPdf(string srcUrl, string title)
        {
            if (string.IsNullOrWhiteSpace(srcUrl))
                throw new Exception($"Se requiere dirección fuente de ${title}");

            var tempPath = ResolveTempFileName(title);

            if(srcUrl.EndsWith(".png") || srcUrl.EndsWith(".jpg") || srcUrl.EndsWith(".jpeg") || srcUrl.EndsWith(".png"))
            {
                if (srcUrl.StartsWith("http://") || srcUrl.StartsWith("https://"))
                {
                    string html = $@"<!DOCTYPE html>
                    <html>
	                    <head>
		                    <title>{title}</title>
		                    <meta charset=""utf-8"" />
		                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"" />
		                    <style>
			                    body {{ font-size: 16px; font-family: ""Arial""; }}
			                    .d-block {{ display: block; }}
			                    .d-inline-block {{ display: inline-block; }}
			                    .text-center {{ align-items: center; }}
			                    .text-uppercase {{ text-transform: uppercase; }}
		                    </style>
	                    </head>
	                    <body>
		                    <div class=""d-block"">
			                    <div class=""d-block text-center"" style=""margin-bottom: 30px;"">
				                    <span class=""text-uppercase text-center"" style=""margin: auto;"">Nombre: {title}</span>
			                    </div>
			                    <div class=""d-block text-center"" style=""height: 1250px; max-height: 1250px;"">
				                    <img src=""{srcUrl}"" alt=""{title}"" style=""max-width: 700px; height: auto; max-height: 1200px; margin: auto;"" />
			                    </div>
		                    </div>
	                    </body>
                    </html>";

                    Generate(html, tempPath);
                }
            } else
            {
                if(srcUrl.StartsWith("http://") || srcUrl.StartsWith("https://"))
                    await DownloadFileAsync(srcUrl, tempPath, true);
            }

            return tempPath;
        }

        public static void Generate(string? html, string fileName)
        {
            if (string.IsNullOrWhiteSpace(html))
                throw new Exception("Error al intentar generar el PDF.");

            PdfConvert.ConvertHtmlToPdf(new Tunaqui.Utils.PdfConvert.PdfDocument
            {
                Html = html,
                PageSize = "A4",
            }, new PdfConvertEnvironment
            {
                Debug = _config?.Value?.Debug ?? true,
                Timeout = 40000,
                TempFolderPath = string.IsNullOrWhiteSpace(_config?.Value.TempFolderPath) ? Path.GetTempPath() : _config.Value.TempFolderPath,
                WkHtmlToPdfPath = _config?.Value.Bin,
            }, new PdfOutput
            {
                OutputFilePath = fileName,
            });
        }

        public static void CombinePdfs(string outputFilePath, params string[] pdfFiles)
        {
            if (pdfFiles == null || pdfFiles.Length == 0)
            {
                throw new ArgumentException("No PDF files provided for combining.");
            }

            // Construct the command
            string args = string.Join(" ", pdfFiles) + " cat output " + outputFilePath;

            // Set up the process start info
            var psi = new ProcessStartInfo
            {
                FileName = _config?.Value.CombinerBin,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Execute the process
            using (var process = new Process { StartInfo = psi })
            {
                process.Start();
                process.WaitForExit();

                // Check for errors
                if (process.ExitCode != 0)
                {
                    string error = process.StandardError.ReadToEnd();
                    throw new InvalidOperationException($"PDFtk failed with error: {error}");
                }
            }
        }

        public static async Task DownloadFileAsync(string uri, string outputPath, bool replace = false)
        {
            using (var httpClient = new HttpClient())
            {
                var directory = Path.GetDirectoryName(outputPath);

                if (!Directory.Exists(directory) && !string.IsNullOrWhiteSpace(directory))
                    Directory.CreateDirectory(directory);

                if (!Uri.TryCreate(uri, UriKind.Absolute, out var uriResult))
                    throw new InvalidOperationException("URI is invalid.");

                if (File.Exists(outputPath) && !replace)
                    throw new InvalidOperationException("File already exists.");

                var response = await httpClient.GetAsync(uriResult, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using (var httpStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = File.Create(outputPath))
                {
                    await httpStream.CopyToAsync(fileStream);
                }
            }
        }

        public static async Task SaveBase64AsPng(string base64, string fileName)
        {
            if (base64.Contains(","))
                base64 = base64.Split(",")[1];

            var bytes = Convert.FromBase64String(base64);
            var imageFile = new FileStream(fileName, FileMode.Create);
            imageFile.Write(bytes, 0, bytes.Length);
            imageFile.Flush();
            imageFile.Dispose();

            await Task.Delay(50);
        }

        public static async Task Signature(string pdfPath, string outputPdfPath, string signatureImagePath, float x, float y, int pageNumber = 1, float width = 150, float height = 85)
        {
            using (var reader = new PdfReader(pdfPath))
            using (var fs = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write))
            using (var stamper = new PdfStamper(reader, fs))
            {
                if (pageNumber < 1 || pageNumber > reader.NumberOfPages)
                    throw new ArgumentOutOfRangeException(nameof(pageNumber), "El número de página es inválido.");

                var signatureImage = iTextSharp.text.Image.GetInstance(signatureImagePath);
                signatureImage.SetAbsolutePosition(Math.Abs(x), Math.Abs(y));
                signatureImage.ScaleAbsolute(width, height);

                PdfContentByte content = stamper.GetOverContent(pageNumber);
                content.AddImage(signatureImage);
            }

            await Task.Delay(20);
        }

        #region Private Helpers
        public static string ResolveTempFileName(string name, string? dir = null)
        {
            if (string.IsNullOrWhiteSpace(dir))
                dir = string.Format("{0:yyyyMMdd}", DateTime.Now);

            var tempPath = Path.Combine(Path.GetTempPath(), "TuLicencia", dir);

            if(!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            name = string.Format("{0:HHmmss}-{1}.pdf", DateTime.Now, GenerateSlug(name));

            return Path.Combine(tempPath, name);
        }

        private static string GenerateSlug(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // Convertir a minúsculas
            string slug = input.ToLowerInvariant();

            // Reemplazar caracteres acentuados o especiales
            slug = RemoveDiacritics(slug);

            // Reemplazar cualquier carácter que no sea letra, número o espacio por guiones
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Reemplazar múltiples espacios o guiones consecutivos con un solo guion
            slug = Regex.Replace(slug, @"[\s-]+", "-").Trim('-');

            return slug;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string RandomString(int length = 16)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                result.Append(chars[random.Next(chars.Length)]);

            return result.ToString();
        }
        #endregion
    }
}
