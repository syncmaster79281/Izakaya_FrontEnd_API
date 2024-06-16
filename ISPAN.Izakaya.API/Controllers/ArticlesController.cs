using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public ArticlesController(IzakayaContext context)
        {
            _context = context;
        }

        // GET: api/<ArticlesController>
        [HttpGet]
        public IEnumerable<Article> Get()
        {

            var articles = _context.Articles
                 .OrderByDescending(a => a.PublishDate)
                  //.Where(a => a.Status == true)
                  .Select(a => new Article { Title = a.Title, Contents = a.Contents, PublishDate = a.PublishDate, HideTime = a.HideTime, ImageUrl = a.ImageUrl, Status = a.Status })
                 .ToList();

            foreach (var article in articles)
            {
                if (article.HideTime == null)
                {
                    article.Status = true;
                }

                else if (article.HideTime != null && article.HideTime < DateTime.Now)
                {
                    article.Status = false;
                }
            }
            return articles;
        }

        // GET api/<ArticlesController>/5
        [HttpGet("{id}")]
        public IEnumerable<Article> Get(int id)
        {
            var articles = _context.Articles
                .Where(a => a.Id == id)
                .Select(a => new Article { Title = a.Title, Contents = a.Contents, PublishDate = a.PublishDate, ImageUrl = a.ImageUrl })
                .FirstOrDefault();

            if (articles == null)
            {
                return Enumerable.Empty<Article>();
            }

            return new List<Article> { articles };

        }


        // POST api/<ArticlesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ArticlesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ArticlesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
