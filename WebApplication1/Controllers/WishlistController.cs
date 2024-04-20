using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishListRepository wishListRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public WishlistController(IWishListRepository wishListRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.wishListRepository = wishListRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<WishList> wishlist = wishListRepository.GetAll();
            if (wishlist != null)
            {
                List<ClientNameWithrodctNameDTO> dTOs = new List<ClientNameWithrodctNameDTO>();
                foreach (WishList wish in wishlist)
                {
                    ClientNameWithrodctNameDTO dTO = new ClientNameWithrodctNameDTO();
                    dTO.productName = wish.product.Name;
                    dTO.userName = wish.customer.UserName;
                    dTOs.Add(dTO);
                }
                return Ok(dTOs);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddWishList(WishlistDTO wishlistDTO)
        {
            if (ModelState.IsValid)
            {
                var currenUser = await userManager.GetUserAsync(User);
                string userId = currenUser.Id;
                WishList wishList = mapper.Map<WishList>(wishlistDTO);
                wishList.Customer_Id = userId;
                wishListRepository.Insert(wishList);
                wishListRepository.Save();
                return Ok("Added Successfully");
            }
            return BadRequest("bad");
        }
        [HttpGet("byCustomerId/{id}")]
        public IActionResult GetByCustomerId(string id)
        {
            List<WishList> wishList = wishListRepository.GetByCustomerId(id);
            if (wishList != null)
            {
                List<ProductNameDTO> dTOs = new List<ProductNameDTO>();
                foreach (WishList wish in wishList)
                {
                    ProductNameDTO dTO = new ProductNameDTO()
                    {
                        productName = wish.product.Name,
                    };
                    dTOs.Add(dTO);
                }

                return Ok(dTOs);
            }

            return NotFound("Not Found");
        }
        [HttpGet("byCustomerName/{userName}")]
        public IActionResult GetByCustomerName(string userName)
        {
            List<WishList> wishLists = wishListRepository.GetByCustomerName(userName);
            if (wishLists != null)
            {
                List<ProductNameDTO> dTOs = new List<ProductNameDTO>();
                foreach (WishList wishList in wishLists)
                {
                    ProductNameDTO dTO = new ProductNameDTO
                    {
                        productName = wishList.product.Name,
                    };
                    dTOs.Add(dTO);
                }
                return Ok(dTOs);
            }
            return BadRequest();
        }
        [HttpPut("{productId:int}")]
        public async Task<IActionResult> Edit(int productId, int newProductId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            string userId = currentUser.Id;
            List<WishList> wishLists = wishListRepository.GetByCustomerId(userId);
            WishList existingWishList = wishListRepository.FindByProductId(productId);
            if (existingWishList != null)
            {
                existingWishList.Product_Id = newProductId;
                wishListRepository.Update(existingWishList);
                wishListRepository.Save();
                return Ok("Updated Successfully");
            }
            return BadRequest();
        }
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            List<WishList> wishLists = wishListRepository.GetAll();
            foreach (WishList wishList in wishLists)
            {
                if (wishList.Id == id)
                {
                    wishListRepository.Delete(id);
                    wishListRepository.Save();
                    return Ok("Deleted Successfully");

                }
            }
            return NotFound();
        }

    }
}
