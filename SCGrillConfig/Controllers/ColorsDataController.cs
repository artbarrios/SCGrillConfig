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
    public class ColorsDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetColorGrillConfgurations/?ColorId=1
        [Route("api/GetColorGrillConfgurations/")]
        public List<GrillConfguration> GetColorGrillConfgurations(int ColorId)
        {
            Color color = db.Colors.Find(ColorId);
            if (color == null)
            {
                return null;
            }
            return color.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToColor/?ColorId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToColor/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToColor(int ColorId, int GrillConfgurationId)
        {
            Color color = db.Colors.Find(ColorId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (color != null && grillConfguration != null)
            {
                try
                {
                    color.GrillConfgurations.Add(grillConfguration);
                    db.Entry(color).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToColor - ColorId:" + color.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
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
        } // AddGrillConfgurationToColor

        // PUT: api/RemoveGrillConfgurationFromColor/?ColorId=1&GrillConfgurationId=1
        // NOTE: the GrillConfguration.ColorId property is not nullable so this routine must be omitted

        // GET: api/ColorsData
        public IQueryable<Color> GetColors()
        {
            return db.Colors;
        }

        // GET: api/ColorsData/5
        [ResponseType(typeof(Color))]
        public IHttpActionResult GetColor(int id)
        {
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return NotFound();
            }

            return Ok(color);
        }

        // PUT: api/ColorsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutColor(int id, Color color)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != color.Id)
            {
                return BadRequest();
            }

            db.Entry(color).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/ColorsData/ - ColorId:" + color.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColorExists(id))
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

        // POST: api/ColorsData
        [ResponseType(typeof(Color))]
        public IHttpActionResult PostColor(Color color)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Colors.Add(color);
            db.SaveChanges();
            LogManager.Log("POST: api/ColorsData/ - ColorId:" + color.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = color.Id }, color);
        }

        // DELETE: api/ColorsData/5
        [ResponseType(typeof(Color))]
        public IHttpActionResult DeleteColor(int id)
        {
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return NotFound();
            }

            db.Colors.Remove(color);
            db.SaveChanges();
            LogManager.Log("DELETE: api/ColorsData/ - ColorId:" + color.Id.ToString());

            return Ok(color);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ColorExists(int id)
        {
            return db.Colors.Count(e => e.Id == id) > 0;
        }
    }
}
