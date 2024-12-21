using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Databaser.Model
{
    public partial class FörfattareBöcker
    {
        public int FörfattareId { get; set; }

        public string Isbn { get; set; } = null!;

        public virtual Författare Författare { get; set; } = null!;

        public virtual Böcker Bok { get; set; } = null!;
    }
}
