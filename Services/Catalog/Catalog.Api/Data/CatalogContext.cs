using Catalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Data {

    public class CatalogContext : DbContext {
        public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();

        public CatalogContext(DbContextOptions options) : base(options) {
        }
    }
}