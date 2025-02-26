using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RunBase_API.Models;

public partial class Felhasznalok
{
    public int Id { get; set; }

    public int? VersenyzoId { get; set; }

    public string Nev { get; set; } = null!;

    [JsonIgnore]
    public string? Jelszo { get; set; }

    public string Tipus { get; set; } = null!;

    public virtual Versenyzo? Versenyzo { get; set; }
}
