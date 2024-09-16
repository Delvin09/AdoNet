using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace AdoNet
{
    /*
Commonly Used Types:
    Microsoft.Data.SqlClient.SqlConnection
    Microsoft.Data.SqlClient.SqlException
    Microsoft.Data.SqlClient.SqlParameter
    Microsoft.Data.SqlClient.SqlDataReader
    Microsoft.Data.SqlClient.SqlCommand
    Microsoft.Data.SqlClient.SqlTransaction
    Microsoft.Data.SqlClient.SqlParameterCollection
    Microsoft.Data.SqlClient.SqlClientFactory
     */

    internal class Program
    {
        static async Task Main(string[] args)
        {
            //ORM

            using var connection = new SqlConnection("Data Source=DESKTOP-Q2BKQOF\\SQLEXPRESS;Initial Catalog=Univer;Integrated Security=True;Encrypt=False");
            await connection.OpenAsync();

            var tran = await connection.BeginTransactionAsync();
            try
            {
                await InsertLesson(connection, tran);

                await SelectLessons(connection, tran, 2);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.Flush();
            }
            finally
            {
                await tran.RollbackAsync();
            }
        }

        private static async Task InsertLesson(SqlConnection connection, DbTransaction tran)
        {
            var cmd = connection.CreateCommand();
            cmd.Transaction = (SqlTransaction)tran;
            cmd.CommandText = @"insert into Lessons ([Name], TeacherId, FacId) VALUES ('History', NULL, NULL);";
            var r = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine($"result = {r}");
        }

        private static async Task SelectLessons(SqlConnection connection, DbTransaction tran, int findTeacherById)
        {
            var cmd = connection.CreateCommand();

            cmd.Transaction = (SqlTransaction)tran;

            cmd.CommandText = @$"select t.Id, t.FirstName, t.LastName, t.Passport, t.Age,
	l.Id as LessonId, l.Name, l.TeacherId, l.FacId
from Teachers t
	full join Lessons l ON l.TeacherId = t.Id or l.TeacherId is null
WHERE t.Id = @{nameof(findTeacherById)}";

            var p = cmd.Parameters.Add(nameof(findTeacherById), System.Data.SqlDbType.Int);
            p.Value = findTeacherById;

            using var reader = await cmd.ExecuteReaderAsync();

            var teachers = new HashSet<Teacher>();
            var lessons = new List<Lesson>();

            while (reader.Read())
            {
                var teacher = new Teacher
                {
                    Id = reader.GetFieldValue<int>(reader.GetOrdinal("Id")),
                    FullName = reader.GetString(reader.GetOrdinal("FirstName"))
                        + " " + reader.GetString(reader.GetOrdinal("LastName")),
                    Passport = reader.GetString(reader.GetOrdinal("Passport")),
                    Age = reader.GetByte(reader.GetOrdinal("Age"))
                };

                teachers.Add(teacher);
                // -------
                var teacherIdColumn = reader.GetOrdinal("TeacherId");
                var facIdColumn = reader.GetOrdinal("FacId");

                var teacherId = reader.IsDBNull(teacherIdColumn) ? null : reader.GetFieldValue<int?>(teacherIdColumn);

                if (teacherId.HasValue)
                    teachers.TryGetValue(new Teacher { Id = teacherId.Value }, out teacher);
                else
                    teacher = null;

                var lesson = new Lesson
                {
                    Id = reader.GetFieldValue<int>(reader.GetOrdinal("LessonId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Teacher = teacher,
                    Fac = reader.IsDBNull(facIdColumn) ? null : reader.GetFieldValue<int?>(facIdColumn),
                };

                teacher?.Lessons.Add(lesson);
                lessons.Add(lesson);
            }

            lessons.ForEach(l =>
            {
                Console.WriteLine(l);
            });

            foreach (var t in teachers)
                Console.WriteLine(t);
        }

        class Teacher
        {
            public int Id { get; set; }

            public string FullName { get; set; } = string.Empty;

            public string Passport { get; set; } = string.Empty;

            public int Age { get; set; }

            public List<Lesson> Lessons { get; set; } = new List<Lesson>();

            public override string ToString()
            {
                return $"{FullName}";
            }

            public override bool Equals(object? obj)
            {
                if (obj == null || obj.GetType() != GetType()) return false;
                if (ReferenceEquals(this, obj)) return true;
                return ((Teacher)obj).Id == Id;
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }
        }

        class Lesson
        {
            public int Id { get; set; }

            public string Name { get ; set; } = string.Empty;

            public Teacher? Teacher { get; set; }

            public int? Fac { get; set; }

            public override string ToString()
            {
                return $"{Id}\t{Name}\t{Teacher?.FullName}\t{Fac}";
            }
        }
    }
}
