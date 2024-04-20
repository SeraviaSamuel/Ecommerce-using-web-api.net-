using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        [HttpPost("{id}")]
        public IActionResult GetById(int id)
        {
            Category category = categoryRepository.GetById(id);
            CategoryWithListOfProductDTO categoryWithListOfProductDTO = new CategoryWithListOfProductDTO();
            categoryWithListOfProductDTO.Name = category.Name;
            categoryWithListOfProductDTO.ProductNames = productRepository.GetByCategoryId(id).Select(p => p.Name).ToList();
            categoryWithListOfProductDTO.Id = category.Id;
            return Ok(categoryWithListOfProductDTO);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categories = categoryRepository.GetAll();
            List<CategoryDTO> categoryNameWithIdDTO = new List<CategoryDTO>();
            foreach (Category cat in categories)
            {
                CategoryDTO dto = new CategoryDTO
                {
                    Id = cat.Id,
                    Name = cat.Name
                };
                categoryNameWithIdDTO.Add(dto);

            }
            return Ok(categoryNameWithIdDTO);
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.Id = categoryDTO.Id;
                category.Name = categoryDTO.Name;
                categoryRepository.Insert(category);
                categoryRepository.Save();
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [HttpPut]
        public IActionResult Edit(int id, CategoryDTO updatedCategory)
        {
            Category category = categoryRepository.GetById(id);
            if (category == null)
            {
                return BadRequest("Invalid");
            }
            category.Name = updatedCategory.Name;
            categoryRepository.Update(category);
            categoryRepository.Save();
            return NoContent();

        }
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            Category category = categoryRepository.GetById(id);
            if (category == null)
            {
                return BadRequest();
            }
            categoryRepository.Delete(id);
            categoryRepository.Save();
            return Ok("deleted successfully");
        }

    }
}
