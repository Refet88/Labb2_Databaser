using System;
using System.Collections.Generic;

namespace Labb2_Databaser.Model;

public partial class Ordrar
{
    public int OrderId { get; set; }

    public int? KundId { get; set; }

    public DateOnly OrderDatum { get; set; }

    public virtual Kunder? Kund { get; set; }

    public virtual ICollection<OrderDetaljer> OrderDetaljer { get; set; } = new List<OrderDetaljer>();
}
