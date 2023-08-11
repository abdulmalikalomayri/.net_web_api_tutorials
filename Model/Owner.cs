namespace simpleapi.Model
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        // if we write Class name here and write a class name coutnry it will be one to one relationship
        // One to many
        public Country Country { get; set; }
        // many to many
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
