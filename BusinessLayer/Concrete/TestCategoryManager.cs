using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Entities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TestCategoryManager : ITestCategoryService
    {
        private readonly ITestCategoryDal _testCategoryDal;

        public TestCategoryManager(ITestCategoryDal testCategoryDal)
        {
            _testCategoryDal = testCategoryDal;
        }

        public IResult Add(TestCategory entity)
        {
            try
            {
                _testCategoryDal.Add(entity);
                return new SuccessResult(Messages.TestCategoryAdded);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.TestCategoryInvalid + " " + ex.Message);
            }
        }

        public IResult Delete(TestCategory entity)
        {
            try
            {
                _testCategoryDal.Delete(entity);
                return new SuccessResult(Messages.TestCategoryDelete);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestCategoryNotDelete + " " + ex.Message);
            }
        }

        public IDataResult<List<TestCategory>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<TestCategory>>(_testCategoryDal.GetAll(), Messages.TestCategoryGetAll);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TestCategory>>(Messages.TestCategoryNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<TestCategory> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<TestCategory>(_testCategoryDal.Get(x => x.TestCategoryID == id), id + Messages.TestCategoryGetById);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<TestCategory>(id + Messages.TestCategoryNotGetById + " " + ex.Message);
            }
        }

        public IResult Update(TestCategory entity)
        {
            try
            {
                _testCategoryDal.Update(entity);
                return new SuccessResult(Messages.TestCategoryUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestCategoryNotUpdate + " " + ex.Message);
            }
        }
    }
}
