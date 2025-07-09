using DH.RateLimter;

using Microsoft.AspNetCore.Mvc;

using NewLife.Caching;

using Pek.Helpers;
using Pek.Models;
using Pek.Webs;

namespace DH.SLazyCaptcha.Controllers;

/// <summary>
/// 验证码控制器
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public partial class CaptChaController : Controller
{
    private readonly ICaptcha Captcha;
    private readonly ICacheProvider Cache;

    public CaptChaController(ICaptcha _Captcha, ICacheProvider cache)
    {
        Captcha = _Captcha;
        Cache = cache;
    }

    /// <summary>
    /// 验证码，适用于跨平台。
    /// </summary>
    /// <returns></returns>
    [RateValve(Policy = Policy.Ip, Limit = 30, Duration = 60)]
    public IActionResult GetCheckCode()
    {
        var SId = DHWebHelper.FillDeviceId(Pek.Webs.HttpContext.Current);

        var info = Captcha.GenerateSId(SId);
        var stream = new MemoryStream(info.Bytes);
        return File(stream, "image/gif");
    }

    /// <summary>
    /// 验证码
    /// </summary>
    /// <returns></returns>
    [RateValve(Policy = Policy.Ip, Limit = 600, Duration = 3600)]
    public IActionResult GetVierificationCode()
    {
        var result = new DGResult();

        var SId = DHWebHelper.FillDeviceId(Pek.Webs.HttpContext.Current);
        var info = Captcha.GenerateSId(SId);
        var code = info.Code;

        var data = new
        {
            img = Convert.ToBase64String(info.Bytes),
            uuid = Guid.NewGuid()
        };

        Cache.Cache.Set(data.uuid.ToString(), code, 300);

        result.Code = StateCode.Ok;
        result.Data = data;
        return result;
    }
}