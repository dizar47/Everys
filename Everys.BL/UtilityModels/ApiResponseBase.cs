using Everys.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Everys.BL.UtilityModels
{
    public record class Error(int Code, string Message);

    public abstract class ApiResponseBase
    {
        public bool IsSuccessful { get; protected set; }
    }

    public class BadApiResponse : ApiResponseBase
    {
        const int DEFAULT_ERROR_CODE = (int)HttpStatusCode.InternalServerError;

        public BadApiResponse()
        {
            Error = new Error(DEFAULT_ERROR_CODE, "Something went wrong");
        }

        public BadApiResponse(string message)
        {
            Error = new Error(default, message);
        }

        public BadApiResponse(int code, string message)
        {
            Error = new Error(code, message);
        }

        public BadApiResponse(OurUserFriendlyException e)
        {
            Error = new Error(DEFAULT_ERROR_CODE, e.Message);
        }

        public Error? Error { get; private set; }
    }

    public class ApiResponse<T> : ApiResponseBase
    {
        private ApiResponse() { }

        public ApiResponse(T? data, bool isCached = false)
        {
            IsSuccessful = true;
            Data = data;
            IsCached = isCached;
        }

        public T? Data { get; set; }

        public bool IsCached { get; set; }
    }
}
