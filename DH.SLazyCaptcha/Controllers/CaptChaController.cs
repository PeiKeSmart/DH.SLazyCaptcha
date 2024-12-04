using DH.RateLimter;

using Microsoft.AspNetCore.Mvc;

using Pek.Cookies;
using Pek.Ids;

namespace DH.SLazyCaptcha.Controllers;

/// <summary>
/// 验证码控制器
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public partial class CaptChaController : Controller
{
    public readonly ICaptcha Captcha;
    public readonly ICookie PekCookies;

    public CaptChaController(ICaptcha _Captcha, ICookie pekCookies)
    {
        Captcha = _Captcha;
        PekCookies = pekCookies;
    }

    /// <summary>
    /// 验证码，适用于跨平台。
    /// </summary>
    /// <returns></returns>
    [RateValve(Policy = Policy.Ip, Limit = 30, Duration = 60)]
    public IActionResult GetCheckCode()
    {
        Int64 SId;
        try
        {
            SId = PekCookies?.GetValue<Int64>("sid") ?? 0;
            if (SId <= 0)
            {
                SId = IdHelper.GetSId();
                PekCookies?.SetValue("sid", SId);
            }
        }
        catch
        {
            SId = IdHelper.GetSId();
            PekCookies?.SetValue("sid", SId);
        }

        var info = Captcha.Generate(SId);
        var stream = new MemoryStream(info.Bytes);
        return File(stream, "image/gif");
    }
}