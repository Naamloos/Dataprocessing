using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DatabaseHelper.Models
{
    [Table("authkeys")]
    public class AuthKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("keyowner")]
        public string KeyOwner { get; set; }

        [Column("totalcalls")]
        public int TotalCalls { get; set; } = 0;

        [Column("authkey")]
        public string Key { get; set; }

        [Column("admin")]
        public bool IsAdmin { get; set; } = false;
    }
}
