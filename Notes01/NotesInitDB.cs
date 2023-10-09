
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes01
{
     public class NotesInitDB: DbContext
     {
        public DbSet<NotesEntity> Notes { get; set; } = null!; //Определение свойства Notes, которое представляет собой набор сущностей NotesEntity

        public NotesInitDB() // Создание базы данных, если она еще не существует.                             
        {                   // Этот метод проверяет, существует ли база данных, и если нет, то создает ее.
            Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {                                                                               //Метод настраивает параметры подключения к базе данных, используя провайдер PostgreSQL.
                                                                                        //В данном случае, параметры подключения указываются явно в строке подключения
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Notes;Username=postgres;Password=stud");
        }
     }
}
