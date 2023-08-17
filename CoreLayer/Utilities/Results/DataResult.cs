using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        //public DataResult(bool success, string message) : base(success, message)
        //{
        //}
        //public T Data => throw new NotImplementedException();

        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success) 
        {

        }

        public T Data { get; }
    }
}
