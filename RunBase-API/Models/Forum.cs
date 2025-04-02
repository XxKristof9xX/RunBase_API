using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RunBase_API.Models
{
    [Table("forum", Schema = "runbase")]
    public class Forum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FelhasznaloId { get; set; }

        [Required]
        public string Tartalom { get; set; } = string.Empty;

        public byte[]? Kep { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Datum { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        [ForeignKey("FelhasznaloId")]
        public virtual Felhasznalok Felhasznalo { get; set; } = null!;
    }
}
