using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class Förlag
{
    public int FörlagId { get; set; }

    public string Namn { get; set; } = null!;

    public string? TelefonNummer { get; set; }

    public string Adress { get; set; } = null!;

    public string PostNummer { get; set; } = null!;

    public string Ort { get; set; } = null!;

    public virtual ICollection<Böcker> Böcker { get; set; } = new List<Böcker>();
}
