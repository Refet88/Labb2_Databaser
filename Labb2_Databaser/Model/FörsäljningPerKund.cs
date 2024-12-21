using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class FörsäljningPerKund
{
    public int KundId { get; set; }

    public string KundNamn { get; set; } = null!;

    public int? AntalOrdrar { get; set; }

    public decimal? TotalFörsäljning { get; set; }
}
