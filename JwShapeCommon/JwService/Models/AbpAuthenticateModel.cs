﻿
namespace JwShapeCommon.JwService.Models
{
    public class AbpAuthenticateModel 
    {
        public string UserNameOrEmailAddress { get; set; }

        public string Password { get; set; }

        public string TwoFactorVerificationCode { get; set; }

        public bool RememberClient { get; set; }

        public string TwoFactorRememberClientToken { get; set; }

        public bool? SingleSignIn { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsTwoFactorVerification => !string.IsNullOrEmpty(TwoFactorVerificationCode);
    }
}