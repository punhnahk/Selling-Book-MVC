using ASM1670.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASM1670.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            //Anh em vui lòng đọc hướng dẫn tạo account Admin
            //create a new account
            //sau đó vào SQL tạo querry mới rồi gõ DELETE FORM UserRoles where User = '..' sau đó chạy
            //tiếp theo gõ INSERT INTO UserRoles Values('cái id của user','cái id của role Admin') sau đó chạy 
            //rồi logout rồi login lại là xong
        }
    }
}
