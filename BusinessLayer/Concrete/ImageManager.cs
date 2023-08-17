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
    public class ImageManager : IImageService
    {
        private readonly IImageDal _ımageDal;

        public ImageManager(IImageDal ımageDal)
        {
            _ımageDal = ımageDal;
        }

        public IResult Add(Image entity)
        {
            try
            {
                _ımageDal.Add(entity);
                return new SuccessResult(Messages.ImageAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.ImageInvalid + " " + ex);
            }
        }

        public IResult Delete(Image entity)
        {
            try
            {
                _ımageDal.Delete(entity);
                return new SuccessResult(Messages.ImageDelete);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.ImageNotDelete + " " + ex);
            }
        }

        public IDataResult<List<Image>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Image>>(_ımageDal.GetAll(), Messages.ImageGetAll);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Image>>(Messages.ImageNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<Image> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Image>(_ımageDal.Get(x => x.ImageID == id), id + Messages.ImageGetById);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Image>(id + Messages.ImageNotGetById + " " + ex.Message);
            }
        }

        public IResult Update(Image entity)
        {
            try
            {
                _ımageDal.Update(entity);
                return new SuccessResult(Messages.ImageUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.ImageNotUpdate + " " + ex.Message);
            }
        }
    }
}
