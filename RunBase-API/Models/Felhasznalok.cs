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

    [JsonIgnore]
    public string? Jelszo { get; set; }

    [RegularExpression("^(user|competitor|organizer|administrator)$", ErrorMessage = "Érvénytelen felhasználói típus.")]
    public string Tipus { get; set; } = "user";

    public virtual Versenyzo? Versenyzo { get; set; }
}
