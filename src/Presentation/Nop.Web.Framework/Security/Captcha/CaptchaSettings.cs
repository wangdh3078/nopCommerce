using Nop.Core.Configuration;

namespace Nop.Web.Framework.Security.Captcha
{
    /// <summary>
    /// 验证码设置
    /// </summary>
    public class CaptchaSettings : ISettings
    {
        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        ///登录页面是否启用验证码
        /// </summary>
        public bool ShowOnLoginPage { get; set; }
        /// <summary>
        /// 是否在注册页面显示验证码
        /// </summary>
        public bool ShowOnRegistrationPage { get; set; }
        /// <summary>
        ///是否应在联系人页面上显示验证码
        /// </summary>
        public bool ShowOnContactUsPage { get; set; }
        /// <summary>
        /// 是否应该在心愿单页面上显示验证码
        /// </summary>
        public bool ShowOnEmailWishlistToFriendPage { get; set; }
        /// <summary>
        /// 是否应在“电子邮件朋友”页面上显示验证码
        /// </summary>
        public bool ShowOnEmailProductToFriendPage { get; set; }
        /// <summary>
        /// 验证码是否应显示在“评论博客”页面上
        /// </summary>
        public bool ShowOnBlogCommentPage { get; set; }
        /// <summary>
        ///是否应在“评论新闻”页面上显示验证码
        /// </summary>
        public bool ShowOnNewsCommentPage { get; set; }
        /// <summary>
        /// 是否应在产品评论页面上显示验证码
        /// </summary>
        public bool ShowOnProductReviewPage { get; set; }
        /// <summary>
        ///是否应在“申请供应商帐户”页面上显示验证码
        /// </summary>
        public bool ShowOnApplyVendorPage { get; set; }
        /// <summary>
        /// reCAPTCHA public key
        /// </summary>
        public string ReCaptchaPublicKey { get; set; }
        /// <summary>
        /// reCAPTCHA private key
        /// </summary>
        public string ReCaptchaPrivateKey { get; set; }
        /// <summary>
        /// reCAPTCHA version
        /// </summary>
        public ReCaptchaVersion ReCaptchaVersion { get; set; }
        /// <summary>
        /// reCAPTCHA theme
        /// </summary>
        public string ReCaptchaTheme { get; set; }
        /// <summary>
        /// reCAPTCHA language
        /// </summary>
        public string ReCaptchaLanguage { get; set; }
    }
}