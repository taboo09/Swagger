using AutoMapper;
using BookShop.Api.Entities;
using BookShop.Api.Models;
using BookShop.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Api.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorsRepository;
        private readonly IMapper _mapper;

        public AuthorsController(
            IAuthorRepository authorsRepository,
            IMapper mapper)
        {
            _authorsRepository = authorsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of authors
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Author</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authorsFromRepo = await _authorsRepository.GetAuthorsAsync();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        /// <summary>
        /// Get an author by his/her id
        /// </summary>
        /// <param name="authorId">The id of the author you want to get</param>
        /// <returns>An ActionResult of type Author</returns>
        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(
            Guid authorId)
        {
            var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Author>(authorFromRepo));
        }

        [HttpPut("{authorId}")]
        public async Task<ActionResult<Author>> UpdateAuthor(
            Guid authorId,
            AuthorForUpdate authorForUpdate)
        {
            var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(authorForUpdate, authorFromRepo);

            //// update & save
            _authorsRepository.UpdateAuthor(authorFromRepo);
            await _authorsRepository.SaveChangesAsync();

            // return the author
            return Ok(_mapper.Map<Author>(authorFromRepo)); 
        }

        /// <summary>
        /// Partially update an author
        /// </summary>
        /// <param name="authorId">The id of the author you want to get</param>
        /// <param name="patchDocument">The set of operations to apply to the author</param>
        /// <returns>An ActionResult of type Author</returns>
        /// <remarks>Sample request (this request updates the author's **first name**)  
        /// 
        /// PATCH /authors/authorId
        /// [ 
        ///     {
        ///         "op": "replace", 
        ///         "path": "/firstname", 
        ///         "value": "new first name" 
        ///     } 
        /// ] 
        /// </remarks>
        /// <response code="200">Returns the updated author</response>
        [HttpPatch("{authorId}")]
        public async Task<ActionResult<Author>> UpdateAuthor(
            Guid authorId,
            JsonPatchDocument<AuthorForUpdate> patchDocument)
        {
            var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            // map to DTO to apply the patch to
            var author = _mapper.Map<AuthorForUpdate>(authorFromRepo);

            // patchDocument.ApplyTo(author, ModelState);

            // if there are errors when applying the patch the patch doc 
            // was badly formed  These aren't caught via the ApiController
            // validation, so we must manually check the modelstate and
            // potentially return these errors.
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            // map the applied changes on the DTO back into the entity
            _mapper.Map(author, authorFromRepo);

            // update & save
            _authorsRepository.UpdateAuthor(authorFromRepo);
            await _authorsRepository.SaveChangesAsync();

            // return the author
            return Ok(_mapper.Map<Author>(authorFromRepo));
        }
    }
}