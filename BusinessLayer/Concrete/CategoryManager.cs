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
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IResult Add(Category entity)
        {
            try
            {
                _categoryDal.Add(entity);
                return new SuccessResult(Messages.CategoryAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.CategoryInvalid + " " + ex);
            }
        }

        public IResult Delete(Category entity)
        {
            try
            {
                _categoryDal.Delete(entity);
                return new SuccessResult(Messages.CategoryDelete);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.CategoryNotDelete + " " + ex);
            }
        }

        public IDataResult<List<Category>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Category>>(_categoryDal.GetAll(), Messages.CategoryGetAll);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<Category>>(Messages.CategoryNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<Category> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Category>(_categoryDal.Get(x => x.CategoryID == id), id + Messages.CategoryGetById);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<Category>(id + Messages.CategoryNotGetById + " " + ex.Message);
            }
        }

        public IResult Update(Category entity)
        {
            try
            {
                _categoryDal.Update(entity);
                return new SuccessResult(Messages.CategoryUpdate);
            }
            catch(Exception ex)
            {
                return new ErrorResult(Messages.CategoryNotUpdate + " " + ex.Message);
            }
        }
    }
}
