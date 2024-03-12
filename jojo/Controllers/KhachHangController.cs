using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cafe.Models;
using Cafe.DTO;
using Newtonsoft.Json;

namespace Cafe.Controllers
{
    public class KhachHangController : Controller
    {
        //
        // GET: /KhachHang/

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

        public JsonResult TakeData(string tenkhachhang, string sodienthoai, string diachi, string email, string trangthai, string page, string rowperpage)
        {
            var query = from b in db.khachhangs
                        where b.tenkhachhang.Contains(tenkhachhang)
                        && b.sodienthoai.Contains(sodienthoai)
                        && b.diachi.Contains(diachi)
                        && b.email.Contains(email)
                        && b.trangthai.Contains(trangthai)
                        && b.active == "true"
                        select b;
            List<khachhang> khachhang = query.ToList();
            int page2 = Convert.ToInt32(page);
            int rowperpage2 = Convert.ToInt32(rowperpage);
            //
            int offset = (page2 - 1) * rowperpage2;

            var data2 = query.OrderByDescending(x => x.id).Skip(offset).Take(rowperpage2).ToList();

            List<khachhang0> data = new List<khachhang0>();
            for (int i = 0; i < data2.Count; i++)
            {
                khachhang0 new_kh = new khachhang0
                {
                    id = data2[i].id,
                    tenkhachhang = data2[i].tenkhachhang,
                    sodienthoai = data2[i].sodienthoai,
                    diachi = data2[i].diachi,
                    email = data2[i].email,
                    active = data2[i].active,
                    ghichu = data2[i].ghichu,
                    trangthai = data2[i].trangthai
                };
                data.Add(new_kh);
            }
            var count = query.Count();
            var result = new
            {
                data = data,
                count = count
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThemMoi()
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

        [HttpPost]
        public ActionResult ThemMoi2()
        {
            try
            {
                var tenkhachhang = Request.Form["tenkhachhang"];
                var sodienthoai = Request.Form["sodienthoai"];
                var diachi = Request.Form["diachi"];
                var email = Request.Form["email"];
                var ghichu = Request.Form["ghichu"];
                var trangthai = Request.Form["trangthai"];
                var active = "true";
                //
                khachhang c = new khachhang
                {
                    tenkhachhang = tenkhachhang,
                    sodienthoai = sodienthoai,
                    diachi = diachi,
                    email = email,
                    ghichu = ghichu,
                    trangthai = trangthai,
                    active = active,
                };
                db.khachhangs.Add(c);
                db.SaveChanges();
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/KhachHang/ThemMoi");
        }


        public ActionResult CapNhat(string id)
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

            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.khachhangs
                            where b.id == id2 && b.active == "true"
                            select b;
                khachhang c = query.FirstOrDefault<khachhang>();
                if (c == null)
                {
                    return Redirect("/KhachHang");
                }
                else
                {
                    ViewBag.khachhang = c;
                    return View();
                }
            }
            catch
            {
                return Redirect("/KhachHang");
            }
        }


        [HttpPost]
        public ActionResult CapNhat2()
        {
            try
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var tenkhachhang = Request.Form["tenkhachhang"];
                var sodienthoai = Request.Form["sodienthoai"];
                var diachi = Request.Form["diachi"];
                var email = Request.Form["email"];
                var ghichu = Request.Form["ghichu"];
                var trangthai = Request.Form["trangthai"];

                //
                var query = from b in db.khachhangs
                            where b.id == id && b.active == "true"
                            select b;
                khachhang c = query.FirstOrDefault<khachhang>();
                if (c != null)
                {
                    c.tenkhachhang = tenkhachhang;
                    c.sodienthoai = sodienthoai;
                    c.diachi = diachi;
                    c.email = email;
                    c.ghichu = ghichu;
                    c.trangthai = trangthai;
                    db.SaveChanges();
                }


            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/KhachHang");
        }

        public ActionResult Xoa(string id)
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
            try
            {
                int id2 = Int32.Parse(id);
                var query = from b in db.khachhangs
                            where b.id == id2 && b.active == "true"
                            select b;
                khachhang c = query.FirstOrDefault<khachhang>();
                if (c != null)
                {
                    c.active = "false";
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/KhachHang");
        }
	}
}