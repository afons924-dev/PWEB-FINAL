using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace RestfulAPIWeb.Entities;

public class ModoEntrega
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string? Nome { get; set; }
        [StringLength(200)]
         public string? Detalhe { get; set; }

        //[JsonIgnore]
        //public ICollection<Produto>? produtos { get; set; }
}

