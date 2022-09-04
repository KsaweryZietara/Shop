using Catalog.Api.Data;
using Catalog.Api.Dtos;
using Catalog.Api.IntegrationEvents.Events;
using Catalog.Api.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase {
        private readonly ICatalogRepo repository;

        private readonly IPublishEndpoint publishEndpoint;

        public CatalogController(ICatalogRepo repository, IPublishEndpoint publishEndpoint) {
            this.repository = repository;
            this.publishEndpoint = publishEndpoint;
        }

        //POST api/v1/catalog/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateItemAsync([FromBody] CatalogItem item) {
            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemById), new {id = item.Id}, null);
        }

        //GET api/v1/catalog/
        [HttpGet]
        [ProducesResponseType(typeof(List<CatalogItem>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CatalogItem>> GetAllItems() {
            var items = repository.GetAllItems();

            return Ok(items);
        }

        //GET api/v1/catalog/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CatalogItem), StatusCodes.Status200OK)]
        public ActionResult<CatalogItem> GetItemById([FromRoute] int id) {
            if(id < 0) {
                return BadRequest();
            }

            var item = repository.GetItemById(id);

            if(item == null){
                return NotFound();
            }

            return Ok(item);
        }

        //PUT api/v1/catalog/
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> UpdateItemAsync([FromBody] UpdateItemDto item){
            var catalogItem = await repository.UpdateItemAsync(item);

            if(catalogItem == null){
                return NotFound();
            }

            await publishEndpoint.Publish<ProductPriceChanged>(new {
                ProductId = item.Id,
                NewPrice = item.Price
            });

            return CreatedAtAction(nameof(GetItemById), new {id = item.Id}, null);
        }

        //DELETE api/v1/catalog/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteItemAsync([FromRoute] int id) {
            var catalogItem = repository.GetItemById(id);

            if(catalogItem == null) {
                return BadRequest();
            }

            await repository.DeleteItemAsync(catalogItem);

            return NoContent();
        }
    }
}