using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.DTO
{
    public class NhanVienDto
    {
        public int id { get; set; }
        public string matkhau { get; set; }
        public string hovaten { get; set; }
        public string gioitinh { get; set; }
        public string ngaysinh { get; set; }
        public string sodienthoai { get; set; }
        public string diachi { get; set; }
        public string cmnd { get; set; }
        public string ngayvaolam { get; set; }
        public string congviec { get; set; }
        public string taikhoan { get; set; }
        public int loai { get; set; }
        public string active { get; set; }
    }
}