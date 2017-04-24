using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SCGrillConfig.Models;

namespace SCGrillConfig.Controllers
{
    public class SideBurnerTypesDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetSideBurnerTypeGrillConfgurations/?SideBurnerTypeId=1
        [Route("api/GetSideBurnerTypeGrillConfgurations/")]
        public List<GrillConfguration> GetSideBurnerTypeGrillConfgurations(int SideBurnerTypeId)
        {
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(SideBurnerTypeId);
            if (sideBurnerType == null)
            {
                return null;
            }
            return sideBurnerType.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToSideBurnerType/?SideBurnerTypeId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToSideBurnerType/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToSideBurnerType(int SideBurnerTypeId, int GrillConfgurationId)
        {
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(SideBurnerTypeId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (sideBurnerType != null && grillConfguration != null)
            {
                try
                {
                    sideBurnerType.GrillConfgurations.Add(grillConfguration);
                    db.Entry(sideBurnerType).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToSideBurnerType - SideBurnerTypeId:" + sideBurnerType.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // AddGrillConfgurationToSideBurnerType

        // PUT: api/RemoveGrillConfgurationFromSideBurnerType/?SideBurnerTypeId=1&GrillConfgurationId=1
        // NOTE: the GrillConfguration.SideBurnerTypeId property is not nullable so this routine must be omitted

        // GET: api/SideBurnerTypesData
        public IQueryable<SideBurnerType> GetSideBurnerTypes()
        {
            return db.SideBurnerTypes;
        }

        // GET: api/SideBurnerTypesData/5
        [ResponseType(typeof(SideBurnerType))]
        public IHttpActionResult GetSideBurnerType(int id)
        {
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            if (sideBurnerType == null)
            {
                return NotFound();
            }

            return Ok(sideBurnerType);
        }

        // PUT: api/SideBurnerTypesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSideBurnerType(int id, SideBurnerType sideBurnerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sideBurnerType.Id)
            {
                return BadRequest();
            }

            db.Entry(sideBurnerType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/SideBurnerTypesData/ - SideBurnerTypeId:" + sideBurnerType.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SideBurnerTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SideBurnerTypesData
        [ResponseType(typeof(SideBurnerType))]
        public IHttpActionResult PostSideBurnerType(SideBurnerType sideBurnerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SideBurnerTypes.Add(sideBurnerType);
            db.SaveChanges();
            LogManager.Log("POST: api/SideBurnerTypesData/ - SideBurnerTypeId:" + sideBurnerType.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = sideBurnerType.Id }, sideBurnerType);
        }

        // DELETE: api/SideBurnerTypesData/5
        [ResponseType(typeof(SideBurnerType))]
        public IHttpActionResult DeleteSideBurnerType(int id)
        {
            SideBurnerType sideBurnerType = db.SideBurnerTypes.Find(id);
            if (sideBurnerType == null)
            {
                return NotFound();
            }

            db.SideBurnerTypes.Remove(sideBurnerType);
            db.SaveChanges();
            LogManager.Log("DELETE: api/SideBurnerTypesData/ - SideBurnerTypeId:" + sideBurnerType.Id.ToString());

            return Ok(sideBurnerType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SideBurnerTypeExists(int id)
        {
            return db.SideBurnerTypes.Count(e => e.Id == id) > 0;
        }
    }
}
