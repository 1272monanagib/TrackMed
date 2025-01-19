using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrackMed.Domain.Shared;

namespace TrackMed.Repository.Context
{
    public class TrackMedAppContext: IdentityDbContext<IdentityUser>
    {
        public TrackMedAppContext(DbContextOptions<TrackMedAppContext> options)
            : base(options) 
        {

        }
 
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            foreach (var entityType in modelbuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = SetGlobalQueryFilterMethod?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(this, [modelbuilder]);
                }
            }
        }
        private static readonly MethodInfo? SetGlobalQueryFilterMethod = typeof(TrackMedAppContext)
            .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static);

        private static void SetGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedOn = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
