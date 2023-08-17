using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ITestService : IGenericService<Test>
    {
        IDataResult<List<TestCategoryDetailDto>> GetTestDetails(int id);
        IDataResult<List<Test>> GetTestsByTestCategoryId(int id);
        IDataResult<TestQuestionsListDto> GetTestQuestions(int id);
    }
}
