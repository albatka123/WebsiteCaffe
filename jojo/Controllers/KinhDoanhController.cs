using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.Models;
using Cafe.DTO;
using System.Globalization;
using Newtonsoft.Json;

namespace Cafe.Controllers
{
    public class KinhDoanhController : Controller
    {
        //
        // GET: /KinhDoanh/
        CafeContext db = new CafeContext();
        public ActionResult Index()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            ViewBag.nhanvien = a;
            //
           
            var query = from b in db.sanphams
                        where b.active == "true"
                        select b;


            ViewBag.sanphams = query.ToList();

            
            var query2 = from b in db.sanphamphus
                         where b.active == "true"
                         select b;
            ViewBag.sanphamphus = query2.ToList();
            //
            List<khachhang0> khachhangs = new List<khachhang0>();

            var query3 = db.khachhangs.Where(x => x.active == "true");

            if (query3.Count() != 0)
            {
                foreach (khachhang kh in query3)
                {
                    khachhang0 k = new khachhang0
                    {
                        id = kh.id,
                        tenkhachhang = kh.tenkhachhang,
                        sodienthoai = kh.sodienthoai,
                        diachi = kh.diachi,
                        email = kh.email,
                        ghichu = kh.ghichu,
                        trangthai = kh.trangthai,
                        active = kh.active
                    };
                    khachhangs.Add(k);
                }
            }

            ViewBag.khachhang = khachhangs;

            return View();
        }
        [HttpPost]
        public ActionResult ThanhToan(hoadon0 h)
        {
            string sdtkhachhang = h.sdtkhachhang + "";
            string tenkhachhang = h.tenkhachhang + "";
            string diachikhachhang = h.diachikhachhang + "";
            int idkh = 0;

            khachhang kh = db.khachhangs.Where(x => x.sodienthoai == sdtkhachhang && x.active == "true").FirstOrDefault<khachhang>();
            if (kh == null)
            {
                khachhang kh_new = new khachhang
                {
                    sodienthoai = sdtkhachhang,
                    tenkhachhang = tenkhachhang,
                    diachi = diachikhachhang,
                    email = "",
                    ghichu = "",
                    active = "true",
                    trangthai = "Chưa cập nhật"
                };
                db.khachhangs.Add(kh_new);
                idkh = kh_new.id;
            }
            else
            {
                idkh = kh.id;
            }

            hoadon hoadon = new hoadon
            {
                id = -1,
                idnhanvien = Convert.ToInt32(h.idnhanvien),
                idkhachhang = idkh,
                tongtien = Convert.ToInt32(h.tongtien),
                giamgia = Convert.ToInt32(h.giamgia),
                khachphaitra = Convert.ToInt32(h.khachphaitra),
                ngayhoadon = DateTime.ParseExact(h.ngay, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                active = "true"
            };
            db.hoadons.Add(hoadon);
            var tmp = -10000;
            for (int i = 0; i < h.chitiets.Count; i++)
            {
                chitiethoadon chitiet = new chitiethoadon
                {
                    id = tmp,
                    idhoadon = -1,
                    idspchinh = Convert.ToInt32(h.chitiets[i].id),
                    soluong = Convert.ToInt32(h.chitiets[i].soluong),
                    thanhtien = Convert.ToInt32(h.chitiets[i].thanhtien),
                };
                db.chitiethoadons.Add(chitiet);

                for (int j = 0; j < h.chitiets[i].phus.Count; j++)
                {
                    bangphu bang = new bangphu
                    {
                        idchitiethd = tmp,
                        idsanpham = Convert.ToInt32(h.chitiets[i].phus[j].id),
                        soluong = Convert.ToInt32(h.chitiets[i].phus[j].soluong) * Convert.ToInt32(h.chitiets[i].soluong)
                    };
                    db.bangphus.Add(bang);
                }
                tmp++;
            }
            db.SaveChanges();

            return Json(new { msg = "OK" });


        }
    }
}