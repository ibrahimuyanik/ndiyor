using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfEatDal : EfEntityRepositoryBase<Eat, Context>, IEatDal
    {
        Context context = new Context();
        
        public Eat GetByIdAll(int id)
        {
            var result = context.Eats.Include(x => x.Comments).ThenInclude(y => y.AppUser).Where(x=>x.EatID == id).FirstOrDefault();
            return result;
        }

        public EatDetailDto GetEatDetailById(int eatId)
        {
            var result = context.Eats.First(x=>x.EatID==eatId);
            return new EatDetailDto
            {
                 EatID=result.EatID,
                  Comments=result.Comments,
                   EatTitle=result.EatTitle,
                    Content=result.Content,
                     Photo = result.Photo,
                      ReleaseDate=result.ReleaseDate,
                       SavedFileName = result.SavedFileName,
                        SavedUrl = result.SavedUrl,
                         SignedUrl=result.SignedUrl,
            };
        }
    }
}
 