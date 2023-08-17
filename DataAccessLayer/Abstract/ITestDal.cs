using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ITestDal : IEntityRepository<Test>
    {
        List<TestCategoryDetailDto> GetTestDetails(int id);
        List<Test> GetTestsByTestCategoryId(int id);
        TestQuestionsListDto GetTestQuestions(int id);

    }
}
