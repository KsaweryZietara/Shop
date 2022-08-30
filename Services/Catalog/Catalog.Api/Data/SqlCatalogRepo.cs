using Catalog.Api.Dtos;
using Catalog.Api.Models;

namespace Catalog.Api.Data {

    public class SqlCatalogRepo : ICatalogRepo {
        private readonly CatalogContext context;

        public SqlCatalogRepo(CatalogContext context){
            this.context = context;    
        }

        public async Task<CatalogItem> CreateItemAsync(CatalogItem item) {
            if(item == null) { 
                throw new ArgumentNullException(nameof(item));
            }

            await context.CatalogItems.AddAsync(item);
            await context.SaveChangesAsync();

            return item;
        }

        public IEnumerable<CatalogItem> GetAllItems() {
            return context.CatalogItems.ToList();
        }

        public CatalogItem? GetItemById(int id) {
            return context.CatalogItems.FirstOrDefault(x => x.Id == id);
        }

        public async Task<CatalogItem?> UpdateItemAsync(UpdateItemDto item) {
            if(item == null) { 
                throw new ArgumentNullException(nameof(item));
            }
            
            var catalogItem = context.CatalogItems.SingleOrDefault(x => x.Id == item.Id);

            if(catalogItem == null) { 
                return null;
            }

            catalogItem.Name = item.Name;
            catalogItem.Description = item.Description;
            catalogItem.Price = item.Price;
            catalogItem.Brand = item.Brand;
            catalogItem.AvailableStock = item.AvailableStock;

            await context.SaveChangesAsync();

            return catalogItem;
        }

        public async Task<CatalogItem> DeleteItemAsync(CatalogItem item) {
            if(item == null) { 
                throw new ArgumentNullException(nameof(item));
            }

            context.Remove(item);
            await context.SaveChangesAsync();

            return item;
        }
    }
}