using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TO_DO_List.Models;

namespace TO_DO_List.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TO_DO_List.Models.TaskToDo> TaskToDo { get; set; } = default!;
        public DbSet<TO_DO_List.Models.TaskRepetitiveAcions> TaskRepetitives{ get; set; } = default!;
        public DbSet<TO_DO_List.Models.Category> Categories{ get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Обов'язково викликаємо базовий метод для коректної роботи Identity (користувачів/ролей)
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<TO_DO_List.Models.TaskVeiwModel> TaskVeiwModel { get; set; } = default!;

    }
}