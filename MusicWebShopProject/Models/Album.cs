using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicWebShopProject.Models
{
    public class Album
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ArtistID { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual ICollection<MusicTrack> Tracks { get; set; }
    }
}