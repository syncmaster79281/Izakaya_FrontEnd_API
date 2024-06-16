using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnairesController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public QuestionnairesController(IzakayaContext context)
        {
            _context = context;
        }
        [HttpGet("{productId}")]
        public IActionResult GetScore(int productId)
        {
            var satisfactionLevels = _context.Questionnaires
               .Where(q => q.ProductId == productId)
               .Select(q => q.SatisfactionLevel)
               .ToList();

            if (satisfactionLevels.Any())
            {
                int totalScore = 0;
                foreach (var level in satisfactionLevels)
                {
                    totalScore += int.Parse(level);
                }
                double averageScore = satisfactionLevels.Average(level => int.Parse(level));
                return Ok(averageScore);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("top5")]
        public IActionResult GetTop5Rating()
        {
            var questionnaires = _context.Questionnaires.AsNoTracking().Include(q => q.Product)
                .Select(q => new QuestionnairesInfo
                {
                    Id = q.Id,
                    ProductId = q.ProductId,
                    SatisfactionLevel = q.SatisfactionLevel,
                    FavoriteDish = q.FavoriteDish,
                    Date = q.Date,
                    OrderId = q.OrderId,
                    Name = q.Product.Name,
                    ImageUrl = q.Product.ImageUrl
                }).ToList();


            var productScores = questionnaires
                .GroupBy(q => q.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key, // 提取产品ID
                    AverageScore = group.Average(q =>
                    {
                        int score;
                        int.TryParse(q.SatisfactionLevel, out score); // 尝试解析满意度分数
                        return score;
                    }),
                    Name = group.First().Name, // 获取产品名称
                    ImageUrl = group.First().ImageUrl // 获取产品图像URL
                })
                .OrderByDescending(result => result.AverageScore)
                .Take(5) // 取前五名
                .ToList(); // 转换为列表
            if (productScores.Any())
            {
                return Ok(productScores);
            }

            else
            {
                return NotFound();
            }
        }

        // POST api/<QuestionnairesController>
        [HttpPost("submitForm")]
        public IActionResult SubmitForm([FromBody] FormData formData)
        {
            try
            {
                //使用轉換後的數據創建後端模型對象
                var model = new FormData
                {
                    SelectedSatisfaction = formData.SelectedSatisfaction,
                    FavoriteDish = formData.FavoriteDish,
                    OrderId = formData.OrderId
                };

                // 儲存資料庫
                foreach (var q in formData.SelectedSatisfaction)
                {
                    var questionnaire = new Questionnaire
                    {
                        ProductId = q.ProductId,
                        SatisfactionLevel = q.SatisfactionLevel,
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        FavoriteDish = formData.FavoriteDish,
                        OrderId = formData.OrderId
                    };

                    _context.Add(questionnaire);
                }
                _context.SaveChanges();
                return Ok("成功");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"An error occurred while saving the entity changes. Inner exception: {ex.InnerException.Message}");
                }
                else
                {
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
        }

        public class FormData
        {
            public List<Satisfaction> SelectedSatisfaction { get; set; }
            public string? FavoriteDish { get; set; }
            public int OrderId { get; set; }
        }

        public class Satisfaction
        {
            public string DishType { get; set; }
            public int ProductId { get; set; }
            public string SatisfactionLevel { get; set; }
        }

        public class QuestionnairesInfo
        {
            public int Id { get; set; }

            public int ProductId { get; set; }

            public string SatisfactionLevel { get; set; }

            public string FavoriteDish { get; set; }

            public DateOnly Date { get; set; }

            public int OrderId { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}






