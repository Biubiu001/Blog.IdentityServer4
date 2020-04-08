using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Core.AuthHelper.OverWrite;
using Web.Core.Common.Redis;
using Web.Core.IServices;
using Web.Core.Model.Models;


namespace Web.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        private IAdvertisementService _advertisementService;
        private IRedisCacheManage _cache;
        public BlogController(IAdvertisementService advertisementService,IRedisCacheManage cache) {

            _advertisementService = advertisementService;
            _cache = cache;
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
         

            return  await _advertisementService.Query(c => c.Id == 10);

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
        [HttpGet("GetBlogs")]
        public void GetBlogs() {

            //Advertisement advertisement = new Advertisement { Id=1,ImgUrl="textimge",Title="title",Url="ImgUrl"};
            //_cache.Set("Web.core",advertisement,TimeSpan.fromhou(3));    

        string value= _cache.GetValue("Web.core");
        
        }
    }
}