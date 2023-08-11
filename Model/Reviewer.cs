namespace simpleapi.Model
{
    public class Reviewer
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // We can use List instead of ICollection. a List has more feature than ICollection
        // one to many
        public ICollection<Review> Reviews { get; set; }
    }
}
