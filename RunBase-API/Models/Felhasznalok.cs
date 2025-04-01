using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RunBase_API.Models;

public partial class Felhasznalok
{
    public int Id { get; set; }

    public int? VersenyzoId { get; set; }

    public string Nev { get; set; } = null!;

    public string? Jelszo { get; set; }

    [RegularExpression("^(user|competitor|organizer|admin)$", ErrorMessage = "Érvénytelen felhasználói típus.")]
    public string Tipus { get; set; } = "user";

    public virtual Versenyzo? Versenyzo { get; set; }
    public virtual ICollection<Forum> ForumBejegyzesek { get; set; } = new List<Forum>();
}
