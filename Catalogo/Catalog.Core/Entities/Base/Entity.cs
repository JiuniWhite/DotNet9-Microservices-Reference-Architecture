using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Core.Entities.Base
{

    public enum EntityAction
    {
        INSERT = 0,
        UPDATE = 1,
        DELETE = 2
    }


    public abstract class Entity : EntityBase<int>
    {
        [NotMapped]
        public EntityAction Action { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       // [Timestamp]
       // public byte[] TimeStamp { get; set; }

        public bool IsDelete { get; set; } = false;


        /// <summary>
        /// Activo o Visible
        /// </summary>
        public bool IsVisible { get; set; } = true;

        [Required]
        [Column(TypeName = "varchar(80)")]
        public string UserAt { get; set; } = "default";

        //[Required]
        public DateTime DateJoinAt { get; set; } = DateTime.UtcNow;
    }
}

