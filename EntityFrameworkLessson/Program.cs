using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EntityFrameworkLessson
{
    public class Teacher : Person
    {
    }

    public class Student : Person
    {
        public virtual Facultet? Facultet { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        public int Year { get; set; }
    }

    public abstract class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Passport { get; set; } = string.Empty;

        public int Age { get; set; }
    }

    public class Lesson
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        //public int? TeacherId { get; set; }

        public virtual Teacher? Teacher { get; set; } // TeacherId int FK to Teacher table

        public int? FacId { get; set; }

        public virtual Facultet? Fac { get; set; }
    }

    public class Facultet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Student> Students  { get; set; }

        public virtual List<Lesson> Lessons { get; set; }
    }

    public class Mark
    {
        public int Id { get; set; }
        public virtual Student Student { get; set; }
        public virtual Lesson Lesson { get; set; }

        public int ExcamMark { get; set; }
    }

    public class UniverContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Facultet> Facultets { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public UniverContext() 
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data Source=DESKTOP-Q2BKQOF\\SQLEXPRESS;Initial Catalog=Univer_2;Integrated Security=True;Encrypt=False");
            optionsBuilder.LogTo(Console.WriteLine);

            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable(tableBuilder => tableBuilder.HasCheckConstraint("check_age", "Age BETWEEN 16 AND 70"));

            modelBuilder.Entity<Facultet>().HasData(
                [
                    new Facultet { Id = 1, Name = "Tech fac" },
                    new Facultet { Id = 2, Name = "Social fac" }
                ]);

            base.OnModelCreating(modelBuilder);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            //InitData();

            using var ctx = new UniverContext();

            //var st = ctx.Students.Where(s => s.Facultet != null && s.Facultet.Id == 1).ToList();

            var allLessons = ctx.Lessons.Where(l => l.Fac != null).ToArray();

            //var r = ctx.Marks.ToArray();

            foreach(var lesson in allLessons)
            {
                var facultet = lesson.Fac;
                foreach (var s in facultet.Students)
                {
                    Console.WriteLine(s.FirstName);
                }
            }

            //var fac = ctx.Facultets
            //   // .Include(f => f.Students)
            //   //    .ThenInclude(s => s.Lessons)
            //    .Where(f => f.Id == 1)
            //    .First(f => f.Id == 1);

            //var facultets = ctx.Facultets.ToArray();
            //var students = ctx.Students.Where(s => s.Facultet == null).ToArray();
            //var counter = 0;
            //foreach (var item in students)
            //{
            //    var index = (counter < students.Count() / 2 ? 0 : 1);
            //    item.Facultet = facultets[index];
            //    counter++;
            //}

            ctx.SaveChanges();
        }

        private static void InitData()
        {
            using var ctx = new UniverContext();

            var teachers = new List<Teacher>
            {
                new Teacher { FirstName = "Oleg", LastName = "Smith", Age = 33, Passport = "MMDD444" },
                new Teacher { FirstName = "Sam", LastName = "Smith", Age = 21, Passport = "DFFDSDF" },
                new Teacher { FirstName = "Bill", LastName = "Smith", Age = 47, Passport = "IUOUKJLK" },
                new Teacher { FirstName = "Vika", LastName = "Smith", Age = 22, Passport = "EQWEEWQ" },
                new Teacher { FirstName = "Frank", LastName = "Smith", Age = 66, Passport = "LKLJKLJ" },

                new Teacher { FirstName = "Jonh", LastName = "Doe", Age = 56, Passport = "ERWIRUWE" },
            };

            ctx.Teachers.AddRange(teachers);

            var lessons = new List<Lesson>
            {
                new Lesson { Teacher = teachers[0], Name = "Math", FacId = 1 },
                new Lesson { Teacher = teachers[0], Name = "Physics", FacId = 1 },
                new Lesson { Teacher = teachers[0], Name = "Biology", FacId = 1 },

                new Lesson { Teacher = teachers[1], Name = "Art design", FacId = 2 },
                new Lesson { Teacher = teachers[2], Name = "Philosophy", FacId = 2 },
                new Lesson { Teacher = teachers[3], Name = "Mechanics", FacId = 1 },

                new Lesson { Teacher = teachers[4], Name = "Sociology", FacId = 2 },
                new Lesson { Teacher = teachers[4], Name = "Geometry", FacId = 1 },
            };

            ctx.Lessons.AddRange(lessons);

            ctx.Lessons.Add(new Lesson { Name = "Fizra" });
            ctx.Lessons.Add(new Lesson { Name = "History" });

            var students = new List<Student>()
            {
                new Student { FirstName = "1", LastName = "1", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[0], lessons[1] } },
                new Student { FirstName = "2", LastName = "2", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[1], lessons[2] } },
                new Student { FirstName = "3", LastName = "3", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[2], lessons[0] } },
                new Student { FirstName = "4", LastName = "4", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[1], lessons[3] } },
                new Student { FirstName = "5", LastName = "5", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[1], lessons[4] } },
                new Student { FirstName = "6", LastName = "6", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[4], lessons[1] } },
                new Student { FirstName = "7", LastName = "7", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[5], lessons[6] } },
                new Student { FirstName = "8", LastName = "8", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[5], lessons[7] } },
                new Student { FirstName = "9", LastName = "9", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[7], lessons[6] } },
                new Student { FirstName = "10", LastName = "10", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[6], lessons[3] } },
                new Student { FirstName = "11", LastName = "11", Year = 1, Passport = "", Age = 18, Lessons = new List<Lesson> { lessons[6], lessons[2], lessons[0] } },
            };

            ctx.Students.AddRange(students);

            ctx.SaveChanges();
        }
    }
}
