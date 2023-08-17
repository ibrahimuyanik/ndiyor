using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TestManager : ITestService
    {
        private readonly ITestDal _testDal;

        public TestManager(ITestDal testDal)
        {
            _testDal = testDal;
        }

        public IResult Add(Test entity)
        {
            try
            {
                _testDal.Add(entity);
                return new SuccessResult(Messages.TestAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestInvalid + " " + ex);
            }
        }

        public IResult Delete(Test entity)
        {
            try
            {
                _testDal.Delete(entity);
                return new SuccessResult(Messages.TestDelete);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestNotDelete + " " + ex);
            }
        }

        public IDataResult<List<Test>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Test>>(_testDal.GetAll(), Messages.TestGetAll);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Test>>(Messages.TestNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<Test> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Test>(_testDal.Get(x => x.TestID == id), id + Messages.TestGetById);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Test>(id + Messages.TestNotGetById + " " + ex.Message);
            }
        }

        public IDataResult<List<TestCategoryDetailDto>> GetTestDetails(int id)
        {
            try
            {
                return new SuccessDataResult<List<TestCategoryDetailDto>>(_testDal.GetTestDetails(id), "Test Listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TestCategoryDetailDto>>(Messages.TestCategoryNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<TestQuestionsListDto> GetTestQuestions(int id)
        {
            return new SuccessDataResult<TestQuestionsListDto>(_testDal.GetTestQuestions(id), "Deneme");
        }

        public IDataResult<List<Test>> GetTestsByTestCategoryId(int id)
        {
            try
            {
                return new SuccessDataResult<List<Test>>(_testDal.GetTestsByTestCategoryId(id), "Test Listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Test>>(Messages.TestCategoryNotGetAll + " " + ex.Message);
            }
        }

        public IResult Update(Test entity)
        {
            try
            {
                _testDal.Update(entity);
                return new SuccessResult(Messages.TestUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestNotUpdate + " " + ex.Message);
            }
        }

    }
}
