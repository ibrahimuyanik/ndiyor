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
    public class EfQuestionDal : EfEntityRepositoryBase<Question, Context>, IQuestionDal
    {
        public QuesitonDetailDto GetQuestionDetailById(int id)
        {
            using (Context context = new Context())
            {
                var result = context.Questions.First(x=>x.TestID== id);
                return new QuesitonDetailDto
                {
                     Content= result.Content,
                      TestID=result.TestID,
                       Images= result.Images,
                        QuestionID=result.QuestionID,
                         QuestionOptions=result.QuestionOptions,
                          Test = result.Test,
                           TestAnswer =result.TestAnswer,
                            TestCategory=result.TestCategory,
                             TestCategoryID=result.TestCategoryID,
                              TrueAnswer=result.TrueAnswer,
                };

            }
        }
    }
}
