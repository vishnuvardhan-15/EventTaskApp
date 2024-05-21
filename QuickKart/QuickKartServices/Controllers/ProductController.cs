using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QuickKartDataAccessLayer;
using QuickKartDataAccessLayer.Models;
using QuickKartServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuickKartServices.Controllers
{
    [Route("api/[controller]/[action]")] 
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly QuickKartRepository _repository;
        private readonly IMapper _mapper;
       
        public ProductController(QuickKartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public JsonResult GetProducts()
        {
            List<Models.Product> products = new List<Models.Product>();
            try
            {
                List<Products> productList = _repository.GetProducts();
                if(productList!=null)
                {
                    foreach(var item in productList)
                    {
                        Models.Product prodObj = _mapper.Map<Models.Product>(item);
                        products.Add(prodObj);
                    }
                }
            }
    
            catch(Exception ex)
            {
                products = null;
            }
          
            return new JsonResult(products);
        }

        [HttpGet]
        public JsonResult GetProductId(string productId)
        {
            Models.Product product = null;
            try
            {
                product = _mapper.Map<Models.Product>(_repository.GetProductDetails(productId));
            }
            catch(Exception ex)
            {
                product = null;
            }
            return new JsonResult(product);
        }

        [HttpPost]
        public JsonResult AddProd(Models.Product product)
        {
            bool status = false;
            status = _repository.AddProduct(_mapper.Map<Products>(product));
            return new JsonResult(status);

        }

        [HttpPut]
        public JsonResult UpdateProduct(Models.Product product)
        {
            bool status = false;
            try
            {
                status = _repository.UpdateProduct(_mapper.Map<Products>(product));

            }
            catch(Exception ex)
            {
                status = false;

            }
            return new JsonResult(status);
        }

        [HttpDelete]
        public JsonResult DeleteProduct(string productId)
        {
            bool status = false;
            try
            {
                status = _repository.DeleteProduct(productId);
            }
            catch(Exception ex)
            {
                status = false;
            }
            

            return new JsonResult(status);
        }
    }
}
