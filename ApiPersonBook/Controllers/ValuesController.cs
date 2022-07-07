using ApiPersonBook.Services;
using DataBaseLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiPersonBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        public ValuesController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getlist")]
        public async Task<string> Get()
        {
            var obj = await _dataRepository.GetListData();
            var bodyContent = JsonConvert.SerializeObject(obj);

            return bodyContent;
        }

        [HttpGet]
        [Authorize]
        [Route("getlist/{UserId}")]
        public async Task<string> GetEserList(string UserId)
        {
            var obj = await _dataRepository.GetUserListData(UserId);
            var bodyContent = JsonConvert.SerializeObject(obj);

            return bodyContent;
        }


        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<string> GetItem(int id)
        {
            var obj = await _dataRepository.GetData(id);
            var bodyContent = JsonConvert.SerializeObject(obj);

            return bodyContent;
        }

        [HttpPost]
        [Authorize]
        [Route("setdata")]
        public async Task Set([FromBody] Contact value)
        {
            await _dataRepository.CreateData(value);
        }

        [HttpPost]
        [Authorize]
        [Route("updata")]
        public async Task Update([FromBody] Contact value)
        {
            await _dataRepository.UpdateData(value);
        }

        [HttpDelete]
        [Authorize]
        [Route("deldata/{id}")]
        public async Task Delete(int id)
        {
            await _dataRepository.DeleteData(id);
        }

    }
}
