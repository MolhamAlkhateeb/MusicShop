﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicWebShopProject.Models
{
    public class Artist
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}