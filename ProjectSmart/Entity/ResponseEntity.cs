using System.Runtime.Serialization;

namespace SmartLicencia.Entity
{
    public class ResponseEntity<T>
    {

        public IEnumerable<T> items { get; set; }
        public T item { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public object extra { get; set; }

        public ResponseEntity()
        {
            this.success = true;
            this.items = new List<T>();
        }

    }

    public class ResponseEntity
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object extra { get; set; }
    }
}
