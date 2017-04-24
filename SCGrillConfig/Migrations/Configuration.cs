namespace SCGrillConfig.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SCGrillConfig.Models.SCGrillConfigContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SCGrillConfig.Models.SCGrillConfigContext";
        }

        protected override void Seed(SCGrillConfig.Models.SCGrillConfigContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // seed data for SCGrillConfig

            if (context.GrillTypes.Count() == 0)
            {
                context.GrillTypes.AddOrUpdate(
                    g => g.Id,
                    new GrillType { Id = 1, Name = "Built-In" },
                    new GrillType { Id = 2, Name = "Freestanding" },
                    new GrillType { Id = 3, Name = "Portable" }
                    );
                context.SaveChanges();
            }  // save seed data for GrillType

            if (context.Fuels.Count() == 0)
            {
                context.Fuels.AddOrUpdate(
                    f => f.Id,
                    new Fuel { Id = 1, Name = "Natural Gas" },
                    new Fuel { Id = 2, Name = "Propane" },
                    new Fuel { Id = 3, Name = "Wood" }
                    );
                context.SaveChanges();
            }  // save seed data for Fuel

            if (context.SideBurnerTypes.Count() == 0)
            {
                context.SideBurnerTypes.AddOrUpdate(
                    s => s.Id,
                    new SideBurnerType { Id = 1, Name = "None" },
                    new SideBurnerType { Id = 2, Name = "Power Burner" },
                    new SideBurnerType { Id = 3, Name = "Side Burner" }
                    );
                context.SaveChanges();
            }  // save seed data for SideBurnerType

            if (context.GrillSizes.Count() == 0)
            {
                context.GrillSizes.AddOrUpdate(
                    g => g.Id,
                    new GrillSize { Id = 1, Name = "Large (34 - 42 Inches)" },
                    new GrillSize { Id = 2, Name = "Medium (27 - 33 Inches)" },
                    new GrillSize { Id = 3, Name = "Small (0 - 26 Inches)" }
                    );
                context.SaveChanges();
            }  // save seed data for GrillSize

            if (context.Materials.Count() == 0)
            {
                context.Materials.AddOrUpdate(
                    m => m.Id,
                    new Material { Id = 1, Name = "Cast Aluminum" },
                    new Material { Id = 2, Name = "Painted Steel" },
                    new Material { Id = 3, Name = "Wood" }
                    );
                context.SaveChanges();
            }  // save seed data for Material

            if (context.Colors.Count() == 0)
            {
                context.Colors.AddOrUpdate(
                    c => c.Id,
                    new Color { Id = 1, Name = "Black" },
                    new Color { Id = 2, Name = "Gray" },
                    new Color { Id = 3, Name = "Green" }
                    );
                context.SaveChanges();
            }  // save seed data for Color

            if (context.BuildTasks.Count() == 0)
            {
                context.BuildTasks.AddOrUpdate(
                    b => b.Id,
                    new BuildTask { Id = 1, Name = "Confirm Inventory", Notes = "Notes1" },
                    new BuildTask { Id = 2, Name = "Assemble Parts", Notes = "Notes2" },
                    new BuildTask { Id = 3, Name = "Build Grill", Notes = "Notes3" }
                    );
                context.SaveChanges();
            }  // save seed data for BuildTask

            if (context.GrillConfgurations.Count() == 0)
            {
                context.GrillConfgurations.AddOrUpdate(
                    g => g.Id,
                    new GrillConfguration { Id = 1, Name = "Grill Type 1", MainBurnerCount = 1, InfraredBurnerCount = "1", GrillTypeId = 1, FuelId = 1, SideBurnerTypeId = 1, GrillSizeId = 1, MaterialId = 1, ColorId = 1, BuildTaskFlowchartDiagramData = "BuildTaskFlowchartDiagramData1" },
                    new GrillConfguration { Id = 2, Name = "Grill Type 2", MainBurnerCount = 2, InfraredBurnerCount = "2", GrillTypeId = 2, FuelId = 2, SideBurnerTypeId = 2, GrillSizeId = 2, MaterialId = 2, ColorId = 2, BuildTaskFlowchartDiagramData = "BuildTaskFlowchartDiagramData2" },
                    new GrillConfguration { Id = 3, Name = "Grill Type 3", MainBurnerCount = 3, InfraredBurnerCount = "3", GrillTypeId = 3, FuelId = 3, SideBurnerTypeId = 3, GrillSizeId = 3, MaterialId = 3, ColorId = 3, BuildTaskFlowchartDiagramData = "BuildTaskFlowchartDiagramData3" }
                    );
                context.SaveChanges();
            }  // save seed data for GrillConfguration

            if (context.BuildTaskGrillConfgurations.Count() == 0)
            {
                context.BuildTaskGrillConfgurations.AddOrUpdate(
                    b => b.Id,
                    new BuildTaskGrillConfgurations { Id = 1, BuildTaskId = 1, GrillConfgurationId = 1 },
                    new BuildTaskGrillConfgurations { Id = 2, BuildTaskId = 2, GrillConfgurationId = 2 },
                    new BuildTaskGrillConfgurations { Id = 3, BuildTaskId = 3, GrillConfgurationId = 3 }
                    );
                context.SaveChanges();
            }  // save seed data for BuildTaskGrillConfgurations


        }
    }
}
