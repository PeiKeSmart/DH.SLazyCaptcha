﻿using DH.SLazyCaptcha.Generator.Code;
using DH.SLazyCaptcha.Generator.Image;
using DH.SLazyCaptcha.Storage;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NewLife;
using NewLife.Caching;
using NewLife.Log;

namespace DH.SLazyCaptcha;

public class DefaultCaptcha : ICaptcha
{
    protected CaptchaOptions _options;
    protected IStorage _storage;
    protected ICaptchaCodeGenerator _captchaCodeGenerator;
    protected ICaptchaImageGenerator _captchaImageGenerator;

    public DefaultCaptcha(IOptionsSnapshot<CaptchaOptions> options, IStorage storage)
    {
        _options = options.Value;
        _storage = storage;

        ChangeOptions(_options);

        _captchaCodeGenerator = new DefaultCaptchaCodeGenerator(_options.CaptchaType);
        _captchaImageGenerator = new DefaultCaptchaImageGenerator();
    }

    // 选项更新
    protected virtual void ChangeOptions(CaptchaOptions options)
    {

    }

    /// <summary>
    /// 使用session及固定Key
    /// </summary>
    /// <returns></returns>
    public virtual CaptchaData Generate()
    {
        var captchaId = "ybbcode";

        var (renderText, code) = _captchaCodeGenerator.Generate(_options.CodeLength);
        var image = _captchaImageGenerator.Generate(renderText, _options.ImageOption);

        Pek.Webs.HttpContext.Current.Session.SetString(captchaId, code);

        return new CaptchaData(captchaId, code, image);
    }

    /// <summary>
    /// 使用session及固定Key
    /// </summary>
    /// <returns></returns>
    public virtual CaptchaData GenerateSId(String SId)
    {
        var captchaId = "ybbcode";

        var (renderText, code) = _captchaCodeGenerator.Generate(_options.CodeLength);
        var image = _captchaImageGenerator.Generate(renderText, _options.ImageOption);

        XTrace.WriteLine($"[DefaultCaptcha.Generate]验证码存储方式：{_options.StoreType}:{SId}:{code}");

        if (_options.StoreType == StoreType.Session || SId.IsNullOrWhiteSpace())
            Pek.Webs.HttpContext.Current.Session.SetString(captchaId, code);
        else
        {
            var cache = Pek.Webs.HttpContext.Current.RequestServices.GetRequiredService<ICacheProvider>().Cache;

            cache.Set($"{SId}CaptchaCode", code, 5 * 30);
        }

        return new CaptchaData(captchaId, code, image);
    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <param name="expirySeconds">缓存时间，未设定则使用配置时间</param>
    /// <returns></returns>
    public virtual CaptchaData Generate(String captchaId, Int32? expirySeconds = null)
    {
        var (renderText, code) = _captchaCodeGenerator.Generate(_options.CodeLength);
        var image = _captchaImageGenerator.Generate(renderText, _options.ImageOption);
        expirySeconds ??= _options.ExpirySeconds;
        _storage.Set(captchaId, code, DateTime.Now.AddSeconds(expirySeconds.Value).ToUniversalTime());

        return new CaptchaData(captchaId, code, image);
    }

    /// <summary>
    /// 校验
    /// </summary>
    /// <param name="captchaId">验证码id</param>
    /// <param name="code">用户输入的验证码</param>
    /// <param name="removeIfSuccess">校验成功时是否移除缓存(用于多次验证)</param>
    /// <param name="removeIfFail">校验失败时是否移除缓存</param>
    /// <returns></returns>
    public virtual Boolean Validate(String captchaId, String code, Boolean removeIfSuccess = true, Boolean removeIfFail = true)
    {
        var val = _storage.Get(captchaId);
        var comparisonType = _options.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
        var success = !String.IsNullOrWhiteSpace(code) &&
                      !String.IsNullOrWhiteSpace(val) &&
                      String.Equals(val, code, comparisonType);

        if ((!success && removeIfFail) || (success && removeIfSuccess))
        {
            _storage.Remove(captchaId);
        }

        return success;
    }
}