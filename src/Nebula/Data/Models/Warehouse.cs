namespace Nebula.Data.Models
{
    public class Warehouse
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nombre Almacén.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Observación.
        /// </summary>
        public string Remark { get; set; }
    }
}
