namespace moviles.Backend.Models
{
    using Domain;
    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<moviles.Domain.User> Users { get; set; }
    }
}