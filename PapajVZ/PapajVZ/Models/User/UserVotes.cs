using System.Collections.Generic;

namespace PapajVZ.Model
{
    public class UserVotes 
    {
        public UserVotes()
        {
            Menus = new List<int>();
        }
        public string DeviceId { get; set; }
        public List<int> Menus { get; set; }

    }
}