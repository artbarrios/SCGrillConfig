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
    public class SideBurnerTypesController : Controller
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: AddGrillConfgurationToSideBurnerType
        public ActionResult AddGrillConfgurationToSideBurnerType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            var grillConfgurationsAvailable = db.GrillConfgurations.ToList().Except(sideBurnerType.GrillConfgurations.ToList()).ToList();
            ViewBag.GrillConfgurationId = new SelectList(grillConfgurationsAvailable, "Id", "Name");
            if (sideBurnerType == null)
            {
                return HttpNotFound();
            }
            SideBurnerTypeGrillConfgurationViewModel viewModel = new SideBurnerTypeGrillConfgurationViewModel();
            viewModel.SideBurnerTypeId = sideBurnerType.Id;
            viewModel.SideBurnerType_Name = sideBurnerType.Name;
            return View(viewModel);
        }

        // POST: SideBurnerTypes/AddGrillConfgurationToSideBurnerType/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrillConfgurationToSideBurnerType([Bind(Include = "SideBurnerTypeId,SideBurnerType_Name,GrillConfgurationId,GrillConfguration_Name")] SideBurnerTypeGrillConfgurationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(viewModel.SideBurnerTypeId);
                GrillConfguration grillConfguration = db.GrillConfgurations.Find(viewModel.GrillConfgurationId);
                sideBurnerType.GrillConfgurations.Add(grillConfguration);
                db.Entry(sideBurnerType).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("SideBurnerTypes/AddGrillConfgurationToSideBurnerType/ - GrillConfgurationId:" + grillConfguration.Id.ToString() + " to SideBurnerTypeId: " + sideBurnerType.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.SideBurnerTypeId });
            }
            return View(viewModel);
        }

        // GET: RemoveGrillConfgurationFromSideBurnerType
        // NOTE: the GrillConfguration.SideBurnerTypeId property is not nullable so this routine must be omitted

        // GET: SideBurnerTypes
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/SideBurnerTypesIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.SideBurnerTypes.ToList());
        }

        // GET: SideBurnerTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            if (sideBurnerType == null)
            {
                return HttpNotFound();
            }
            return View(sideBurnerType);
        }

        // GET: SideBurnerTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SideBurnerTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] SideBurnerType sideBurnerType, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.SideBurnerTypes.Add(sideBurnerType);
                db.SaveChanges();
                LogManager.Log("SideBurnerTypes/Create - SideBurnerTypeId:" + sideBurnerType.Id.ToString());
                return RedirectToAction("Index");
            }

            return View(sideBurnerType);
        }

        // GET: SideBurnerTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            if (sideBurnerType == null)
            {
                return HttpNotFound();
            }
            return View(sideBurnerType);
        }

        // POST: SideBurnerTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] SideBurnerType sideBurnerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sideBurnerType).State = EntityState.Modified;
                db.SaveChanges();
                LogManager.Log("SideBurnerTypes/Edit/ - SideBurnerTypeId:" + sideBurnerType.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(sideBurnerType);
        }

        // GET: SideBurnerTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            if (sideBurnerType == null)
            {
                return HttpNotFound();
            }
            return View(sideBurnerType);
        }

        // POST: SideBurnerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            db.SideBurnerTypes.Remove(sideBurnerType);
            db.SaveChanges();
            LogManager.Log("SideBurnerTypes/Delete/ - SideBurnerTypeId:" + sideBurnerType.Id.ToString());
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
