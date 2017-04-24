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
    public class GrillSizesDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetGrillSizeGrillConfgurations/?GrillSizeId=1
        [Route("api/GetGrillSizeGrillConfgurations/")]
        public List<GrillConfguration> GetGrillSizeGrillConfgurations(int GrillSizeId)
        {
            GrillSize grillSize = db.GrillSizes.Find(GrillSizeId);
            if (grillSize == null)
            {
                return null;
            }
            return grillSize.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToGrillSize/?GrillSizeId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToGrillSize/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToGrillSize(int GrillSizeId, int GrillConfgurationId)
        {
            GrillSize grillSize = db.GrillSizes.Find(GrillSizeId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (grillSize != null && grillConfguration != null)
            {
                try
                {
                    grillSize.GrillConfgurations.Add(grillConfguration);
                    db.Entry(grillSize).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToGrillSize - GrillSizeId:" + grillSize.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
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
        } // AddGrillConfgurationToGrillSize

        // PUT: api/RemoveGrillConfgurationFromGrillSize/?GrillSizeId=1&GrillConfgurationId=1
        // NOTE: the GrillConfguration.GrillSizeId property is not nullable so this routine must be omitted

        // GET: api/GrillSizesData
        public IQueryable<GrillSize> GetGrillSizes()
        {
            return db.GrillSizes;
        }

        // GET: api/GrillSizesData/5
        [ResponseType(typeof(GrillSize))]
        public IHttpActionResult GetGrillSize(int id)
        {
            GrillSize grillSize = db.GrillSizes.Find(id);
            if (grillSize == null)
            {
                return NotFound();
            }

            return Ok(grillSize);
        }

        // PUT: api/GrillSizesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrillSize(int id, GrillSize grillSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grillSize.Id)
            {
                return BadRequest();
            }

            db.Entry(grillSize).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/GrillSizesData/ - GrillSizeId:" + grillSize.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrillSizeExists(id))
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

        // POST: api/GrillSizesData
        [ResponseType(typeof(GrillSize))]
        public IHttpActionResult PostGrillSize(GrillSize grillSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GrillSizes.Add(grillSize);
            db.SaveChanges();
            LogManager.Log("POST: api/GrillSizesData/ - GrillSizeId:" + grillSize.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = grillSize.Id }, grillSize);
        }

        // DELETE: api/GrillSizesData/5
        [ResponseType(typeof(GrillSize))]
        public IHttpActionResult DeleteGrillSize(int id)
        {
            GrillSize grillSize = db.GrillSizes.Find(id);
            if (grillSize == null)
            {
                return NotFound();
            }

            db.GrillSizes.Remove(grillSize);
            db.SaveChanges();
            LogManager.Log("DELETE: api/GrillSizesData/ - GrillSizeId:" + grillSize.Id.ToString());

            return Ok(grillSize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrillSizeExists(int id)
        {
            return db.GrillSizes.Count(e => e.Id == id) > 0;
        }
    }
}
