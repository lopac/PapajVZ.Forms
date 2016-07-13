using System;
using System.Collections.Generic;

namespace PapajVZ.Model
{
    public class  Carte 
    {
        public Carte()
        {
            Menus = new List<Menu>();
        }

        public virtual ICollection<Menu> Menus { get; set; }
        public DateTimeOffset DateTime { get; set; }

    }
}