using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RealStateAPI.Data;
using RealStateAPI.Models;

namespace RealStateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesControllerLearning : ControllerBase
    {
        ApiDBContext _dbContext = new ApiDBContext();
        // GET: api/<CategoriesController>
        [HttpGet]
        public IActionResult Get()
        {
           
            return Ok(_dbContext.Categories);
        }
        
        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category =  _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound("No record found against this Id" + id);
            }

            return Ok(category);
        }
        [HttpGet("[action]")]
        public IActionResult GetSortCategories()
        {
            return Ok(_dbContext.Categories.OrderByDescending(x => x.Name));
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category categoryObj)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound("No record found against this Id" + id);
            }else
            {
                category.Name = categoryObj.Name;
                category.ImageUrl = categoryObj.ImageUrl;
                category.Description = categoryObj.Description;
                _dbContext.SaveChanges();

                return Ok("Record updated sucessfully");
            }
           
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if(category == null)
            {
                return NotFound("No record found against this Id" + id);
            }
       
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return Ok("Record deleted");
        }
    }
}
         