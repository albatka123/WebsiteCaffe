using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.DTO
{
    public class chitiet0
    {
        public string id { get; set; }
        public string soluong { get; set; }
        public string thanhtien { get; set; }

        public List<phu0> phus { get; set; }
    }
}