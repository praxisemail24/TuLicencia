namespace SmartLicencia.Entity
{
    public class PaginatorEntity
    {

        private string _sort;
        public int limit { get; set; }
        public int offset { get; set; }

        public string Sort
        {
            get { return this._sort; }
            set
            {
                this._sort = String.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace(".", "").ToLower();
            }
        }

        public string Order { get; set; }
        public int Total { get; set; }
    }

}
