using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestModels
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, bool success = true)
        {
            Success = success;
            Data = data;
            StatusCode = 200;
        }

        public ApiResponse(string errorMessage, int statusCode)
        {
            Success = false;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}
