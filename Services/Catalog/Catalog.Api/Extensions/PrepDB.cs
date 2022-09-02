using Catalog.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Extensions {
    
    public static class PrepDB {
        public static void MigrateDatabase(IApplicationBuilder app){
            using (var serviceScope = app.ApplicationServices.CreateScope()) {
                var context = serviceScope.ServiceProvider.GetService<CatalogContext>();
                context?.Database.Migrate();
            }
        }
    }
}