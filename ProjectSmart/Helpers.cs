namespace SmartLicencia
{
    public class Helpers
    {
        public enum TypeName
        {
            DATE = 1,
            RANGE = 2,
            MONTH = 3,
        }

        public static string MonthName(int index)
        {
            var months = new string[] { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };
            return months[index - 1];
        }

        public static string ExcelFileName(TypeName type, string prefix, params object[] args)
        {
            string fileName = string.Empty;
            prefix = prefix.Replace(" ", "_");
            switch (type)
            {
                case TypeName.DATE:
                    fileName = string.Format("{0}_{1:dd_MM_yyyy}", prefix, args[0]);
                    break;
                case TypeName.RANGE:
                    fileName = string.Format("{0}_{1:dd_MM_yyyy}_al_{2:dd_MM_yyyy}", prefix, args[0], args[1]);
                    break;
                case TypeName.MONTH:
                    fileName = string.Format("{0}_{1}_del_{2}", prefix, MonthName(Convert.ToInt32(args[0])), args[1]);
                    break;
                default:
                    fileName = string.Format("{0}_{1:dd_MM_yyyy}", prefix, DateTime.Now);
                    break;
            }

            return $"{fileName}.xlsx";
        }
    }
}
