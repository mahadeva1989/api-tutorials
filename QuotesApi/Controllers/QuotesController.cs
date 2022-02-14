using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using QuotesApi.Data;
using QuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/{controller}")]
    public class QuotesController : Controller
    {
        private QuotesDbContext _quotesDbContext;
        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }


        [HttpGet]
        public IEnumerable<Quote> Get()
        {
            return _quotesDbContext.Quotes;
        }

        [HttpGet("{id}")]
        public Quote Get(int id)
        {
            return _quotesDbContext.Quotes.Find(id);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetQuotes(int id) {

            var quotes = _quotesDbContext.Quotes;
            return Ok(quotes);
        }


        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();

            return Ok("Saved successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ResponseReturnType response = new ResponseReturnType();
            var quote = _quotesDbContext.Quotes.Find(id);
            if(quote != null)
            {
                _quotesDbContext.Quotes.Remove(quote);
                _quotesDbContext.SaveChanges();
                response.SuccessIndct = true;
                response.Message = "Successfully Delted the record";
                return Ok(response);
            }
            response.Message = "Could not delte, please check the id provided";
            return NotFound(response);
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public IActionResult GetSortQuotes(string sort)
        {
            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(x => x.CreatedDate);
                    break;
                case "dsc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(x => x.CreatedDate);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }

            return Ok(quotes);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetQuotePage(int id, int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            IEnumerable<Quote> quotes = _quotesDbContext.Quotes;
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public IActionResult GetSearchQuote(string type)
        {
            IEnumerable<Quote> quotes = _quotesDbContext.Quotes;
            return Ok(quotes.Where(p => p.Title.Contains(type)));
        }
    }

    public class ResponseReturnType
    {
        public bool SuccessIndct { get; set; }
        public string Message { get; set; }
        public ResponseReturnType()
        {

        }
    }
}
