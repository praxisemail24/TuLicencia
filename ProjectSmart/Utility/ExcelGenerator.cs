using ClosedXML.Excel;
using System.ComponentModel;

namespace SmartLicencia.Utility
{
    public class ExcelGenerator
    {
        public delegate void DrawEvent(IXLCell cell, object meta);
        public const string MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        protected readonly XLWorkbook _workBook;
        private IXLWorksheet? _workSheet;
        public DrawEvent? OnDrawColumn;
        public DrawEvent? OnDrawRow;

        public IXLWorksheet? ActiveWorkSheet
        {
            get
            {
                return _workSheet;
            }
        }

        public ExcelGenerator()
        {
            _workBook = new XLWorkbook();
        }

        public void GenerateExcel<T>(string sheetName, IEnumerable<T> dataList)
        {
            _workSheet = _workBook.AddWorksheet(sheetName);

            int rowIndex = 1;
            int columnIndex = 1;

            Type modelType = typeof(T);
            var columns = new List<ExcelColumn>();

            var propierties = modelType.GetProperties();

            foreach (var property in propierties)
            {
                var attr = property?.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                if (property != null && attr != null)
                {
                    var column = new ExcelColumn();
                    column.Name = property.Name;
                    column.Property = property.Name;
                    column.Name = attr.DisplayName;
                    column.Index = columnIndex;
                    columns.Add(column);
                }
                columnIndex++;
            }

            foreach (var column in columns)
            {
                var cell = _workSheet.Cell(rowIndex, column.Index);
                cell.Value = column.Name;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.Yellow;

                OnDrawColumn?.Invoke(cell, column);
            }

            rowIndex++;

            foreach (T item in dataList)
            {
                if (item != null)
                {
                    foreach (var col in columns)
                    {
                        var cell = _workSheet.Cell(rowIndex, col.Index);
                        var property = item.GetType().GetProperty(col.Property);
                        if (property != null)
                        {
                            var cellValue = property.GetValue(item);

                            if (cellValue != null)
                            {
                                cell.Value = cellValue.ToString();

                                if (property.Name == "Estado")
                                {
                                    cell.Style.Fill.BackgroundColor = ColorEstadoTipo(cell.Value.ToString());
                                }

                                if (property.Name == "EstadoProceso")
                                {
                                    cell.Style.Fill.BackgroundColor = ColorEstadoProceso(cell.Value.ToString());
                                }
                            }
                        }

                        OnDrawRow?.Invoke(cell, new ExcelRow
                        {
                            Column = col,
                            ColIndex = col.Index,
                            RowIndex = rowIndex,
                            Value = item,
                        });
                    }
                }
                rowIndex++;
            }

            _workSheet.Columns().AdjustToContents();
        }

        public Stream ToStream()
        {
            var stream = new MemoryStream();
            _workBook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }


        public XLColor ColorEstadoTipo(string? estado)
        {
            estado = string.IsNullOrWhiteSpace(estado) ? "" : estado.ToLower();
            switch (estado)
            {
                case "new cases":
                    return XLColor.LightPastelPurple;
                case "reviewed cases":
                    return XLColor.LightYellow;
                case "processed cases":
                    return XLColor.LightBlue;
                case "closed cases":
                    return XLColor.LightGreen;
                default:
                    return XLColor.Transparent;
            }
        }

        public XLColor ColorEstadoProceso(string? estado)
        {
            estado = string.IsNullOrWhiteSpace(estado) ? "" : estado.ToLower();
            switch (estado)
            {
                case "pendiente":
                    return XLColor.LightYellow;
                case "aceptado":
                    return XLColor.LightGreen;
                default:
                    return XLColor.Transparent;
            }
        }
    }

    public class ExcelColumn
    {
        public string Name { get; set; }
        public string Property { get; set; }

        public int Index { get; set; }

        public ExcelColumn()
        {
            Name = string.Empty;
            Property = string.Empty;
            Index = -1;
        }
    }

    public class ExcelRow
    {
        public ExcelColumn Column { get; set; }
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public object? Value { get; set; }

        public ExcelRow() 
        {
            Column = new ExcelColumn();
            RowIndex = -1;
            ColIndex = -1;
        }
    }
}
