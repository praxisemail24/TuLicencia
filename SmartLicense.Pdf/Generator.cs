using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;
using System.Reflection;

namespace SmartLicense.Pdf
{
    public class Generator
    {
        private readonly TemplateGenerator _template;
        private string _tempTemplatePath;

        public string TemplatePath { get; set; }

        public Generator(string template)
        {
            if (!File.Exists(template))
                throw new FileNotFoundException("Archivo de plantilla no encontrada.");

            TemplatePath = template;
            _template = new TemplateGenerator();

            _tempTemplatePath = $"{Path.GetDirectoryName(TemplatePath)}\\{Path.GetFileNameWithoutExtension(TemplatePath)}.txt";
        }

        public void SetParameter(string name, object value)
        {
            _template.GetOrCreateSession().Add(name, value);
        }

        public async Task<string?> Html()
        {
            string htmlTemplate = string.Empty;

            //var assembly = Assembly.GetExecutingAssembly();

            //File.Copy(assembly.Location, "SmartLicense.Pdf.dll");

            //_template.Refs.Add("$(SolutionDir)SmartLicense\\bin\\Debug\\net6.0\\SmartLicense.Pdf.dll");
            //_template.Refs.Add(assembly.Location);

            //_template.IncludePaths.Add("$(SolutionDir)SmartLicense\\bin\\Debug\\net6.0");
            //_template.IncludePaths.Add(Path.GetDirectoryName(assembly.Location));

            //_template.Imports.Add("SmartLicense.Pdf");

            var processed = await _template.ProcessTemplateAsync(TemplatePath, _tempTemplatePath);

            if (!processed)
                throw new InvalidOperationException("Error al intentar procesar plantilla.");

            htmlTemplate = File.ReadAllText(_tempTemplatePath);

            File.Delete(_tempTemplatePath);

            return htmlTemplate;
        }
    }
}
