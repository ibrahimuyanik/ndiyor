using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class EatManager : IEatService
    {
        private readonly IEatDal _eatDal;

        public EatManager(IEatDal eatDal)
        {
            _eatDal = eatDal;
        }

        public IResult Add(Eat entity)
        {
            try
            {
                _eatDal.Add(entity);
                return new SuccessResult(Messages.EatAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.EatInvalid + " " + ex.Message);

            }
        }

        public IResult Delete(Eat entity)
        {
            try
            {
                _eatDal.Update(entity);
                return new SuccessResult(Messages.EatDelete);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.EatNotDelete + " " + ex.Message);   
            }
        }

        public IDataResult<List<Eat>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Eat>>(_eatDal.GetAll(), Messages.EatGetAll);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<Eat>>(Messages.EatNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<Eat> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Eat>(_eatDal.Get(x => x.EatID == id), id + Messages.EatGetById);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<Eat>(id + Messages.EatNotGetById + " " + ex.Message);    
            }
        }

        public IDataResult<Eat> GetByIdAll(int id)
        {
            return new SuccessDataResult<Eat>(_eatDal.GetByIdAll(id), "listelendi");
        }

        public IDataResult<EatDetailDto> GetEatDetailById(int eatId)
        {
            try
            {
                return new SuccessDataResult<EatDetailDto>(_eatDal.GetEatDetailById(eatId), "Yemekler Listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<EatDetailDto>(_eatDal.GetEatDetailById(eatId), "Hata" + ex.Message);
            }
        }

        public IResult Update(Eat entity)
        {
            try
            {
                _eatDal.Update(entity);
                return new SuccessResult(Messages.EatUpdate);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.EatNotUpdate + " " + ex.Message);
            }
        }
    }
}
