using System.Linq;
using PapajVZ.Helpers;
using PapajVZ.Models;
using Xamarin.Forms;

namespace PapajVZ.Model
{
    public class Vote
    {
        public bool Unvoting { get; set; }
        public string DeviceId { get; set; }
        public int MenuId { get; set; }
    }
}