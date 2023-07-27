using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateAPI.Data;
using RealStateAPI.Models;
using System.Security.Claims;

namespace RealStateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        ApiDBContext _dbContext = new ApiDBContext();

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Property property)
        {
            if (property == null)
            {
                return NoContent();
            }
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.First(u => u.Email == userEmail);
            if (user == null) return NotFound();
            property.IsTreding = false;
            property.UserId = user.Id;
            _dbContext.Properties.Add(property);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Update with put
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] Property property) {
            var propertyResult = _dbContext.Properties.FirstOrDefault(p => p.Id == id);
            if (propertyResult == null) return NotFound();
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.First(u => u.Email == userEmail);
            if (user == null) return NotFound();
            if (propertyResult.UserId == user.Id)
            {
                propertyResult.Name = property.Name;
                propertyResult.Detail = property.Detail;
                propertyResult.Price = property.Price;
                propertyResult.Address = property.Address;
                property.IsTreding = false;
                property.UserId = user.Id;

                _dbContext.SaveChanges();
                return Ok("Property updated successfully");
            }

            return BadRequest();
          
        }

        //Delete
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
             var propertyResult = _dbContext.Properties.FirstOrDefault(p => p.Id == id);
            if (propertyResult == null) return NotFound();
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.First(u => u.Email == userEmail);
            if (user == null) return NotFound();
            if (propertyResult.UserId == user.Id)
            {
                _dbContext.Properties.Remove(propertyResult);
                _dbContext.SaveChanges();
                return Ok("Property deleted successfully");
            }
            return BadRequest();
        }

        //Get all properties
        [HttpGet("PropertyList")]
        [Authorize]
        public IActionResult GetProperties(int categoryId)
        {
            var propertiesResult = _dbContext.Properties.Where(p => p.CategoryId == categoryId).ToList(); // where list of properties
            if (propertiesResult == null) return NotFound();
            return Ok(propertiesResult);
        }

        //get property details
        [HttpGet("PropertyDetail")]
        [Authorize]
        public IActionResult GetPropertyDetail(int id)
        {
            var propertyResult = _dbContext.Properties.FirstOrDefault(p => p.Id == id);  // first or default returns one property
            if (propertyResult == null) return NotFound();
            return Ok(propertyResult);
        }

        //get treding properties
        [HttpGet("TredingProperties")]
        [Authorize]
        public IActionResult GetTredingProperties()
        {
            var propertiesResult = _dbContext.Properties.Where(p => p.IsTreding == true).ToList(); // where list of properties, ToList() can be removed
            if (propertiesResult == null) return NotFound();
            return Ok(propertiesResult);
        }

        // Search properties
        [HttpGet("SearchProperties")]
        [Authorize]
        public IActionResult GetSearchProperties(string address) {
            var propertiesResult = _dbContext.Properties.Where(p => p.Address.Contains(address)).ToList();
            if (propertiesResult == null) return NotFound();
            return Ok(propertiesResult);
        }
    }
}
        
// the jason to add a property: id, detail, addres, price, imageurl, categoryId
