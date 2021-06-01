using Bootcamp.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Bootcamp
{
    class Program
    {
        static void Main(string[] args)
        {
            var _context = new BootcampContext();
            var scores = from s in _context.Students
                         join asc in _context.AssessmentScores
                         on s.Id equals asc.StudentId
                         join a in _context.Assessments
                         on asc.AssessmentId equals a.Id
                         select new { s.Lastname, asc.ActualScore };

            foreach (var score in scores)
            {
                Console.WriteLine($"{score.Lastname} {score.ActualScore}");
            }
        }
        static void LoadDb()
        {
            var _context = new BootcampContext();
            var student = new Student();
            student.Firstname = "John";
            student.Lastname = "Atkins";
            student.TargetSalary = 200000;
            student.InBootcamp = true;

            _context.Students.Add(student);

            if (_context.SaveChanges() != 1) { throw new Exception("Insert student failed!"); }


            var assessments = new List<Assessment>();

            var git = new Assessment() { MaxPoints = 10, NumberOfQuestions = 6, Topic = "Git/GitHub" };
            var sql = new Assessment() { MaxPoints = 10, NumberOfQuestions = 12, Topic = "SQL Server" };
            var cs = new Assessment() { MaxPoints = 10, NumberOfQuestions = 10, Topic = "CSharp" };
            var js = new Assessment() { MaxPoints = 10, NumberOfQuestions = 8, Topic = "Javascript" };
            var ang = new Assessment() { MaxPoints = 10, NumberOfQuestions = 8, Topic = "Angular" };

            _context.Assessments.AddRange(git, sql, cs, js, ang);

            if (_context.SaveChanges() != 5) { throw new Exception("Insert assessments failed!"); }

            var assessmentscores = new List<AssessmentScore>();

            var gitScore = new AssessmentScore()
            {
                StudentId = student.Id,
                AssessmentId = git.Id,
                ActualScore = 100
            };
            var sqlScore = new AssessmentScore()
            {
                StudentId = student.Id,
                AssessmentId = sql.Id,
                ActualScore = 110
            };

            _context.AssessmentScores.AddRange(gitScore, sqlScore);
            if (_context.SaveChanges() != 2) { throw new Exception("Insert assessment score failed!"); }

        }
    }
}
