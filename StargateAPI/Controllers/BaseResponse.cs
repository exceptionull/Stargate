using System.Net;

namespace StargateAPI.Controllers
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = Constants.ResponseSuccessMessage;
        public int ResponseCode { get; set; } = (int)HttpStatusCode.OK;
    }
}