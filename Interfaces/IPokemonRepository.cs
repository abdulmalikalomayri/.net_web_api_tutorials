using simpleapi.Model;

namespace simpleapi.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();

        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokedId);
        bool PokemonExists(int pokeId);
    }
}
