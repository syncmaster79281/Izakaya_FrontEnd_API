using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqsController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public FaqsController(IzakayaContext context)
        {
            _context = context;
        }


        // GET: api/<FaqsController>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _context.Faqs
                .Select(f => f.Keyword)
                .ToListAsync();
        }

        // GET api/<FaqsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var faq = _context.Faqs.Find(id);
            return faq.Question;

        }

        // POST api/<FaqsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FaqsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FaqsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
