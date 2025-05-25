using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    //API Controller
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // endPoint : public non static method
        // sort : nameasc [default] || namedescending || priceasc || pricedescending
        // [FromQuery] : to stop 415 error as we tell him take the parameter of the function and class from the query

        [HttpGet] // Get: api/Products
        public async Task<IActionResult> GetAllProducts( [FromQuery] ProductSpecificationParameters specParams)
        { 
            var result = await serviceManager.ProductService.GetAllProductsAsync(specParams);
            if (result is null) return BadRequest();  //400 
            return Ok(result); //200
        }


        [HttpGet("{id}")] // Get: api/Products/id
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync(id);
            if (result is null) return NotFound();  //404 
            return Ok(result); //200
        }


        [HttpGet("brands")]  // Get: api/Products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();  //400 
            return Ok(result); //200
        }


        [HttpGet("types")]   // Get: api/Products/types
        public async Task<IActionResult> GetAllTypes()
        { 
            var result = await serviceManager.ProductService.GetAllTypessAsync();
            if (result is null) return BadRequest(); return Ok(result);
        }

    }
}
