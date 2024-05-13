using API.DTOs;
using API.Models;
using API.Reposatories;
using AutoMapper;
using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReposatory<Article> _reposatory;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UserManager<IdentityUser> _userManager;

        public ArticlesController(IMapper mapper, IReposatory<Article> reposatory, ISieveProcessor sieveProcessor, UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _reposatory = reposatory;
            _sieveProcessor = sieveProcessor;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var usr = await GetCurrentUserAsync();

            List<Article> articles = await _reposatory.GetAll();

            // Get only published articles and return them as Queryable to use in SieveProcessor
            IQueryable<Article>? publishedArticles;

            if (usr == null)
            {
                publishedArticles = articles.Where(art => art.PublicationDate <= DateOnly.FromDateTime(DateTime.Now)).AsQueryable();
            }
            else
            {
                publishedArticles = articles.AsQueryable();
            }

            var result = _sieveProcessor.Apply(sieveModel, publishedArticles);

            List<ArticleOutputDTO> articleOutputs = [];

            result.ToList().ForEach(article => articleOutputs.Add(_mapper.Map<ArticleOutputDTO>(article)));

            return Ok(articleOutputs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            Article? article = await _reposatory.GetById(id);

            if (article == null)
            {
                return NotFound();
            }

            ArticleOutputDTO articleOutput = _mapper.Map<ArticleOutputDTO>(article);

            return Ok(articleOutput);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticlePostDTO articleInput)
        {
            Article article = _mapper.Map<Article>(articleInput);

            if (!IsValidPublicationData(article))
            {
                return BadRequest();
            }

            await _reposatory.Create(article);
            await _reposatory.Save();

            return CreatedAtAction(nameof(GetById), new { id = article.Id }, _mapper.Map<ArticleOutputDTO>(article));
        }

        [HttpPut]
        public async Task<IActionResult> Update(string articleId, ArticlePutDTO articleInput)
        {
            Article article = _mapper.Map<Article>(articleInput);

            if (articleId != article.Id)
            {
                return BadRequest();
            }

            if (!IsValidPublicationData(article))
            {
                return BadRequest();
            }

            _reposatory.Update(article);
            await _reposatory.Save();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Article? article = await _reposatory.GetById(id);

            if (article == null)
            {
                return BadRequest();
            }

            _reposatory.Delete(article);
            await _reposatory.Save();

            return NoContent();
        }

        [HttpGet("{id}/Summarize")]
        [AllowAnonymous]
        public async Task<IActionResult> Summarize(string id)
        {
            var bot = new ChatGpt("pk-oEPsMsKGASHQsXfqvziKIvjYuSwSSIxlmFzfrsXoTVkZClPR", new ChatGptOptions
            {
                BaseUrl = "https://api.pawan.krd",
                Model = "pai-001"
            });

            var article = await _reposatory.GetById(id);
            if (article == null)
            {
                return BadRequest();
            }

            var response = await bot.Ask("Summarize the following into bullets and make sure to return only completed bullets, make your response super short (only most important 3) and simple, return it as html code:\n" + article.Content);

            return Ok(new { Summary = response.Trim() });
        }

        private bool IsValidPublicationData(Article article)
        {
            if (article.PublicationDate > article.CreationDate.AddDays(7) || article.PublicationDate < article.CreationDate)
            {
                return false;
            }

            return true;
        }

        private async Task<IdentityUser?> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
