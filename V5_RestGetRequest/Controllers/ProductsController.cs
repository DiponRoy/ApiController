using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductEntity;
using V5_RestGetRequest.Data;
using V5_RestGetRequest.Data.Models;
using V5_RestGetRequest.Helpers;

namespace V5_RestGetRequest.Controllers
{
    public class ProductsController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {

            try
            {
                List<Products> products = ProductsData.SelectAll().ToList<Products>();
                return Request.CreateResponse(HttpStatusCode.OK, products);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Products product = ProductsData.Select_Record(new Products() { ProductId = id });
                if (product == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with given Id does not exist!");

                }
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        public HttpResponseMessage Post(Products product)
        {
            try
            {
                ProductsData.Add(product);
                HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, product);
                message.Headers.Location = new Uri(Request.RequestUri + product.ProductId.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Products newProduct)
        {
            try
            {
                Products existingProduct = ProductsData.Select_Record(new Products() { ProductId = id });
                if (existingProduct == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with Given Id does not exist!");
                }
                ProductsData.Update(existingProduct, newProduct);

                return Request.CreateResponse(HttpStatusCode.OK, newProduct);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Products product = ProductsData.Select_Record(new Products() { ProductId = id });
                if (product == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Product with Given Id does not exist!");
                }

                ProductsData.Delete(product);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
