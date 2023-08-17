using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface INewsService : IGenericService<News>
    {
        IDataResult<List<NewsDetailDto>> GetNewsDetails();
        IDataResult<List<News>> GetNewsesByCategoryId(int categoryId);
        IDataResult<NewsDetailDto> GetNewsDetailById(int newsId);
        IDataResult<List<NewsDetailDto>> GetNewsesDetailsByCategoryId(int categoryId);
        //IDataResult<NewsDetailDto> DeleteEditorStatus(News news);
    }
}
