﻿using Runbase_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RunBase_API.Models;

public partial class Versenyek
{
    [Key]
    public int VersenyId { get; set; }

    public string Nev { get; set; } = null!;

    public string Helyszin { get; set; } = null!;

    public DateOnly Datum { get; set; }

    public string Leiras { get; set; } = null!;

    public int MaxLetszam { get; set; }

    public byte[]? Kep { get; set; }

    [JsonIgnore]
    public virtual ICollection<Versenyindulas> Versenyindulas { get; set; } = new List<Versenyindulas>();
    [JsonIgnore]
    public virtual ICollection<Versenytav> Versenytavs { get; set; } = new List<Versenytav>();
}
