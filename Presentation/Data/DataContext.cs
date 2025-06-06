using Microsoft.EntityFrameworkCore;

namespace Presentation.Data {

    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        //Add-Migration "" Update-Database
        public DbSet<ConfirmationEntity> Confirmations { get; set; }
    }
}
