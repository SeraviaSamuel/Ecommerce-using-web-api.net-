using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Context context;
        private readonly IProductRepository productRepository;

        public ProductController(Context _context, IProductRepository productRepository)
        {
            context = _context;
            this.productRepository = productRepository;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            List<Product> productsList = productRepository.GetAll();
            return Ok(productsList);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetByID(int id)
        {
            Product product = productRepository.GetById(id);
            return Ok(product);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddDProduct(Product newProdcut)//from request body
        {
            if (ModelState.IsValid == true)
            {
                productRepository.Insert(newProdcut);
                productRepository.Save();
                return CreatedAtAction("GetByID", new { id = newProdcut.Id }, newProdcut);
            }
            return BadRequest(ModelState);
        }
        [HttpPut]//httpPatch
        [Authorize]
        public IActionResult Edit(int id, Product updatedProduct)
        {
            Product productFromDB = productRepository.GetById(id);

            if (productFromDB == null)
            {
                return BadRequest("Invalid ID");
            }
            else if (productFromDB.Id != productFromDB.Id)
            {
                return BadRequest("Invalid ID");

            }
            productFromDB.Name = updatedProduct.Name;
            productFromDB.Price = updatedProduct.Price;
            productFromDB.Description = updatedProduct.Description;
            productRepository.Save();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Remove(int id)
        {
            try
            {
                Product product = productRepository.GetById(id);
                productRepository.Delete(id);
                productRepository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
