using DH.RateLimter;

using Microsoft.AspNetCore.Mvc;

namespace DH.SLazyCaptcha.Controllers;

/// <summary>
/// 验证码控制器
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public partial class CaptChaController : Controller {

    public readonly ICaptcha Captcha;

    public CaptChaController(ICaptcha _Captcha)
    {
        Captcha = _Captcha;
    }

    /// <summary>
    /// 验证码，适用于跨平台。
    /// </summary>
    /// <returns></returns>
    [RateValve(Policy = Policy.Ip, Limit = 30, Duration = 60)]
    public IActionResult GetCheckCode()
    {
        var info = Captcha.Generate();
        var stream = new MemoryStream(info.Bytes);
        return File(stream, "image/gif");
    }
}