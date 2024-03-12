using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.DTO
{
    public class chiphi0
    {
        public int idloaichiphi { get; set; }
        public string ten { get; set; }
        public int idchiphi { get; set; }
        public DateTime ngay { get; set; }
        public int tongchiphi { get; set; }

        public chiphi0(int idloaichiphi, string ten, int idchiphi, DateTime ngay, int tongchiphi)
        {
            this.idloaichiphi = idloaichiphi;
            this.ten = ten;
            this.idchiphi = idchiphi;
            this.ngay = ngay;
            this.tongchiphi = tongchiphi;
        }
    }
}