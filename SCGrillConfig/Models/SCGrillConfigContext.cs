using SCGrillConfig.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SCGrillConfig.Models
{
    public class SCGrillConfigContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SCGrillConfigContext() : base("name=SCGrillConfigContext")
        {
            Database.SetInitializer<SCGrillConfigContext>(new MigrateDatabaseToLatestVersion<SCGrillConfigContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public System.Data.Entity.DbSet<SCGrillConfig.Models.GrillConfguration> GrillConfgurations { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.GrillType> GrillTypes { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.Fuel> Fuels { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.Material> Materials { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.Color> Colors { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.BuildTask> BuildTasks { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.BuildTaskGrillConfgurations> BuildTaskGrillConfgurations { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.SideBurnerType> SideBurnerTypes { get; set; }

        public System.Data.Entity.DbSet<SCGrillConfig.Models.GrillSize> GrillSizes { get; set; }
    } // public class SCGrillConfigContext : DbContext
}
