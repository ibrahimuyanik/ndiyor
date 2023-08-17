using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TestAnswerManager : ITestAnswerService
    {
        private readonly ITestAnswerDal _testAnswerDal;

        public TestAnswerManager(ITestAnswerDal testAnswerDal)
        {
            _testAnswerDal = testAnswerDal;
        }

        public IResult Add(TestAnswer entity)
        {
            try
            {
                _testAnswerDal.Add(entity);
                return new SuccessResult(Messages.TestAnswerAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestAnswerInvalid + " " + ex);
            }
        }

        public IResult Delete(TestAnswer entity)
        {
            try
            {
                _testAnswerDal.Delete(entity);
                return new SuccessResult(Messages.TestAnswerDelete);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestAnswerNotDelete + " " + ex);
            }
        }

        public IDataResult<List<TestAnswer>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<TestAnswer>>(_testAnswerDal.GetAll(), Messages.TestAnswerGetAll);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TestAnswer>>(Messages.TestAnswerNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<TestAnswer> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<TestAnswer>(_testAnswerDal.Get(x => x.TestAnswerID == id), id + Messages.TestAnswerGetById);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<TestAnswer>(id + Messages.TestAnswerNotGetById + " " + ex.Message);
            }
        }

        public IResult Update(TestAnswer entity)
        {

            try
            {
                _testAnswerDal.Update(entity);
                return new SuccessResult(Messages.TestAnswerUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.TestAnswerNotUpdate + " " + ex.Message);
            }
        }
    }
}
