using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;

        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        public IResult Add(Contact entity)
        {

            try
            {
                _contactDal.Add(entity);
                return new SuccessResult(Messages.ContactAdded);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.ContactInvalid + " " + ex);
            }
        }

        public IResult Delete(Contact entity)
        {
            try
            {
                _contactDal.Delete(entity);
                return new SuccessResult(Messages.ContactDelete);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.ContactNotDelete + " " + ex);
            }
        }

        public IDataResult<List<Contact>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Contact>>(_contactDal.GetAll(), Messages.ContactGetAll);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Contact>>(Messages.ContactNotGetAll + " " + ex.Message);
            }
        }

        public IDataResult<Contact> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Contact>(_contactDal.Get(x => x.ContactID == id), id + Messages.ContactGetById);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Contact>(id + Messages.ContactNotGetById + " " + ex.Message);
            }

        }

        public IResult Update(Contact entity)
        {
            try
            {
                _contactDal.Update(entity);
                return new SuccessResult(Messages.CategoryUpdate);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.CategoryNotUpdate + " " + ex.Message);
            }
        }
    }
}
