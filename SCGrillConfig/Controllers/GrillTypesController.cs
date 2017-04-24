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
    public class GrillTypesController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToGrillType
        public ActionResult AddGrillConfgurationToGrillType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillType grillType = db.GrillTypes.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(grillType.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (grillType == null)
            {
                return HttpNotFound();
            }
            GrillTypeGrillConfgurationViewModel viewModel = new GrillTypeGrillConfgurationViewModel();
            viewModel.GrillTypeId = grillType.Id;
            viewModel.GrillType_Name = grillType.Name;
            return View(viewModel);
        }

        // POST: GrillTypes/AddGrillConfgurationToGrillType/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToGrillType([Bind(Include = "GrillTypeId,GrillType_Name,GrillConfgurationId,GrillConfguration_Name")] GrillTypeGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                GrillType grillType = db.GrillTypes.Find(viewModel.GrillTypeId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                grillType.GrillConfgurations.Add(grillConfguration);
                db.Entry(grillType).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("GrillTypes/AddGrillConfgurationToGrillType/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to GrillTypeId: " + grillType.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.GrillTypeId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromGrillType
        // NOTE: the GrillConfguration.GrillTypeId property is not nullable so this routine must be omitted

        // GET: GrillTypes
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/GrillTypesIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.GrillTypes.ToList());
        }

        // GET: GrillTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillType grillType = db.GrillTypes.Find(id);
            if (grillType == null)
            {
                return HttpNotFound();
            }
            return View(grillType);
        }

        // GET: GrillTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrillTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] GrillType grillType, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.GrillTypes.Add(grillType);
                db.SaveChanges();
                LogManager.Log("GrillTypes/Create - GrillTypeId:" + grillType.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(grillType);
        }

        // GET: GrillTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillType grillType = db.GrillTypes.Find(id);
            if (grillType == null)
            {
                return HttpNotFound();
            }
            return View(grillType);
        }

        // POST: GrillTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] GrillType grillType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grillType).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("GrillTypes/Edit/ - GrillTypeId:" + grillType.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(grillType);
        }

        // GET: GrillTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrillType grillType = db.GrillTypes.Find(id);
            if (grillType == null)
            {
                return HttpNotFound();
            }
            return View(grillType);
        }

        // POST: GrillTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrillType grillType = db.GrillTypes.Find(id);
            db.GrillTypes.Remove(grillType);
            db.SaveChanges();
            LogManager.Log("GrillTypes/Delete/ - GrillTypeId:" + grillType.Id.ToString());
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
