using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicWebShopProject.Models
{
    public class MusicTrack
    {
        public int ID { get; set; }

        public string AlbumID { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public decimal Price { get; set; }
        public string TrackImgUrl { get; set; }
        public byte[] TrackImg { get; set; }
        public string TrackFileUrl { get; set; }
        public byte[] TrackFile { get; set; }


        public virtual Album Album { get; set; }
    }
}