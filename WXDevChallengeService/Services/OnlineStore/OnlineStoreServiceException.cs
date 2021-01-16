using System;

namespace WXDevChallengeService.Services.OnlineStore
{
    public class OnlineStoreServiceException : ArgumentException
    {
        public OnlineStoreServiceException() : base("Invalid sorting option.")
        {
        }
    }
}