using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADR.Models
{
    public class DataResult<T>
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }

        public static DataResult<T> Success(T data) =>        
            new DataResult<T>()
            {
                IsSuccess = true,
                Data = data
            };        

        public static DataResult<T> Fail(string message = null, T data = default) =>
            new DataResult<T>()
            {
                IsSuccess = false,
                ErrorMessage = message
            };        
    }
}
