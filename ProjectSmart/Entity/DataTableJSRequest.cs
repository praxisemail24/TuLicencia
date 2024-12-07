using Swashbuckle.AspNetCore.Annotations;

namespace SmartLicencia.Entity
{
    public class DataTableJSRequest<TParam>
    {
        public int Draw { get; set; }

        public DataTableColumn[]? Columns { get; set; }

        public DataTableOrder[]? Order { get; set; }

        public int Start { get; set; }

        [SwaggerSchema("Cantidad de elementos por página.")]
        public int Length { get; set; }

        public DataTableSearch? Search { get; set; }

        public string? SortOrder => Columns != null && Order != null && Order.Length > 0
            ? Columns[Order[0].Column].Data +
               (Order[0].Dir == DataTableOrderDir.Desc ? " " + Order[0].Dir : string.Empty)
            : null;

        public TParam AdditionalValues { get; set; }

        public DataTableJSRequest() 
        {
            AdditionalValues = Activator.CreateInstance<TParam>();
        }
    }

    public class DataTableColumn
    {
        public string? Data { get; set; }

        public string? Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public DataTableSearch? Search { get; set; }
    }

    public class DataTableOrder
    {
        public int Column { get; set; }

        public DataTableOrderDir Dir { get; set; }
    }

    public enum DataTableOrderDir
    {
        Asc,
        Desc
    }

    public class DataTableSearch
    {
        public string? Value { get; set; }

        public bool Regex { get; set; }
    }
}
