using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VLTApplication.Models;
using System.Transactions;
namespace VLTApplication.Controllers

{
    public class VLTeaController : Controller
    {
        CS4PEntities db = new CS4PEntities();
        public ActionResult Index()
        {
            var model = db.BubleTeas;
            return View(model.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BubleTea model)
        {
            ValidateBubleTea(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    // add model to database
                    db.BubleTeas.Add(model);
                    db.SaveChanges();
                    // save file to appdata
                    var path = Server.MapPath("~/App_Data");
                    path = System.IO.Path.Combine(path, model.id.ToString());
                    Request.Files["Image"].SaveAs(path);
                    //all done successfully
                    scope.Complete();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult Image(string id)
        {
            var path = Server.MapPath("~/App_Data");
            path = System.IO.Path.Combine(path, id);
            return File(path, "Image/*");
        }

        private void ValidateBubleTea(BubleTea model)
        {
            if (model.Price <= 0)
                ModelState.AddModelError("Price", VLTError.PRICE_LESS_0);
        }

        public ActionResult Edit(int id)
        {
            var model = db.BubleTeas.Find(id);
            if (model == null)
                return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BubleTea model)
        {
            ValidateBubleTea(model);
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = db.BubleTeas.Find(id);
            if (model == null)
                return HttpNotFound();
            db.BubleTeas.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}