using Catalog.Api.Dtos;
using Catalog.Api.Models;

namespace Catalog.Api.Data {

    public interface ICatalogRepo {
        Task<CatalogItem> CreateItemAsync(CatalogItem item);

        IEnumerable<CatalogItem> GetAllItems();

        CatalogItem? GetItemById(int id);

        Task<CatalogItem?> UpdateItemAsync(UpdateItemDto item);

        Task<CatalogItem> DeleteItemAsync(CatalogItem item);
    }
}