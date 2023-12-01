using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using UDW.Library;


namespace _63CNTT4N2.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();

        /// //////////////////////////////////////////////////////////////////////////////////
        // INDEX
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        /// //////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xu ly mot so truong tu dong
                //CreateAt
                categories.CreateAt = DateTime.Now;
                //UpdateAt
                categories.UpdateAt = DateTime.Now;
                //CreateBy
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentId
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Them moi dong du lieu
                categoriesDAO.Insert(categories);
                //thông báo là thêm dữu liệu thành công
                TempData["message"] = new XMessage ("success","Thêm mới mẫu tin thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao Status that bai
                TempData["message"] = new XMessage("danger", "Thay đổi Status thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao Status that bai
                TempData["message"] = new XMessage("danger", "Thay đổi Status thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat
            //updateAt
            categories.UpdateAt = DateTime.Now;
            //updateBy
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //status
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //update DB
            categoriesDAO.Update(categories);
            //thông báo là thêm dữu liệu thành công
            TempData["message"] = new XMessage("success", "Thay đổi Status thành công");
            return RedirectToAction("Index");
        }

        ///// //////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        ///// //////////////////////////////////////////////////////////////////////////////////
        /// Update
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //thong bao Status that bai
                TempData["message"] = new XMessage("danger", "Cập nhật mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao Status that bai
                TempData["message"] = new XMessage("danger", "Cập nhật mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //cập nhật một số trường thông tin tự động
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentId
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //updateAt
                categories.UpdateAt = DateTime.Now;
                //updateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //update DB
                categoriesDAO.Update(categories);

                //thông báo là thêm dữu liệu thành công
                TempData["message"] = new XMessage("success", "Thay đổi Status thành công");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        ///// //////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thông báo là xóa dữ liệu thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thông báo là xóa dữ liệu thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            //Delete 1 dong
            categoriesDAO.Delete(categories);
            //thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            return RedirectToAction("Trash");
        }
        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        //Chuyển mẫu tin đang ở trạng thái Status la 1/2 thành 0: khong hien thi o trang INDEX
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //cap nhat thong tin cho DB (id==id)
            //Cap nhat Status
            categories.Status = 0;
            //cap nhạt Update At
            categories.UpdateAt = DateTime.Now;
            //cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash = luc thung rac
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));//Status= 0
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo/5
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai status = 2
            categories.Status = 2;
            //cap nhạt Update At
            categories.UpdateAt = DateTime.Now;
            //cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");//o lai thung rac de tiep tuc Undo
        }
    }
}
