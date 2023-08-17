using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class NewsManager : INewsService
    {
        private readonly INewsDal _newsDal;

        public NewsManager(INewsDal newsDal)
        {
            _newsDal = newsDal;
        }

        public IResult Add(News entity)
        {           
            try
            {
                _newsDal.Add(entity);
                return new SuccessResult(Messages.NewsAdded);

            } catch (Exception ex)
            {
                return new ErrorResult(Messages.NewsInvalid + " " + ex.Message);
            }
            
        }

        public IResult Delete(News entity)
        {
            
            try
            {
                _newsDal.Update(entity);
                return new SuccessResult(Messages.NewsDelete);

            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.NewsNotDelete + " " + ex.Message);
            }
        }

        public IDataResult<List<News>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<News>>(_newsDal.GetAll(), Messages.NewsGetAll);
            }
            catch(Exception ex)
            {
                return new ErrorDataResult<List<News>>(Messages.NewsNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<News> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<News>(_newsDal.Get(x => x.NewsID == id), id + Messages.NewsGetById);

            }
            catch (Exception ex)
            {
                return new ErrorDataResult<News>(id + Messages.NewsNotGetById + " " + ex.Message);
            }
        }

        public IDataResult<List<News>> GetNewsesByCategoryId(int categoryId)
        {
            var value = new SuccessDataResult<List<News>>(_newsDal.GetNewsesByCategoryId(categoryId),"Deneme");
            return value;
            
        }

        public IDataResult<List<NewsDetailDto>> GetNewsDetails()
        {
            try
            {
                return new SuccessDataResult<List<NewsDetailDto>>(_newsDal.GetNewsDetails(), "Haber listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<NewsDetailDto>>(_newsDal.GetNewsDetails(), "Haber Listelenemedi" + " " + ex);
            }
        }

        public IResult Update(News entity)
        {
            try
            {
                _newsDal.Update(entity);
                return new SuccessResult(Messages.NewsUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.NewsNotUpdate + " " + ex.Message);
            }
        }

        public IDataResult<NewsDetailDto> GetNewsDetailById(int newsId)
        {
            

            try
            {
                return new SuccessDataResult<NewsDetailDto>(_newsDal.GetNewsDetailById(newsId), "Haber Listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<NewsDetailDto>(_newsDal.GetNewsDetailById(newsId), "Hata" + ex.Message);
            }
        }

        public IDataResult<List<NewsDetailDto>> GetNewsesDetailsByCategoryId(int categoryId)
        {
            try
            {
                return new SuccessDataResult<List<NewsDetailDto>>(_newsDal.GetNewsesDetailsByCategoryId(categoryId), "Deneme");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<NewsDetailDto>>(_newsDal.GetNewsesDetailsByCategoryId(categoryId) + " " + ex.Message);
            }
        }

        
    }
}
