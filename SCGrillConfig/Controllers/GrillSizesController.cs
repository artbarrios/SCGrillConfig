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
    public class GrillSizesController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToGrillSize
        public ActionResult AddGrillConfgurationToGrillSize(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillSize grillSize = db.GrillSizes.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(grillSize.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (grillSize == null)
            {
                return HttpNotFound();
            }
            GrillSizeGrillConfgurationViewModel viewModel = new GrillSizeGrillConfgurationViewModel();
            viewModel.GrillSizeId = grillSize.Id;
            viewModel.GrillSize_Name = grillSize.Name;
            return View(viewModel);
        }

        // POST: GrillSizes/AddGrillConfgurationToGrillSize/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToGrillSize([Bind(Include = "GrillSizeId,GrillSize_Name,GrillConfgurationId,GrillConfguration_Name")] GrillSizeGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                GrillSize grillSize = db.GrillSizes.Find(viewModel.GrillSizeId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                grillSize.GrillConfgurations.Add(grillConfguration);
                db.Entry(grillSize).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("GrillSizes/AddGrillConfgurationToGrillSize/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to GrillSizeId: " + grillSize.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.GrillSizeId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromGrillSize
        // NOTE: the GrillConfguration.GrillSizeId property is not nullable so this routine must be omitted

        // GET: GrillSizes
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/GrillSizesIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.GrillSizes.ToList());
        }

        // GET: GrillSizes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillSize grillSize = db.GrillSizes.Find(id);
            if (grillSize == null)
            {
                return HttpNotFound();
            }
            return View(grillSize);
        }

        // GET: GrillSizes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrillSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GrillSize grillSize, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.GrillSizes.Add(grillSize);
                db.SaveChanges();
                LogManager.Log("GrillSizes/Create - GrillSizeId:" + grillSize.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(grillSize);
        }

        // GET: GrillSizes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillSize grillSize = db.GrillSizes.Find(id);
            if (grillSize == null)
            {
                return HttpNotFound();
            }
            return View(grillSize);
        }

        // POST: GrillSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GrillSize grillSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grillSize).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("GrillSizes/Edit/ - GrillSizeId:" + grillSize.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(grillSize);
        }

        // GET: GrillSizes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillSize grillSize = db.GrillSizes.Find(id);
            if (grillSize == null)
            {
                return HttpNotFound();
            }
            return View(grillSize);
        }

        // POST: GrillSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrillSize grillSize = db.GrillSizes.Find(id);
            db.GrillSizes.Remove(grillSize);
            db.SaveChanges();
            LogManager.Log("GrillSizes/Delete/ - GrillSizeId:" + grillSize.Id.ToString());
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
