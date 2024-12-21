using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class Kunder
{
    public int KundId { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public string Epost { get; set; } = null!;

    public string? TelefonNummer { get; set; }

    public string Adress { get; set; } = null!;

    public string PostNummer { get; set; } = null!;

    public string Ort { get; set; } = null!;

    public virtual ICollection<Ordrar> Ordrar { get; set; } = new List<Ordrar>();
}
