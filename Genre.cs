using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStoreFall2018.Models
{
    public class Genre
    {
        public virtual int GenreId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public List<Album> Albums { get; set; }
    }
}