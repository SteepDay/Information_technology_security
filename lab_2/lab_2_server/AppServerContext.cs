using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace lab_2_server
{
    /// <summary>
    /// Контекст базы данных приложения сервера
    /// </summary>
    public class AppServerContext : DbContext
    {
        // Определение наборов данных
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Right> Rights { get; set; } = null!;
        public DbSet<Data> Datas { get; set; } = null!;

        public AppServerContext(DbContextOptions<AppServerContext> options)
            : base(options)
        {
            // Создание или обновление базы данных
            Database.EnsureCreated();
        }

        /// <summary>
        /// Настройка модели данных в базе
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Определение связей между таблицами User и Right через промежуточную таблицу UserRight
            modelBuilder
                .Entity<User>()
                .HasMany(c => c.Rights)
                .WithMany(s => s.Users)
                .UsingEntity<UserRight>(
                   j => j
                    .HasOne(pt => pt.Right)
                    .WithMany(t => t.UserRights)
                    .HasForeignKey(pt => pt.RightId),
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(p => p.UserRights)
                    .HasForeignKey(pt => pt.UserId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.RightId });
                    j.ToTable("UserRight");
                });

            // Начальные данные
            Right rightRead = new Right { Id = 1, TypeRight = "READ" };
            Right rightWrite = new Right { Id = 2, TypeRight = "WRITE" };

            var passwordHash = SHA1.HashData(Encoding.UTF8.GetBytes("admin"));
            User admin = new User { Id = 1, Login = "admin", Password = Encoding.UTF8.GetString(passwordHash) };

            // Заполнение таблиц начальными данными
            modelBuilder.Entity<Right>().HasData(rightRead, rightWrite);
            modelBuilder.Entity<User>().HasData(admin);

            UserRight first = new UserRight { UserId = admin.Id, RightId = rightRead.Id };
            UserRight second = new UserRight { UserId = admin.Id, RightId = rightWrite.Id };
            modelBuilder.Entity<UserRight>().HasData(first, second);

            modelBuilder.Entity<Data>().HasData(new Data { Id = 1, Value = "something" });
        }
    }
}
