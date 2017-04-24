using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;
using SCGrillConfig.Models;

namespace SCGrillConfig.Controllers
{
    public class MaterialsController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToMaterial
        public ActionResult AddGrillConfgurationToMaterial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(material.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (material == null)
            {
                return HttpNotFound();
            }
            MaterialGrillConfgurationViewModel viewModel = new MaterialGrillConfgurationViewModel();
            viewModel.MaterialId = material.Id;
            viewModel.Material_Name = material.Name;
            return View(viewModel);
        }

        // POST: Materials/AddGrillConfgurationToMaterial/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToMaterial([Bind(Include = "MaterialId,Material_Name,GrillConfgurationId,GrillConfguration_Name")] MaterialGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Material material = db.Materials.Find(viewModel.MaterialId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                material.GrillConfgurations.Add(grillConfguration);
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Materials/AddGrillConfgurationToMaterial/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to MaterialId: " + material.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.MaterialId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromMaterial
        // NOTE: the GrillConfguration.MaterialId property is not nullable so this routine must be omitted

        // GET: Materials
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/MaterialsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Materials.ToList());
        }

        // GET: Materials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // GET: Materials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Material material, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Materials.Add(material);
                db.SaveChanges();
                LogManager.Log("Materials/Create - MaterialId:" + material.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(material);
        }

        // GET: Materials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("Materials/Edit/ - MaterialId:" + material.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // GET: Materials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Material material = db.Materials.Find(id);
            db.Materials.Remove(material);
            db.SaveChanges();
            LogManager.Log("Materials/Delete/ - MaterialId:" + material.Id.ToString());
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
