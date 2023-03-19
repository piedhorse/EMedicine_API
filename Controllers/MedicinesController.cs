using EMedicineBE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace EMedicineBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public MedicinesController(IConfiguration configuration)
        {


            _configuration = configuration;
        }

        [HttpPost]
        [Route("addToCart")]
        public Response addToCart(Cart cart)
        {
            DAL DAL = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EMedCS").ToString());
            Response response = DAL.addToCart(cart, connection);
            return response;

        }
        [HttpPost]
        [Route("placeOrder")]
        public Response placeOrder(Users users)
        {
            DAL DAL = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EMedCS").ToString());
            Response response = DAL.placeOrder(users, connection);
            return response;

        }

        [HttpPost]
        [Route("OrderList")]
        public Response OrderList(Users users)
        {
            DAL DAL = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EMedCS").ToString());
            Response response = DAL.OrderList(users, connection);
            return response;

        }
    }
}
