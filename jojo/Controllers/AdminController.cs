using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Cafe.DTO;
using Cafe.Models;
using Newtonsoft.Json;
using PagedList;

namespace Cafe.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        CafeContext db = new CafeContext();

        public ActionResult Index()
        {
            if (Request.Cookies["admin"] != null)// kiem ra cookie
            {
                return Redirect("/Admin/NhanVien");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login()
        {
            try
            {
                var id = Convert.ToInt32(Request.Form["id"]);// lay du lieu string cua truong id chuyen ve int
                var matkhau = Request.Form["matkhau"]; 
                admin a = (from b in db.admins 
                           where b.id == id && b.matkhau == matkhau
                           select b).FirstOrDefault();
                AdminDto adminDto = Mapper.Map<admin, AdminDto>(a); //dung automap anh xa
                if (adminDto != null)
                {
                    Response.Cookies["admin"].Value = HttpUtility.UrlEncode(JsonConvert.SerializeObject(adminDto));//ma hoa thanh url va luu duoi cookia Admin
                    Response.Cookies["admin"].Expires = DateTime.Now.AddDays(30);//Dat thoi gian cookia song 30 ngay
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin");
        }

        public ActionResult NhanVien()
        {
            if (Request.Cookies["admin"] != null)
            {
                var query = from b in db.nhanviens
                            where b.active == "true"
                            select b;
                ViewBag.nhanvien = query.ToList();
                return View();
            }
            return Redirect("/Admin");
        }

        public ActionResult ThemNhanVien()
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ThemNhanVien2()
        {
            try
            {
                var hovaten = Request.Form["hovaten"];
                var gioitinh = Request.Form["gioitinh"];
                var ngaysinh = Request.Form["ngaysinh"];
                var sdt = Request.Form["sdt"];
                var diachi = Request.Form["diachi"];
                var cmnd = Request.Form["cmnd"];
                var ngayvaolam = Request.Form["ngayvaolam"];
                var congviec = Request.Form["congviec"];
                var matkhau = Request.Form["matkhau"];
                var loai = Convert.ToInt16(Request.Form["loai"]);
                var active = "true";
                var taikhoan = Request.Form["taikhoan"];
                //
                nhanvien a = new nhanvien
                {
                    hovaten = hovaten,
                    gioitinh = gioitinh,
                    ngaysinh = ngaysinh,
                    sodienthoai = sdt,
                    diachi = diachi,
                    cmnd = cmnd,
                    ngayvaolam = ngayvaolam,
                    congviec = congviec,
                    matkhau = matkhau,
                    loai = loai,
                    taikhoan = taikhoan,
                    active = active
                };
                db.nhanviens.Add(a);
                db.SaveChanges();
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/NhanVien");
        }

        public ActionResult ChiTietNhanVien(string id)
        {

            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.nhanviens
                            where b.id == id2 && b.active == "true"
                            select b;
                nhanvien a = query.FirstOrDefault<nhanvien>();
                if (a == null)
                {
                    return Redirect("/Admin/NhanVien");
                }
                else
                {
                    ViewBag.nhanvien = a;
                    return View();
                }
            }
            catch
            {
                return Redirect("/Admin/NhanVien");
            }
        }

        public ActionResult CapNhatNhanVien(string id)
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.nhanviens
                            where b.id == id2 && b.active == "true"
                            select b;
                nhanvien a = query.FirstOrDefault<nhanvien>();
                if (a == null)
                {
                    return Redirect("/Admin/NhanVien");
                }
                else
                {
                    ViewBag.nhanvien = a;
                    return View();
                }
            }
            catch
            {
                return Redirect("/Admin/NhanVien");
            }
        }

        [HttpPost]
        public ActionResult CapNhatNhanVien2()
        {
            try
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var hovaten = Request.Form["hovaten"];
                var gioitinh = Request.Form["gioitinh"];
                var ngaysinh = Request.Form["ngaysinh"];
                var sdt = Request.Form["sdt"];
                var diachi = Request.Form["diachi"];
                var cmnd = Request.Form["cmnd"];
                var ngayvaolam = Request.Form["ngayvaolam"];
                var congviec = Request.Form["congviec"];
                var matkhau = Request.Form["matkhau"];
                var taikhoan = Request.Form["taikhoan"];
                var loai = Convert.ToInt16(Request.Form["loai"]);
                var active = "true";
                //
                var query = from b in db.nhanviens
                            where b.id == id && b.active == "true"
                            select b;
                nhanvien a = query.FirstOrDefault<nhanvien>();
                if (a != null)
                {
                    a.hovaten = hovaten;
                    a.gioitinh = gioitinh;
                    a.ngaysinh = ngaysinh;
                    a.sodienthoai = sdt;
                    a.diachi = diachi;
                    a.cmnd = cmnd;
                    a.ngayvaolam = ngayvaolam;
                    a.congviec = congviec;
                    a.matkhau = matkhau;
                    a.loai = loai;
                    a.active = active;
                    a.taikhoan = taikhoan;
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/NhanVien");
        }

        public ActionResult XoaNhanVien(string id)
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.nhanviens
                            where b.id == id2 && b.active == "true"
                            select b;
                nhanvien a = query.FirstOrDefault<nhanvien>();
                if (a != null)
                {
                    a.active = "false";
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/NhanVien");
        }




        public ActionResult SanPham()
        {
            if (Request.Cookies["admin"] != null)
            {
                var query = from b in db.sanphams
                            where b.active == "true"
                            select b;
                ViewBag.sanpham = query.ToList();

                var query2 = from b in db.sanphamphus
                             where b.active == "true"
                             select b;
                ViewBag.sanphamphu = query2.ToList();

                return View();
            }
            return Redirect("/Admin");
        }

        public ActionResult ThemSanPham()
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ThemSanPham2()
        {
            try
            {
                var ten = Request.Form["ten"];
                var gia = Convert.ToInt32(Request.Form["gia"]);
                var loai = Request.Form["loai"];
                string active = "true";
                if (loai == "4")
                {
                    sanphamphu sp = new sanphamphu
                    {
                        ten = ten,
                        gia = gia,
                        active = active
                    };
                    db.sanphamphus.Add(sp);
                    db.SaveChanges();
                }
                else
                {
                    sanpham sp = new sanpham
                    {
                        ten = ten,
                        gia = gia,
                        loai = loai,
                        active = active
                    };
                    db.sanphams.Add(sp);
                    db.SaveChanges();
                }
                
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/SanPham");
        }



        public ActionResult CapNhatSanPhamChinh(string id)
        {
          
            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.sanphams
                            where b.id == id2 && b.active == "true"
                            select b;
                sanpham a = query.FirstOrDefault<sanpham>();
                if (a == null)
                {
                    return Redirect("/Admin/SanPham");
                }
                else
                {
                    ViewBag.sanpham = a;
                    return View();
                }  
            }
            catch
            {
                return Redirect("/Admin/SanPham");
            }
        }

        public ActionResult CapNhatSanPhamPhu(string id)
        {
           
            try
            {
                int id2 = Convert.ToInt32(id);
                var query = from b in db.sanphamphus
                            where b.id == id2 && b.active == "true"
                            select b;
                sanphamphu a = query.FirstOrDefault<sanphamphu>();
                if (a == null)
                {
                    return Redirect("/Admin/SanPham");
                }
                else
                {
                    ViewBag.sanphamphu = a;
                    return View();
                }
            }
            catch
            {
                return Redirect("/Admin/SanPham");
            }
        }

        [HttpPost]
        public ActionResult CapNhatSanPham2()
        {
            try
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var ten = Request.Form["ten"];
                var gia = Convert.ToInt32(Request.Form["gia"]);
                var loai = Request.Form["loai"];
                string active = "true";

                var query = from b in db.sanphams
                            where b.id == id && b.active == "true"
                            select b;
                sanpham a = query.FirstOrDefault<sanpham>();
                if (a != null)
                {
                    a.ten = ten;
                    a.gia = gia;
                    a.loai = loai;
                    a.active = active;
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }


            return Redirect("/Admin/SanPham");
        }

        [HttpPost]
        public ActionResult CapNhatSanPham3()
        {
            try
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var ten = Request.Form["ten"];
                var gia = Convert.ToInt32(Request.Form["gia"]);
                string active = "true";

                var query = from b in db.sanphamphus
                            where b.id == id && b.active == "true"
                            select b;
                sanphamphu a = query.FirstOrDefault<sanphamphu>();
                if (a != null)
                {
                    a.ten = ten;
                    a.gia = gia;
                    a.active = active;
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }
            return Redirect("/Admin/SanPham");
        }

        public ActionResult XoaSanPham2(string id)
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                var id2 = Convert.ToInt32(id);
                var query = from b in db.sanphams
                            where b.id == id2 && b.active == "true"
                            select b;
                sanpham a = query.FirstOrDefault<sanpham>();
                if (a != null)
                {                   
                    a.active = "false";
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/SanPham");
        }

        public ActionResult XoaSanPham3(string id)
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                var id2 = Convert.ToInt32(id);
                var query = from b in db.sanphamphus
                            where b.id == id2 && b.active == "true"
                            select b;
                sanphamphu a = query.FirstOrDefault<sanphamphu>();
                if (a != null)
                {
                    a.active = "false";
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/SanPham");
        }


        public ActionResult HoaDon(int? page)
        {

            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            var query = from b in db.bangxoahoadons
                        where b.active == "true"
                        select b;

            List<bangxoahoadon> list = query.OrderByDescending(x => x.ngay).ToList();
            //
            int pageSize = 5;
            int pageNumber = (page ?? 1);



            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Xoahoadon(string id)
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                var id2 = Convert.ToInt32(id);
                var query = from b in db.bangxoahoadons
                            where b.id == id2 && b.active == "true"
                            select b;
                bangxoahoadon a = query.FirstOrDefault<bangxoahoadon>();
                if (a != null)
                {
                    a.active = "false";
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/HoaDon");
        }

        public ActionResult Phuchoihoadon(string id)
        {
            if (Request.Cookies["admin"] == null)
            {
                return Redirect("/Admin");
            }
            try
            {
                var id2 = Convert.ToInt32(id);
                var query = from b in db.bangxoahoadons
                            where b.id == id2 && b.active == "true"
                            select b;
                bangxoahoadon a = query.FirstOrDefault<bangxoahoadon>();

                var query2 = from b in db.hoadons
                             where b.id == id2 && b.active == "false"
                             select b;
                hoadon c = query2.FirstOrDefault<hoadon>();

                if (a != null && c != null)
                {
                    a.active = "false";
                    c.active = "true";
                    db.SaveChanges();
                }
            }
            catch
            {
                return Redirect("/Error");
            }

            return Redirect("/Admin/HoaDon");
        }


        public ActionResult Logout()
        {
            if (Request.Cookies["admin"] != null)
            {
                var c = new HttpCookie("admin");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return Redirect("/Admin");
        }

	}
}