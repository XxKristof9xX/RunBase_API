using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RunBase_API.Models;

public partial class Versenyzo
{
    public int VersenyzoId { get; set; }

    public string Nev { get; set; } = null!;

    public int SzuletesiEv { get; set; }

    public string Neme { get; set; } = null!;

    public string TajSzam { get; set; } = null!;
    [JsonIgnore]
    public virtual Felhasznalok? Felhasznalok { get; set; }
    [JsonIgnore]
    public virtual ICollection<Versenyindulas> Versenyindulas { get; set; } = new List<Versenyindulas>();
}
