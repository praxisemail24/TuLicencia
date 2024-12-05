namespace SmartLicencia.Utility
{
    public class Logger
    {
        public static void Write(string path, string message)
        {
            try
            {
                var dirBase = Path.GetDirectoryName(path);

                if (!Directory.Exists(dirBase) && !string.IsNullOrWhiteSpace(dirBase))
                    Directory.CreateDirectory(dirBase);

                if (!File.Exists(path))
                    File.Create(path);

                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar registrar en el archivo log ({path}): {ex.Message}");
            }
        }

        public static string CurrentFileName()
        {
            return string.Format("Logs/log_{0:ddMMyyyy}.txt", DateTime.Now);
        }


        public static void Log(string message)
        {
            Write(CurrentFileName(), message);
        }

        public static void Error(Exception ex)
        {
            Write(CurrentFileName(), string.Format("Error: {0:dd/MM/yyyy HH:mm:ss} - {1} - StackTrace: {2}", DateTime.Now, ex.Message, ex.StackTrace));
        }
    }
}
