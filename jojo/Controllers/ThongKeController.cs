using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;
using Cafe.DTO;
using Cafe.Models;

namespace Cafe.Controllers
{
    public class ThongKeController : Controller
    {
        //
        // GET: /ThongKe/
        CafeContext db = new CafeContext();
        public ActionResult Index()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            if (a.loai == 1)
            {
                return Redirect("/KinhDoanh");
            }
            ViewBag.nhanvien = a;
            return View();
        }

        public ActionResult DoanhThu()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            if (a.loai == 1)
            {
                return Redirect("/KinhDoanh");
            }
            ViewBag.nhanvien = a;
            if (TempData["tungay"] == null)
            {
                return View();
            }
            else
            {
                ViewBag.tungay = TempData["tungay"].ToString();
                ViewBag.toingay = TempData["toingay"].ToString();
                ViewBag.tongtien = TempData["tongtien"].ToString();
                ViewBag.doanhthu = TempData["DoanhThu"] as List<TKDoanhThu>;
                return View("KetQuaDoanhThu");
            }
        }

        [HttpPost]
        public ActionResult DoanhThu2()
        {
            DateTime tungay = DateTime.Parse(Request.Form["tungay"]);
            DateTime toingay = DateTime.Parse(Request.Form["toingay"]);
            var query = from b in db.hoadons
                        where DbFunctions.TruncateTime(b.ngayhoadon) >= DbFunctions.TruncateTime(tungay)
                        && DbFunctions.TruncateTime(b.ngayhoadon) <= DbFunctions.TruncateTime(toingay)
                        && b.active == "true"
                        group b by DbFunctions.TruncateTime(b.ngayhoadon) into g

                        select new
                        {
                            ngayhoadon = g.Key.Value,
                            tongtien = g.Sum(p => p.tongtien),
                            giamgia = g.Sum(p => p.giamgia),
                            khachphaitra = g.Sum(p => p.khachphaitra)
                        };
            List<TKDoanhThu> list = new List<TKDoanhThu>();
            var tongtien = 0;
            if (query.Count() != 0)
            {
                foreach (var item in query)
                {
                    TKDoanhThu tk = new TKDoanhThu
                    {
                        ngay = item.ngayhoadon,
                        tongtien = item.tongtien,
                        giamgia = item.giamgia,
                        khachphaitra = item.khachphaitra
                    };
                    tongtien = tongtien + item.khachphaitra;
                    list.Add(tk);
                }
            }
            TempData["Doanhthu"] = list;
            TempData["tungay"] = tungay.ToString("dd/MM/yyyy");
            TempData["toingay"] = toingay.ToString("dd/MM/yyyy");
            TempData["TongTien"] = tongtien;
            return Redirect("/ThongKe/DoanhThu");

        }

        public ActionResult KhachHang()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            if (a.loai == 1)
            {
                return Redirect("/KinhDoanh");
            }
            ViewBag.nhanvien = a;
            if (TempData["tungay"] == null)
            {
                return View();
            }
            else
            {
                ViewBag.tungay = TempData["tungay"].ToString();
                ViewBag.toingay = TempData["toingay"].ToString();
                ViewBag.khachhang = TempData["KhachHang"] as List<TKKhachHang>;
                return View("KetQuaKhachHang");
            }
        }

        [HttpPost]
        public ActionResult KhachHang2()
        {
            DateTime tungay = DateTime.Parse(Request.Form["tungay"]);
            DateTime toingay = DateTime.Parse(Request.Form["toingay"]);
            var tenkhachhang = Request.Form["tenkhachhang"];
            var sdtkhachhang = Request.Form["sdtkhachhang"];
            var diachi = Request.Form["diachi"];
            var email = Request.Form["email"];
            var sapxep = Request.Form["sapxep"];

            var query = from b in db.hoadons
                        where DbFunctions.TruncateTime(b.ngayhoadon) >= DbFunctions.TruncateTime(tungay)
                        && DbFunctions.TruncateTime(b.ngayhoadon) <= DbFunctions.TruncateTime(toingay)
                        && b.active == "true"
                        && b.khachhang.tenkhachhang.Contains(tenkhachhang)
                        && b.khachhang.sodienthoai.Contains(sdtkhachhang)
                        && b.khachhang.diachi.Contains(diachi)
                        && b.khachhang.email.Contains(email)
                        && b.khachhang.active == "true"
                        group b by new
                        {
                            b.khachhang.id,
                            b.khachhang.tenkhachhang,
                            b.khachhang.sodienthoai,
                            b.khachhang.diachi,
                            b.khachhang.email
                        } into g
                        select new
                        {
                            idkhachhang = g.Key.id,
                            tenkhachhang = g.Key.tenkhachhang,
                            sdtkhachhang = g.Key.sodienthoai,
                            diachi = g.Key.diachi,
                            email = g.Key.email,
                            tongtien = g.Sum(b => b.khachphaitra),
                            solan = g.Count()
                        };
            List<TKKhachHang> list = new List<TKKhachHang>();

            if (query.Count() != 0)
            {
                if (sapxep == "1")
                {
                    query = query.OrderByDescending(x => x.solan);
                }
                else
                {
                    query = query.OrderByDescending(x => x.tongtien);
                }
                foreach (var item in query)
                {
                    TKKhachHang tk = new TKKhachHang
                    {
                        idkhachhang = item.idkhachhang,
                        tenkhachhang = item.tenkhachhang,
                        sdtkhachhang = item.sdtkhachhang,
                        diachi = item.diachi,
                        email = item.email,
                        solan = item.solan,
                        tongtien = item.tongtien,
                    };

                    list.Add(tk);
                }
            }



            TempData["KhachHang"] = list;
            TempData["tungay"] = tungay.ToString("dd/MM/yyyy");
            TempData["toingay"] = toingay.ToString("dd/MM/yyyy");
            return Redirect("/ThongKe/KhachHang");
        }

        public ActionResult SanPham()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            if (a.loai == 1)
            {
                return Redirect("/KinhDoanh");
            }
            ViewBag.nhanvien = a;
            if (TempData["tungay"] == null)
            {
                return View();
            }
            else
            {
                ViewBag.tungay = TempData["tungay"].ToString();
                ViewBag.toingay = TempData["toingay"].ToString();
                ViewBag.sanpham = TempData["SanPham"] as List<TKSanPham>;
                return View("KetQuaSanPham");
            }

        }

        [HttpPost]
        public ActionResult SanPham2()
        {
            DateTime tungay = DateTime.Parse(Request.Form["tungay"]);
            DateTime toingay = DateTime.Parse(Request.Form["toingay"]);
            //

            var query = from b in db.hoadons
                        from c in db.chitiethoadons
                        where DbFunctions.TruncateTime(b.ngayhoadon) >= DbFunctions.TruncateTime(tungay)// bo thong tin thoi gian va chi du thong tin ve ngay
                           && DbFunctions.TruncateTime(b.ngayhoadon) <= DbFunctions.TruncateTime(toingay)
                           && b.active == "true"
                           && b.id == c.idhoadon
                        select c;

            var query2 = from b in db.sanphams
                         from c in query
                         where b.id == c.idspchinh
                         group c by new
                         {
                             c.idspchinh,

                         } into g
                         select new
                         {
                             idsanpham = g.Key.idspchinh,
                             soluong = g.Sum(c => c.soluong)
                         };

            var query3 = from c in query
                         from b in db.bangphus
                         where c.id == b.idchitiethd
                         group b by new
                         {
                             b.idsanpham,

                         } into g
                         select new
                         {

                             idsanpham = g.Key.idsanpham,

                             soluong = g.Sum(b => b.soluong)
                         };

            List<sanpham> list1 = db.sanphams.Where(x => x.active == "true").ToList();
            List<sanphamphu> list2 = db.sanphamphus.Where(x => x.active == "true").ToList();
            List<TKSanPham> list = new List<TKSanPham>();

            for (int i = 0; i < list1.Count; i++)
            {
                TKSanPham tk = new TKSanPham
                {
                    idsanpham = list1[i].id,
                    tensanpham = list1[i].ten,
                    loai = Convert.ToInt32(list1[i].loai),
                    soluong = 0
                };

                if (query2.Count() != 0)
                {
                    foreach (var item in query2)
                    {
                        if (list1[i].id == item.idsanpham)
                        {
                            tk.soluong = item.soluong;
                            break;
                        }
                    }
                }
                list.Add(tk);
            }

            for (int i = 0; i < list2.Count; i++)
            {
                TKSanPham tk = new TKSanPham
                {
                    idsanpham = list2[i].id,
                    tensanpham = list2[i].ten,
                    loai = 4,
                    soluong = 0
                };
                if (query3.Count() != 0)
                {
                    foreach (var item in query3)
                    {
                        if (list2[i].id == item.idsanpham)
                        {
                            tk.soluong = item.soluong;
                            break;
                        }
                    }
                }
                list.Add(tk);
            }

            TempData["SanPham"] = list;
            TempData["tungay"] = tungay.ToString("dd/MM/yyyy");
            TempData["toingay"] = toingay.ToString("dd/MM/yyyy");

            return Redirect("/ThongKe/SanPham");

        }
        public ActionResult ChiPhi()
        {
            if (Request.Cookies["nhanvien"] == null)
            {
                return Redirect("/");
            }
            NhanVienDto a = JsonConvert.DeserializeObject<NhanVienDto>(HttpUtility.UrlDecode(Request.Cookies["nhanvien"].Value));
            if (a.loai == 1)
            {
                return Redirect("/KinhDoanh");
            }
            ViewBag.nhanvien = a;
            if (TempData["tungay"] == null)
            {
                return View();
            }
            else
            {
                ViewBag.tungay = TempData["tungay"].ToString();
                ViewBag.toingay = TempData["toingay"].ToString();
                int tongchiphi = Int32.Parse(TempData["tongchiphi"].ToString());
                int doanhthu = Int32.Parse(TempData["doanhthu"].ToString());
                ViewBag.tongchiphi = tongchiphi;
                ViewBag.chiphi = TempData["ChiPhi"] as List<TKChiPhi>;
                ViewBag.doanhthu = doanhthu;
                ViewBag.loinhuan = doanhthu - tongchiphi;
                return View("KetQuaChiPhi");
            }


        }

        [HttpPost]
        public ActionResult ChiPhi2()
        {
            DateTime tungay = DateTime.Parse(Request.Form["tungay"]);
            DateTime toingay = DateTime.Parse(Request.Form["toingay"]);
            var query = from b in db.chiphis
                        where b.active == "true"
                        && DbFunctions.TruncateTime(b.ngay) >= DbFunctions.TruncateTime(tungay)
                        && DbFunctions.TruncateTime(b.ngay) <= DbFunctions.TruncateTime(toingay)
                        group b by new
                        {
                            b.idloaichiphi
                        } into g
                        select new
                        {
                            idloaichiphi = g.Key.idloaichiphi,
                            tongchiphi = g.Sum(b => b.chiphi1)
                        };
            List<TKChiPhi> list = new List<TKChiPhi>();
            List<loaichiphi> list2 = db.loaichiphis.ToList();
            //
            var tongchiphi = 0;
            for (int i = 0; i < list2.Count; i++)
            {
                TKChiPhi tk = new TKChiPhi
                {
                    idloaichiphi = list2[i].id,
                    ten = list2[i].tenchiphi,
                    tongchiphi = 0
                };
                if (query.Count() != 0)
                {
                    foreach (var item in query)
                    {
                        if (list2[i].id == item.idloaichiphi)
                        {
                            tk.tongchiphi = item.tongchiphi;
                            tongchiphi = tongchiphi + item.tongchiphi;
                            break;
                        }
                    }
                }
                list.Add(tk);
            }

            var query2 = from b in db.hoadons
                         where DbFunctions.TruncateTime(b.ngayhoadon) >= DbFunctions.TruncateTime(tungay)
                         && DbFunctions.TruncateTime(b.ngayhoadon) <= DbFunctions.TruncateTime(toingay)
                         && b.active == "true"
                         group b by DbFunctions.TruncateTime(b.ngayhoadon) into g

                         select new
                         {
                             ngayhoadon = g.Key.Value,
                             tongtien = g.Sum(p => p.tongtien),
                             giamgia = g.Sum(p => p.giamgia),
                             khachphaitra = g.Sum(p => p.khachphaitra)
                         };

            var doanhthu = 0;
            if (query2.Count() != 0)
            {
                foreach (var item in query2)
                {
                    doanhthu = doanhthu + item.khachphaitra;
                }
            }

            TempData["ChiPhi"] = list;
            TempData["tungay"] = tungay.ToString("dd/MM/yyyy");
            TempData["toingay"] = toingay.ToString("dd/MM/yyyy");
            TempData["tongchiphi"] = tongchiphi;
            TempData["doanhthu"] = doanhthu;
            return Redirect("/ThongKe/ChiPhi");
        }


    }
}