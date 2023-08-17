using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfTestDal : EfEntityRepositoryBase<Test, Context>, ITestDal
    {
        //public List<TestCategoryDetailDto> GetTestDetails(int id)
        //{
        //    using (Context context = new Context())
        //    {
        //        var result = from testCategory in context.TestCategories
        //                     join test in context.Tests
        //                     on testCategory.TestCategoryID equals test.TestCategoryID
        //                     join question in context.Questions
        //                     on test.TestID equals question.TestID into questionsGroup
        //                     join image in context.Images
        //                     on test.TestID equals image.TestID into imagesGroup


        //                     select new TestCategoryDetailDto
        //                     {
        //                         TestCategoryID = testCategory.TestCategoryID,
        //                         TestCategoryName = testCategory.TestCategoryName,
        //                         TestID = test.TestID,
        //                         Title = test.Title,
        //                         ImageID = imagesGroup.Select(s => (int)s.ImageID).FirstOrDefault(),
        //                         ImageUrl = imagesGroup.Select(s => s.ImageUrl).FirstOrDefault(),
        //                         Questions = questionsGroup.ToList()

        //                     };

        //        return result.ToList();

        //    }
        //}

        public List<TestCategoryDetailDto> GetTestDetails(int id)
        {
            using (Context context = new Context())
            {
                var testCategories = context.TestCategories.ToList();
                var tests = context.Tests.Where(t => t.TestCategoryID == id).ToList();
                var questions = context.Questions.Where(q => tests.Any(t => t.TestID == q.TestID)).ToList();
                var images = context.Images.Where(i => tests.Any(t => t.TestID == i.TestID)).ToList();

                var result = from testCategory in testCategories
                             join test in tests
                             on testCategory.TestCategoryID equals test.TestCategoryID
                             join question in questions
                             on test.TestID equals question.TestID into questionsGroup
                             join image in images
                             on test.TestID equals image.TestID into imagesGroup
                             select new TestCategoryDetailDto
                             {
                                 TestCategoryID = testCategory.TestCategoryID,
                                 TestCategoryName = testCategory.TestCategoryName,
                                 TestID = test.TestID,
                                 Title = test.Title,
                                 ImageID = imagesGroup.Select(s => (int?)s.ImageID).FirstOrDefault(),
                                 ImageUrl = imagesGroup.Select(s => s.ImageUrl).FirstOrDefault(),
                                 Questions = questionsGroup.ToList()
                             };

                return result.ToList();
            }
        }


        public TestQuestionsListDto GetTestQuestions(int id)
        {
            using (var context = new Context())
            {

                var test =  context.Tests
           .Include(t => t.TestCategory)
           .Include(t => t.Questions).ThenInclude(q => q.QuestionOptions)
           .Include(t => t.Images)
           .FirstOrDefault(t => t.TestID == id);

                var questions = context.Questions
                    .Include(q => q.Images).ThenInclude(x=> x.Question)
                    .Include(x=> x.QuestionOptions).ThenInclude(x => x.Question)
                    .Include(x=> x.TestAnswer).ThenInclude(x => x.Question)
                    .Include(x=> x.Test).ThenInclude(x => x.Questions)
                    .Where(q => q.TestID == id)
                    .ToList();

                var options = context.QuestionOptions.Include(x=>x.Question).ThenInclude(x=> x.QuestionOptions).ToList();

                var questionImages = new Dictionary<int, List<Image>>();
                foreach (var question in questions)
                {
                    questionImages[question.QuestionID] = question.Images.ToList();
                }

                var questionOptions = new Dictionary<int, List<QuestionOption>>();
                //foreach (var option in options)
                //{
                //    questionOptions[option.QuestionID] = options.Where(x=>x.QuestionID == option.QuestionID).ToList();
                //}

                foreach (var question in questions)
                {
                    questionOptions[question.QuestionID] = options.Where(option => option.QuestionID == question.QuestionID).ToList();
                }

              



                var result = new TestQuestionsListDto
                {
                    Test = test,
                    Questions = questions,
                    QuestionImages = questionImages,
                    QuestionOptions = questionOptions
                };

                return result;

            }
        }

        public List<Test> GetTestsByTestCategoryId(int id)
        {
            using (Context context = new Context())
            {
                var result = context.Tests.Include(x => x.TestCategory).Where(x => x.TestCategoryID == id).ToList();
                return result;
            }
        }
    }
}
