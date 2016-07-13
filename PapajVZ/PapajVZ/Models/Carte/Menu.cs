using System.Collections.Generic;


namespace PapajVZ.Model
{
    public enum MenuType
    {
        Lunch,
        Dinner
    };
    public class Menu 
    {
   
        public int MenuId { get; set; }

        public MenuType MenuType { get; set; }
        public long Votes { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}