using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Core.AuthHelper.OverWrite;
using Web.Core.IServices;
using Web.Core.Model.Models;


namespace Web.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        private IAdvertisementService _advertisementService;
        public BlogController(IAdvertisementService advertisementService) {

            _advertisementService = advertisementService;
        
        }
 
        //[Authorize(Roles ="Admin")]
        [HttpGet]
        public int Get(int i,int j)
        {
            //IAdvertisementService advertisementServices = new AdvertisementServices();
            //return advertisementServices.Sum(i,j);
            return 1;
        }

        [HttpGet("id")]
        public async Task< List<Advertisement>> Get(int id)
        {
         

            return  await _advertisementService.Query(c => c.Id == id);

        }
        // POST: api/Blog

        [HttpPost]
        public IActionResult GetJwt(TokenModelJwt tokenModelJwt)
        {
            return Ok(   JwtHelper.IssueJwt(tokenModelJwt));

        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}