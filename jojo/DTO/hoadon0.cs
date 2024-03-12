using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.DTO
{
    public class hoadon0
    {
        public string idnhanvien { get; set; }
        public string tenkhachhang { get; set; }
        public string sdtkhachhang { get; set; }
        public string diachikhachhang { get; set; }
        public string ngay { get; set; }
        public string giamgia { get; set; }
        public string tongtien { get; set; }
        public string khachphaitra { get; set; }



        public List<chitiet0> chitiets { get; set; }
    }
}