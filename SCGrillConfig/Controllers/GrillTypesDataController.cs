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
    public class GrillTypesDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetGrillTypeGrillConfgurations/?GrillTypeId=1
        [Route("api/GetGrillTypeGrillConfgurations/")]
        public List<GrillConfguration> GetGrillTypeGrillConfgurations(int GrillTypeId)
        {
            GrillType grillType = db.GrillTypes.Find(GrillTypeId);
            if (grillType == null)
            {
                return null;
            }
            return grillType.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToGrillType/?GrillTypeId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToGrillType/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToGrillType(int GrillTypeId, int GrillConfgurationId)
        {
            GrillType grillType = db.GrillTypes.Find(GrillTypeId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (grillType != null && grillConfguration != null)
            {
                try
                {
                    grillType.GrillConfgurations.Add(grillConfguration);
                    db.Entry(grillType).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToGrillType - GrillTypeId:" + grillType.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
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
        } // AddGrillConfgurationToGrillType

        // PUT: api/RemoveGrillConfgurationFromGrillType/?GrillTypeId=1&GrillConfgurationId=1
        // NOTE: the GrillConfguration.GrillTypeId property is not nullable so this routine must be omitted

        // GET: api/GrillTypesData
        public IQueryable<GrillType> GetGrillTypes()
        {
            return db.GrillTypes;
        }

        // GET: api/GrillTypesData/5
        [ResponseType(typeof(GrillType))]
        public IHttpActionResult GetGrillType(int id)
        {
            GrillType grillType = db.GrillTypes.Find(id);
            if (grillType == null)
            {
                return NotFound();
            }

            return Ok(grillType);
        }

        // PUT: api/GrillTypesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrillType(int id, GrillType grillType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grillType.Id)
            {
                return BadRequest();
            }

            db.Entry(grillType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/GrillTypesData/ - GrillTypeId:" + grillType.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrillTypeExists(id))
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

        // POST: api/GrillTypesData
        [ResponseType(typeof(GrillType))]
        public IHttpActionResult PostGrillType(GrillType grillType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GrillTypes.Add(grillType);
            db.SaveChanges();
            LogManager.Log("POST: api/GrillTypesData/ - GrillTypeId:" + grillType.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = grillType.Id }, grillType);
        }

        // DELETE: api/GrillTypesData/5
        [ResponseType(typeof(GrillType))]
        public IHttpActionResult DeleteGrillType(int id)
        {
            GrillType grillType = db.GrillTypes.Find(id);
            if (grillType == null)
            {
                return NotFound();
            }

            db.GrillTypes.Remove(grillType);
            db.SaveChanges();
            LogManager.Log("DELETE: api/GrillTypesData/ - GrillTypeId:" + grillType.Id.ToString());

            return Ok(grillType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrillTypeExists(int id)
        {
            return db.GrillTypes.Count(e => e.Id == id) > 0;
        }
    }
}
