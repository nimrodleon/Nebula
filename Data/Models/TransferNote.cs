using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class TransferNote
    {
        public int Id { get; set; }

        /// <summary>
        /// Id Almacén de Origen.
        /// </summary>
        public int? OriginId { get; set; }

        /// <summary>
        /// Almacén Origen.
        /// </summary>
        [ForeignKey("OriginId")]
        public Warehouse Origin { get; set; }

        /// <summary>
        /// Id Almacén de destino.
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Almacén Destino.
        /// </summary>
        [ForeignKey("TargetId")]
        public Warehouse Target { get; set; }

        /// <summary>
        /// Motivo de Inventario.
        /// </summary>
        [MaxLength(250)]
        public string Motivo { get; set; }

        /// <summary>
        /// Fecha de registro.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Observación.
        /// </summary>
        [MaxLength(250)]
        public string Remark { get; set; }

        /// <summary>
        /// Estado de la Transferencia.
        /// </summary>
        [MaxLength(250)]
        public string Status { get; set; }

        /// <summary>
        /// Año de Inventario.
        /// </summary>
        [MaxLength(250)]
        public string Year { get; set; }

        /// <summary>
        /// Mes de Inventario.
        /// </summary>
        [MaxLength(250)]
        public string Month { get; set; }

        /// <summary>
        /// Detalles Nota de Trasferencia.
        /// </summary>
        public List<TransferNoteDetail> TransferNoteDetails { get; set; }
    }
}
