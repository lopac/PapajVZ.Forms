using System;

namespace PapajVZ.Model
{

    public class Comment
    {
        
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User User { get; set; }
        public virtual Menu Menu { get; set; }

    }
}