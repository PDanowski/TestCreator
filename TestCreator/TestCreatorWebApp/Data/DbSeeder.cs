﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestCreatorWebApp.Data.Models;

namespace TestCreatorWebApp.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                CreateUsers(context);
            }

            if (!context.Tests.Any())
            {
                CreateTests(context);
            }
        }

        private static void CreateTests(ApplicationDbContext context)
        {
            DateTime creationDate = new DateTime(2018, 03, 01, 12, 00, 00);
            DateTime modificationDate = DateTime.Now;

            var authorId = context.Users.FirstOrDefault(u => u.UserName == "Admin")?.Id;

            var num = 27;

            for (int i = 1; i <= num; i++)
            {
                CreateSampleTest(context, i, authorId, num - 1, 3, 3, 3, creationDate.AddDays(-num));
            }

            EntityEntry<Test> e1 = context.Tests.Add(new Test
            {
                UserId = authorId,
                CreationDate = creationDate,
                LastModificationDate = modificationDate,
                Title = "Generation X, Y or Z?",
                Description = "Get know, to which generation fit you the most.",
                Text = "Do you feel good in your generation ? " + 
                       "In which year should you be born ? " +
                       "Answer few questions and let's find out!",
                ViewCount = 1280
            });

            EntityEntry<Test> e2 = context.Tests.Add(new Test
            {
                UserId = authorId,
                CreationDate = creationDate,
                LastModificationDate = modificationDate,
                Title = "Which football player are you?",
                Description = "Get know which football player is similar to you.",
                Text = "Do you think, that you are similar to Leo Messi ? " +
                       "Maybe do you think that you have similar character as Cristiano Ronaldo ? " +
                       "Answer few questions and let's find out!",
                ViewCount = 4510
            });

            EntityEntry<Test> e3 = context.Tests.Add(new Test
            {
                UserId = authorId,
                CreationDate = creationDate,
                LastModificationDate = modificationDate,
                Title = "History master",
                Description = "Check yourself in short history test",
                Text = "",
                ViewCount = 110
            });

            context.SaveChanges();
        }

        private static void CreateSampleTest(ApplicationDbContext context, 
            int num, 
            string authorId, 
            int viewCount, 
            int numberOfQuestions, 
            int numberOfAnswersPerQuestions, 
            int numberOfResults, 
            DateTime createdDate)
        {
            var test = new Test
            {
                UserId = authorId,
                Title = $"Title of the test {num}",
                Description = $"This is sample test {num}",
                Text = $"This is sample test created using DbSeeder class. All questions, answers and results were generated " +
                       $"using this class as well.",
                ViewCount = viewCount,
                CreationDate = createdDate,
                LastModificationDate = createdDate
            };

            context.Tests.Add(test);
            context.SaveChanges();

            for (int i = 0; i < numberOfQuestions; i++)
            {
                var question = new Question
                {
                    TestId = test.Id,
                    Text = $"Sample qeustion {i+1}",
                    CreationDate = createdDate,
                    LastModificationDate = createdDate
                };

                context.Questions.Add(question);
                context.SaveChanges();

                for (int j = 0; j < numberOfAnswersPerQuestions; j++)
                {
                    var a1 = context.Answers.Add(new Answer
                    {
                        QuestionId = question.Id,
                        Text = $"Sample answer {j + 1}",
                        Value = j,
                        CreationDate = createdDate,
                        LastModificationDate = createdDate
                    });
                }
            }

            for (int i = 0; i < numberOfResults; i++)
            {
                context.Results.Add(new Result
                {
                    TestId = test.Id,
                    Text = $"Sample result {i + 1}",
                    MinValue = 0,
                    MaxValue = numberOfQuestions + 2,
                    CreationDate = createdDate,
                    LastModificationDate = createdDate
                });
            }

            context.SaveChanges();
        }

        private static void CreateUsers(ApplicationDbContext context)
        {
            DateTime creationDate = new DateTime(2018, 03, 01, 12, 00, 00);
            DateTime modificationDate = DateTime.Now;

            var userAdmin = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@testcreator.com",
                CreationDate = creationDate,
                LastModificationDate = modificationDate
            };

            context.Users.Add(userAdmin);

            #if DEBUG
            var normalUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User1",
                Email = "user1@testcreator.com",
                CreationDate = creationDate,
                LastModificationDate = modificationDate
            };

            context.Users.Add(normalUser);
            #endif

            context.SaveChanges();
        }
    }
}