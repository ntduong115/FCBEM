using Core.Interfaces;
using Core.Models.Systems;

using FCBEMModel.Models.Authorize;
using FCBEMModel.PaperModels;
using FCBEMModel.Registrations;
using FCBEMModel.Systems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

using static Core.Commons.FCBEMConstants;



namespace FCBEMModel
{
    public class DatabaseContext : IdentityDbContext<User, Role, Guid>
    {
        public DatabaseContext()
            : base()
        {

        }
        public virtual DbSet<Log> Logs { get; set; }


        #region Papers 
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Paper> Papers { get; set; }
        public virtual DbSet<PaperNew> PaperNews { get; set; }
        #endregion Papers 

        #region Systems 
        public virtual DbSet<ProjectVersion> ProjectVersions { get; set; }
        #endregion Systems 

        #region Registrations 
        public virtual DbSet<Registration> Registrations { get; set; }
        #endregion Registrations 


        public DatabaseContext(DbContextOptions<DatabaseContext> options)
           : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Relation ship

            modelBuilder.Entity<Author>()
                .HasOne(pt => pt.Paper)
                .WithMany(p => p.Authors)
                .HasForeignKey(pt => pt.PaperId);

            #endregion Relation ship

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                string tableName = entityType.GetTableName() ?? string.Empty;
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }

                foreach (IMutableProperty prop in entityType.GetDeclaredProperties())
                {
                    if (prop.Name == nameof(IEntity.CreatedDate) || prop.Name == nameof(IEntity.UpdatedDate))
                        modelBuilder.Entity(entityType.ClrType).Property(prop.Name).HasDefaultValueSql("getdate()");
                }

            }

            #region seed data

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables(); // Có thể thêm các nguồn cấu hình khác


            IConfiguration configuration;
            configuration = builder.Build();
            string ProjectName = configuration["ProjectName"];
            string ProjectYear = configuration["ProjectYear"];
            ProjectVersion version = new()
            {
                Code = ProjectName + ProjectYear,
                Year = int.Parse("20" + ProjectYear),
            };


            #region authorize


            Role adminRole = new() { Id = Guid.NewGuid(), Name = RoleName.Admin, NormalizedName = RoleName.Admin.ToUpper(), ConcurrencyStamp = "253eab8b-7250-4521-bc46-000c8eaaf1df", UpdatePIC = RoleName.Admin };
            Role userRole = new() { Id = Guid.NewGuid(), Name = RoleName.Client, NormalizedName = RoleName.Client.ToUpper(), ConcurrencyStamp = "9924f689-06b0-4287-977b-604091defa04", UpdatePIC = RoleName.Admin };
            List<Role> roles = [
                adminRole,
                userRole,
            ];

            modelBuilder.Entity<ProjectVersion>().HasData(version);
            //modelBuilder.Entity<Role>().HasData(roles);

            #endregion authorize

            #endregion seed data
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((IEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    if (entityEntry.Entity is IEntityVersion version)
                    {
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables(); // Có thể thêm các nguồn cấu hình khác


                        IConfiguration configuration;
                        configuration = builder.Build();
                        string ProjectName = configuration["ProjectName"];
                        string ProjectYear = configuration["ProjectYear"];
                        version.VersionCode ??= ProjectName + ProjectYear;
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
