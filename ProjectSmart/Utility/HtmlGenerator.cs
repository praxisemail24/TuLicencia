using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace SmartLicencia.Utility
{
    public class HtmlGenerator
    {
        public static string GenerateTable<T>(List<T> items)
        {
            if (items == null || !items.Any())
                return string.Empty;

            var type = typeof(T);
            var properties = type.GetProperties();

            var sb = new StringBuilder();
            sb.AppendLine("<table align=\"left\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">");

            // Generar la cabecera de la tabla
            sb.AppendLine("<thead><tr>");
            foreach (var property in properties)
            {
                var displayAttribute = property?.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                if(displayAttribute != null)
                    sb.AppendLine($"<th style=\"background-color: #ECECEC; margin-left: 2px; margin-right: 2px;\">{displayAttribute.DisplayName}</th>");
            }
            sb.AppendLine("</tr></thead>");

            // Generar las filas de la tabla
            sb.AppendLine("<tbody>");
            foreach (var item in items)
            {
                sb.AppendLine("<tr>");
                foreach (var property in properties)
                {
                    var displayAttribute = property?.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                    if(displayAttribute != null)
                    {
                        var value = property?.GetValue(item, null) ?? string.Empty;
                        sb.AppendLine($"<td>{value}</td>");
                    }
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");

            sb.AppendLine("</table>");
            return sb.ToString();
        }
    }

    public static class HtmlGeneratorExtensions
    {
        public static string ToHtml(this object obj)
        {
            if (obj == null)
                return string.Empty;

            string? str = string.Empty;
            if (obj.GetType() == typeof(decimal))
            {
                str = string.Format("{0:N2}", obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                str = obj.ToString();
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                str = string.Format("{0:MM/dd/yyyy hh:mm}", obj);
            }
            else
            {
                str = Convert.ToString(obj);
            }

            if(string.IsNullOrWhiteSpace(str))
                return string.Empty;

            return str;
        }
    }
}
