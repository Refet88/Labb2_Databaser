using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class Butiker
{
    public int ButikId { get; set; }

    public string Butiksnamn { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string PostNummer { get; set; } = null!;

    public string Ort { get; set; } = null!;

    public virtual ICollection<LagerSaldo> LagerSaldo { get; set; } = new List<LagerSaldo>();
}
