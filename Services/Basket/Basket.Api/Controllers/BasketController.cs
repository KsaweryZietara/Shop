using Basket.Api.Data;
using Basket.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers {

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase {
        private readonly IBasketRepo repository;

        public BasketController(IBasketRepo repository) {
            this.repository = repository;
        }

        //GET api/v1/basket/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync([FromRoute] string id) {
            var basket = await repository.GetBasketByIdAsync(id);

            if(basket == null){
                return NotFound();
            }

            return Ok(basket);
        }

        //PUT api/v1/basket/
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> UpdateBasketAsync([FromBody] CustomerBasket basket) {
            var updatedBasket = await repository.UpdateBasketAsync(basket);

            if(updatedBasket == null) {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetBasketByIdAsync), new {id = basket.BuyerId}, null);
        }

        //DELETE api/v1/basket/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteBasketAsync([FromRoute] string id) {
            var deletedBasket = await repository.DeleteBasketAsync(id);

            if(!deletedBasket) {
                return NotFound();
            }

            return NoContent();
        }
    }
}