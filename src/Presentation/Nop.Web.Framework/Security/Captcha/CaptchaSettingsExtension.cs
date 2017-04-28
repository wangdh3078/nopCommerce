using Nop.Services.Localization;

namespace Nop.Web.Framework.Security.Captcha
{
    /// <summary>
    /// 验证码设置扩展
    /// </summary>
    public static class CaptchaSettingsExtension
    {
        /// <summary>
        /// 获取错误验证码信息
        /// </summary>
        /// <param name="captchaSettings">验证码设置</param>
        /// <param name="localizationService">本地化服务</param>
        /// <returns></returns>
        public static string GetWrongCaptchaMessage(this CaptchaSettings captchaSettings,
            ILocalizationService localizationService)
        {
            if (captchaSettings.ReCaptchaVersion == ReCaptchaVersion.Version1)
                return localizationService.GetResource("Common.WrongCaptcha");
            else if (captchaSettings.ReCaptchaVersion == ReCaptchaVersion.Version2)
                return localizationService.GetResource("Common.WrongCaptchaV2");
            return string.Empty;
        }
    }
}