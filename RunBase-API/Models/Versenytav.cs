using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RunBase_API.Models;

namespace Runbase_API.Models;

public partial class Versenytav
{
    public int Tav { get; set; }

    public int VersenyId { get; set; }
    [JsonIgnore]
    public virtual Versenyek? Verseny { get; set; }
 
    }