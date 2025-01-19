using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMed.Shared
{
    public class ServiceResponse<T>
    {
        public ServiceResponse(T data , string errrorMessage = "") 
        {
           Data = data;
           ErrorMessage = errrorMessage;
        }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
