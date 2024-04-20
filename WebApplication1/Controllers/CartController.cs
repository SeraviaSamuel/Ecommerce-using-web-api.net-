using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.cartRepository = cartRepository;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCart(CartDTO cartDTO)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);
                Cart cart = mapper.Map<Cart>(cartDTO);
                cart.Customer_Id = currentUser.Id;
                cartRepository.Insert(cart);
                cartRepository.Save();
                return Ok("Added Successfully");
            }
            return BadRequest();
        }
        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            List<Cart> carts = cartRepository.GetAll();
            if (carts != null)
            {
                List<CartWithProductNameDTO> dTOs = new List<CartWithProductNameDTO>();
                foreach (Cart cart in carts)
                {
                    CartWithProductNameDTO dTO = new CartWithProductNameDTO
                    {
                        Name = cart.product.Name,
                        Quantity = cart.Quantity,
                    };
                    dTOs.Add(dTO);
                }
                return Ok(dTOs);
            }
            return BadRequest();
        }
        [HttpGet("GetAllForSpecificUser")]
        [Authorize]
        public async Task<IActionResult> GetAllForSpecificUser()
        {
            var currentUser = await userManager.GetUserAsync(User);
            List<Cart> ALLcarts = cartRepository.GetAll();
            List<Cart> carts = new List<Cart>();
            foreach (Cart cart in ALLcarts)
            {
                if (cart.Customer_Id == currentUser.Id)
                {
                    carts.Add(cart);
                }
            }
            if (carts != null)
            {
                List<CartWithProductNameDTO> dTOs = new List<CartWithProductNameDTO>();
                foreach (Cart cart in carts)
                {
                    CartWithProductNameDTO dTO = new CartWithProductNameDTO
                    {
                        Name = cart.product.Name,
                        Quantity = cart.Quantity,
                    };
                    dTOs.Add(dTO);
                }
                return Ok(dTOs);
            }
            return BadRequest();
        }
        [HttpPut("{productId}")]
        public IActionResult UpdateCartQuantity(int productId, [FromBody] int newQuantity)
        {
            Cart cart = cartRepository.GetByProductId(productId);


            if (cart == null)
            {
                return NotFound("Cart not found");
            }
            cart.Quantity = newQuantity;
            cartRepository.Update(cart);
            cartRepository.Save();
            return Ok("Cart quantity updated successfully");
        }

        [HttpDelete("{productId:int}")]
        [Authorize]
        public IActionResult Remove(int productId)
        {
            Cart cart = cartRepository.GetByProductId(productId);
            if (cart != null)
            {
                cartRepository.Delete(cart.Id);
                cartRepository.Save();
                return Ok("Deleted Successfully");
            }
            return NotFound("Cart item not found");
        }
    }

}
