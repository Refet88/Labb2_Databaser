using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class Böcker
{
    public string Isbn13 { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public string Språk { get; set; } = null!;

    public decimal Pris { get; set; }

    public DateOnly Utgivningsdatum { get; set; }

    public int? AntalSidor { get; set; }

    public int? FörlagId { get; set; }

    public virtual Förlag? Förlag { get; set; }

    public virtual ICollection<LagerSaldo> LagerSaldo { get; set; } = new List<LagerSaldo>();

    public virtual ICollection<OrderDetaljer> OrderDetaljer { get; set; } = new List<OrderDetaljer>();

    public virtual ICollection<Författare> Författare { get; set; } = new List<Författare>();

    public virtual ICollection<FörfattareBöcker> FörfattareBöcker { get; set; } = new List<FörfattareBöcker>();

}
