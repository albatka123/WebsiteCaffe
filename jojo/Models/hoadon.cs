//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cafe.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class hoadon
    {
        public hoadon()
        {
            this.bangxoahoadons = new HashSet<bangxoahoadon>();
            this.chitiethoadons = new HashSet<chitiethoadon>();
        }
    
        public int id { get; set; }
        public int tongtien { get; set; }
        public int idkhachhang { get; set; }
        public int idnhanvien { get; set; }
        public System.DateTime ngayhoadon { get; set; }
        public int giamgia { get; set; }
        public int khachphaitra { get; set; }
        public string active { get; set; }
    
        public virtual ICollection<bangxoahoadon> bangxoahoadons { get; set; }
        public virtual ICollection<chitiethoadon> chitiethoadons { get; set; }
        public virtual khachhang khachhang { get; set; }
        public virtual nhanvien nhanvien { get; set; }
    }
}
