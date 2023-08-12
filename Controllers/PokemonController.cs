using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using simpleapi.Interfaces;
using simpleapi.Model;

namespace simpleapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonController(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemon()
        {
            var pokemons = _pokemonRepository.GetPokemons();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            if(!_pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            var pokemon = _pokemonRepository.GetPokemon(id);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonRepository.PokemonExists(id))
            {
                return NotFound();
            }

            var rating = _pokemonRepository.PokemonExists(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();

            }

            return Ok(rating);
        }

         
    }
}
