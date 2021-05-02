using AutoMapper;
using BlazorRestaurant.DataAccess.Data;
using BlazorRestaurant.DataAccess.Models;
using BlazorRestaurant.Shared.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorRestaurant.Server.Controllers
{
    /// <summary>
    /// In charge of products management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private BlazorRestaurantDbContext BlazorRestaurantDbContext { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ProductController"/>
        /// </summary>
        /// <param name="blazorRestaurantDbContext"></param>
        /// <param name="mapper"></param>
        public ProductController(BlazorRestaurantDbContext blazorRestaurantDbContext, IMapper mapper)
        {
            this.BlazorRestaurantDbContext = blazorRestaurantDbContext;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Add a new product
        /// <paramref name="productModel"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProduct(ProductModel productModel)
        {
            var productEntity = await this.BlazorRestaurantDbContext.Product
                .Where(p => p.Name == productModel.Name).SingleOrDefaultAsync();
            if (productEntity != null)
                throw new Exception($"There is already a product named: {productModel.Name}");
            else
            {
                productEntity = this.Mapper.Map<ProductModel, Product>(productModel);
                await this.BlazorRestaurantDbContext.Product.AddAsync(productEntity);
                await this.BlazorRestaurantDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        /// <summary>
        /// Lists all of the Product Types
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ProductTypeModel[]> ListProductTypes()
        {
            return await this.BlazorRestaurantDbContext.ProductType
                .Select(p => this.Mapper.Map<ProductType, ProductTypeModel>(p)).ToArrayAsync();
        }
    }
}
