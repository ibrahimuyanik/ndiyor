using BusinessLayer.Abstract;
using BusinessLayer.Constants;
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
    public class SocialMediaManager : ISocialMediaService
    {
        private readonly ISocialMediaDal _socialMediaDal;

        public SocialMediaManager(ISocialMediaDal socialMediaDal)
        {
            _socialMediaDal = socialMediaDal;
        }

        public IResult Add(SocialMedia entity)
        {
            try
            {
                _socialMediaDal.Add(entity);
                return new SuccessResult(Messages.SocialMediaAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.SocialMediaInvalid + " " +  ex.Message);
            }
        }

        public IResult Delete(SocialMedia entity)
        {
            try
            {
                _socialMediaDal.Delete(entity);
                return new SuccessResult(Messages.SocialMediaDelete);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.SocialMediaNotDelete + " " + ex.Message);
            }
        }

        public IDataResult<List<SocialMedia>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<SocialMedia>>(_socialMediaDal.GetAll(), Messages.SocialMediaGetAll);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<SocialMedia>>(Messages.SocialMediaNotGetAll + " " + ex);
            }
        }

        public IDataResult<SocialMedia> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<SocialMedia>(_socialMediaDal.Get(x => x.SocialMediaID == id), Messages.SocialMediaGetById);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<SocialMedia>(id + Messages.SocialMediaNotGetById + " " + ex.Message); 
            }
        }

        public IResult Update(SocialMedia entity)
        {
            try
            {
                _socialMediaDal.Update(entity);
                return new SuccessResult(Messages.SocialMediaUpdate);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.SocialMediaNotUpdate + " " + ex.Message);
            }
        }
    }
}
