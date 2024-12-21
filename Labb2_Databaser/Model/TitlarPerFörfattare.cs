using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class TitlarPerFörfattare
{
    public string Namn { get; set; } = null!;

    public int? Ålder { get; set; }

    public int? Titlar { get; set; }

    public decimal? Lagervärde { get; set; }
}
