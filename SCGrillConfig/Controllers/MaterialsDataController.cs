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
    public class MaterialsDataController : ApiController
    {
        private SCGrillConfigContext db = new SCGrillConfigContext();

        // GET: api/GetMaterialGrillConfgurations/?MaterialId=1
        [Route("api/GetMaterialGrillConfgurations/")]
        public List<GrillConfguration> GetMaterialGrillConfgurations(int MaterialId)
        {
            Material material = db.Materials.Find(MaterialId);
            if (material == null)
            {
                return null;
            }
            return material.GrillConfgurations;
        }

        // PUT: api/AddGrillConfgurationToMaterial/?MaterialId=1&GrillConfgurationId=1
        [HttpPut]
        [Route("api/AddGrillConfgurationToMaterial/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddGrillConfgurationToMaterial(int MaterialId, int GrillConfgurationId)
        {
            Material material = db.Materials.Find(MaterialId);
            GrillConfguration grillConfguration = db.GrillConfgurations.Find(GrillConfgurationId);
            if (material != null && grillConfguration != null)
            {
                try
                {
                    material.GrillConfgurations.Add(grillConfguration);
                    db.Entry(material).State = EntityState.Modified;
                    db.SaveChanges();
                    LogManager.Log("api/AddGrillConfgurationToMaterial - MaterialId:" + material.Id.ToString() + " GrillConfgurationId:" + grillConfguration.Id.ToString());
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
        } // AddGrillConfgurationToMaterial

        // PUT: api/RemoveGrillConfgurationFromMaterial/?MaterialId=1&GrillConfgurationId=1
        // NOTE: the GrillConfguration.MaterialId property is not nullable so this routine must be omitted

        // GET: api/MaterialsData
        public IQueryable<Material> GetMaterials()
        {
            return db.Materials;
        }

        // GET: api/MaterialsData/5
        [ResponseType(typeof(Material))]
        public IHttpActionResult GetMaterial(int id)
        {
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        // PUT: api/MaterialsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMaterial(int id, Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != material.Id)
            {
                return BadRequest();
            }

            db.Entry(material).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogManager.Log("PUT: api/MaterialsData/ - MaterialId:" + material.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
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

        // POST: api/MaterialsData
        [ResponseType(typeof(Material))]
        public IHttpActionResult PostMaterial(Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Materials.Add(material);
            db.SaveChanges();
            LogManager.Log("POST: api/MaterialsData/ - MaterialId:" + material.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = material.Id }, material);
        }

        // DELETE: api/MaterialsData/5
        [ResponseType(typeof(Material))]
        public IHttpActionResult DeleteMaterial(int id)
        {
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return NotFound();
            }

            db.Materials.Remove(material);
            db.SaveChanges();
            LogManager.Log("DELETE: api/MaterialsData/ - MaterialId:" + material.Id.ToString());

            return Ok(material);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialExists(int id)
        {
            return db.Materials.Count(e => e.Id == id) > 0;
        }
    }
}
