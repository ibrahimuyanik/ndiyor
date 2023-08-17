using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfNewsDal : EfEntityRepositoryBase<News, Context>, INewsDal
    {
        public List<News> GetNewsesByCategoryId(int categoryId)
        {
            using (Context context = new Context())
            {
                var result = context.Newses.Include(x=> x.Category).Where(x => x.CategoryID == categoryId).ToList();
                return result;
            }
        }

        public List<NewsDetailDto> GetNewsDetails()
        {
            using (Context context = new Context())
            {
                var result = from c in context.Categories
                             join n in context.Newses
                             on
                             c.CategoryID equals n.CategoryID
                             select new NewsDetailDto
                             {
                                 CategoryID = c.CategoryID,
                                 CategoryName = c.CategoryName,
                                 Content = n.Content,
                                 Title = n.Title,
                                 NewsID = n.NewsID,
                                 Photo = n.Photo,
                                 SavedFileName = n.SavedFileName,
                                 SavedUrl = n.SavedUrl,
                                 SignedUrl = n.SignedUrl,
                                 ReleaseDate = n.ReleaseDate
                             };
                return result.ToList();

            }
        }


        public List<NewsDetailDto> GetNewsesDetailsByCategoryId(int categoryId)
        {
            using (Context context = new Context())
            {
                var result = from c in context.Categories
                             join n in context.Newses
                             on
                             c.CategoryID equals n.CategoryID
                             where n.CategoryID == categoryId
                             select new NewsDetailDto
                             {
                                 CategoryID = c.CategoryID,
                                 CategoryName = c.CategoryName,
                                 Content = n.Content,
                                 Title = n.Title,
                                 ReleaseDate = n.ReleaseDate,
                                 NewsID = n.NewsID,
                             };

                return result.ToList();
            }
        }

        public NewsDetailDto GetNewsDetailById(int newsId)
        {
            using (Context context = new Context())
            {
                var result = context.Newses.Include(x => x.Category).First(x => x.NewsID == newsId);
                return new NewsDetailDto
                {
                    CategoryID = result.CategoryID,
                    CategoryName = result.Category.CategoryName,
                    Content = result.Content,
                    Title = result.Title,
                    ReleaseDate = result.ReleaseDate,
                    NewsID = result.NewsID,
                    SavedFileName = result.SavedFileName,
                    SavedUrl = result.SavedUrl,
                    SignedUrl = result.SignedUrl,
                    Photo = result.Photo,
                };
                
            }
        }
    }
}
