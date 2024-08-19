using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Microsoft.EntityFrameworkCore;

namespace ETA.API.DbContexts
{
    public class CourseLibraryContext : DbContext
    {
        public CourseLibraryContext(DbContextOptions<CourseLibraryContext> options) : base(options) { }
        public virtual DbSet<UsersStoreProcModel> UsersStoreProcModel { get; set; }
        public virtual DbSet<PatientsProcModel> PatientsProcModels { get; set; }
        public virtual DbSet<ProvidersProcModel> ProvidersProcModels { get; set; }
        public virtual DbSet<PatientCityState> PatientCityState { get; set; }
        public virtual DbSet<PatientGeneDumpProcModel> PatientGeneDumpProcModel { get; set; }
        public virtual DbSet<PatientReportDetailsModel> PatientReportDetailsModel { get; set; }




        //This method in Dbcontext class is used to map database tables and relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersStoreProcModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PatientsProcModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ProvidersProcModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PatientCityState>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PatientGeneDumpProcModel>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<PatientReportDetailsModel>(builder => { builder.HasNoKey(); });


        }
    }
}