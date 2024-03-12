using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.DTO
{
    
    
        public class TradingModel
        {
            public string YearTitle { get; set; }
            public string DoanhThuTitle { get; set; }
            public string ChiPhiTitle { get; set; }
            public Product ProductData { get; set; }
        }
        public class Product
        {
            public string Year { get; set; }
            public string DoanhThus { get; set; }
            public string ChiPhis { get; set; }
        }
    
}