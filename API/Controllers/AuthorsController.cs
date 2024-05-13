using API.DTOs;
using API.Models;
using API.Reposatories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReposatory<Author> _reposatory;
        private readonly ISieveProcessor _sieveProcessor;

        public AuthorsController(IMapper mapper, IReposatory<Author> reposatory, ISieveProcessor sieveProcessor)
        {
            _mapper = mapper;
            _reposatory = reposatory;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            List<Author> authors = await _reposatory.GetAll();

            // Return authors as Queryable to use in SieveProcessor
            var authorsAsQueryable = authors.AsQueryable();

            var result = _sieveProcessor.Apply(sieveModel, authorsAsQueryable);

            List<AuthorOutputDTO> authorOutputs = [];

            result.ToList().ForEach(author => authorOutputs.Add(_mapper.Map<AuthorOutputDTO>(author)));

            return Ok(authorOutputs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            Author? author = await _reposatory.GetById(id);

            if (author == null)
            {
                return NotFound();
            }

            AuthorOutputDTO authorOutput = _mapper.Map<AuthorOutputDTO>(author);

            return Ok(authorOutput);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorPostDTO authorInput)
        {
            Author author = _mapper.Map<Author>(authorInput);

            await _reposatory.Create(author);
            await _reposatory.Save();

            return CreatedAtAction(nameof(GetById), new { id = author.Id }, _mapper.Map<AuthorOutputDTO>(author));
        }

        [HttpPut]
        public async Task<IActionResult> Update(string authorId, AuthorPutDTO authorInput)
        {
            Author author = _mapper.Map<Author>(authorInput);

            if (authorId != author.Id)
            {
                return BadRequest();
            }

            _reposatory.Update(author);
            await _reposatory.Save();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Author? author = await _reposatory.GetById(id);

            if (author == null)
            {
                return BadRequest();
            }

            _reposatory.Delete(author);
            await _reposatory.Save();

            return NoContent();
        }
    }
}
