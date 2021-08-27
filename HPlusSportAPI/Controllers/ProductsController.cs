using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPlusSportAPI.Classes;
using HPlusSportAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSportAPI.Controllers
{
    [ApiVersion("1.0")]
    //[Route("v{v:apiVersion}/products")]
    [Route("products")]
    [ApiController]
    public class ProductsV1_0Controller : ControllerBase
    {

        private readonly ShopContext _shopContext;

        public ProductsV1_0Controller(ShopContext context)
        {
            _shopContext = context;
            _shopContext.Database.EnsureCreated();
        }

        //[HttpGet]

        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return _shopContext.Products.ToArray();
        //}

        [HttpGet]

        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            IQueryable<Product> produts = _shopContext.Products;

            if (queryParameters.MaxPrice != null && queryParameters.MinPrice != null)
            {
                produts = produts.Where(

                    p => p.Price >= queryParameters.MinPrice.Value &&
                        p.Price <= queryParameters.MaxPrice.Value);

            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                produts = produts.Where(
                    p => p.Sku == queryParameters.Sku
                   );
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                produts = produts.Where(

                    p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    produts = produts.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            {

                produts = produts.Where(

                 p => p.Name.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                      p.Sku.ToLower().Contains(queryParameters.SearchTerm.ToLower())

                      );

            }


            produts = produts
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(await produts.ToListAsync());
        }

        //[HttpGet][Route("products/{id}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]

        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {

            if (id != product.Id)
            {
                return BadRequest();

            }

            _shopContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (_shopContext.Products.Find(id) == null)
                {
                    return NotFound();
                }
                throw;

            }

            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _shopContext.Products.Remove(product);

            await _shopContext.SaveChangesAsync();

            return product;

        }

        [HttpPost]
        [Route("Delete")]

        public async Task<IActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();

            foreach (var id in ids)
            {
                var product = await _shopContext.Products.FindAsync(id);

                if (product == null)
                { return NotFound(); }

                products.Add(product);
            }

            _shopContext.RemoveRange(products);
            await _shopContext.SaveChangesAsync();

            return Ok(products);
        }
    }


    [ApiVersion("2.0")]
    //[Route("v{v:apiVersion}/products")]
    [Route("products")]
    [ApiController]
    public class ProductsV2_0Controller : ControllerBase
    {

        private readonly ShopContext _shopContext;

        public ProductsV2_0Controller(ShopContext context)
        {
            _shopContext = context;
            _shopContext.Database.EnsureCreated();
        }

        //[HttpGet]

        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return _shopContext.Products.ToArray();
        //}

        [HttpGet]

        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            IQueryable<Product> produts = _shopContext.Products.Where(p => p.IsAvailable == true);

            if (queryParameters.MaxPrice != null && queryParameters.MinPrice != null)
            {
                produts = produts.Where(

                    p => p.Price >= queryParameters.MinPrice.Value &&
                        p.Price <= queryParameters.MaxPrice.Value);

            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                produts = produts.Where(
                    p => p.Sku == queryParameters.Sku
                   );
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                produts = produts.Where(

                    p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    produts = produts.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }

            if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
            {

                produts = produts.Where(

                 p => p.Name.ToLower().Contains(queryParameters.SearchTerm.ToLower()) ||
                      p.Sku.ToLower().Contains(queryParameters.SearchTerm.ToLower())

                      );

            }


            produts = produts
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(await produts.ToListAsync());
        }

        //[HttpGet][Route("products/{id}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]

        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {

            if (id != product.Id)
            {
                return BadRequest();

            }

            _shopContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (_shopContext.Products.Find(id) == null)
                {
                    return NotFound();
                }
                throw;

            }

            return NoContent();

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _shopContext.Products.Remove(product);

            await _shopContext.SaveChangesAsync();

            return product;

        }

        [HttpPost]
        [Route("Delete")]

        public async Task<IActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();

            foreach (var id in ids)
            {
                var product = await _shopContext.Products.FindAsync(id);

                if (product == null)
                { return NotFound(); }

                products.Add(product);
            }

            _shopContext.RemoveRange(products);
            await _shopContext.SaveChangesAsync();

            return Ok(products);
        }
    }

}