using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStoreFall2018.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int AlbumId { get; set; }

        public int Count { get; set; }

        public decimal UnitPrice { get; set; }



    }
}