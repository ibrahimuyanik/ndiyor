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
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public IResult Add(Comment entity)
        {
            try
            {
                _commentDal.Add(entity);
                return new SuccessResult("Yorum Yapıldı");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Yorum Yapılmadı" + " " + ex.Message);

            }
        }

        public IResult Delete(Comment entity)
        {
            try
            {
                _commentDal.Delete(entity);
                return new SuccessResult("Yorum Silindi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Yorum Silinmedi" + " " + ex.Message);

            }
        }

        public IDataResult<List<Comment>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(), "Mesajlar listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Comment>>("Mesajlar listelenmedi" + " " + ex.Message);
            }
        }

        public IDataResult<Comment> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Comment>(_commentDal.Get(x => x.CommentID == id), id + "Numaralı mesaj listelendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Comment>(id + "Numaralı mesaj listelenmedi" + " " + ex.Message);
            }
        }

        public IResult Update(Comment entity)
        {
            try
            {
                _commentDal.Update(entity);
                return new SuccessResult("Mesaj güncellendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Mesaj güncellenmedi" + " " + ex.Message);
            }
        }
    }
}
