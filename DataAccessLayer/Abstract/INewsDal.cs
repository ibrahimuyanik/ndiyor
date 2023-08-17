using CoreLayer.DataAccess;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface INewsDal : IEntityRepository<News>
    {
        List<NewsDetailDto> GetNewsDetails();
        List<News> GetNewsesByCategoryId(int categoryId);
        List<NewsDetailDto> GetNewsesDetailsByCategoryId(int categoryId);
        NewsDetailDto GetNewsDetailById(int newsId);
    }
}
