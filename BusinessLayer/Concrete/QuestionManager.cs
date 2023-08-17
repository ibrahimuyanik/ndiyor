using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using DataAccessLayer.Migrations;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class QuestionManager : IQuestionService
    {
        private readonly IQuestionDal _questionDal;


        public QuestionManager(IQuestionDal questionDal)
        {
            _questionDal = questionDal;
        }

        public IResult Add(Question entity)
        {
            try
            {
                _questionDal.Add(entity);
                return new SuccessResult(Messages.QuesitonAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.QuesitonInvalid + " " + ex);
            }
        }

        public IResult Delete(Question entity)
        {
            try
            {
                _questionDal.Delete(entity);
                return new SuccessResult(Messages.QuesitonDelete);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.QuesitonNotDelete + " " + ex);
            }
        }

        public IDataResult<List<Question>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Question>>(_questionDal.GetAll(), Messages.QuesitonGetAll);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Question>>(Messages.QuesitonNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<Question> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Question>(_questionDal.Get(x => x.QuestionID == id), id + Messages.QuesitonGetById);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Question>(id + Messages.QuesitonNotGetById + " " + ex.Message);
            }
        }

        public IDataResult<QuesitonDetailDto> GetQuestionDetailById(int id)
        {
            try
            {
                return new SuccessDataResult<QuesitonDetailDto>(_questionDal.GetQuestionDetailById(id), "Sorular Listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<QuesitonDetailDto>(_questionDal.GetQuestionDetailById(id), "Hata" + ex.Message);
            }
        }

        public IResult Update(Question entity)
        {
            try
            {
                _questionDal.Update(entity);
                return new SuccessResult(Messages.QuesitonUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.QuesitonNotUpdate + " " + ex.Message);
            }
        }

       
    }
}
