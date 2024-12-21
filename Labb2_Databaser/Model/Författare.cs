using Labb2_Databaser.Model;
using System;
using System.Collections.Generic;

namespace Labb2_Databaser;

public partial class Författare
{
    public int Id { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateOnly Födelsedatum { get; set; }

    public DateOnly? Dödsdatum { get; set; }

    public virtual ICollection<Böcker> Isbn { get; set; } = new List<Böcker>();
    public virtual ICollection<FörfattareBöcker> FörfattareBöcker { get; set; } = new List<FörfattareBöcker>();
}
