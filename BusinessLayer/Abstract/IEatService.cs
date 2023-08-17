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
    public interface IEatService : IGenericService<Eat>
    {
        IDataResult<Eat> GetByIdAll(int id);
        IDataResult<EatDetailDto> GetEatDetailById(int eatId);
    }
}
