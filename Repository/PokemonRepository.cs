using simpleapi.Data;
using simpleapi.Interfaces;
using simpleapi.Model;

namespace simpleapi.Repository
{
    /// <summary>
    /// Repository pattern
    /// is where we do all the Database calls
    /// </summary>
    public class PokemonRepository : IPokemonRepository
    {
        private DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;

        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int id)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == id);

            if(review.Count() <= 0)
            {
                return 0;
            }

            // converstation data to decimal 
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Id).ToList() ;
        }

        public bool PokemonExists(int id)
        {
            return _context.Pokemon.Any(p => p.Id == id);
        }
    }
}
