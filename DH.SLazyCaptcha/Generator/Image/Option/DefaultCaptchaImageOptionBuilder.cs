using SkiaSharp;

namespace DH.SLazyCaptcha.Generator.Image.Option;

public class DefaultCaptchaImageOptionBuilder : ICaptchaImageOptionBuilder
{
    private CaptchaImageGeneratorOption _option = new CaptchaImageGeneratorOption();

    public static DefaultCaptchaImageOptionBuilder Create()
    { 
        return new DefaultCaptchaImageOptionBuilder();
    }

    /// <summary>
    /// 背景色
    /// </summary>
    /// <param name="backgroundColor"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder CaptchaType(SKColor backgroundColor)
    {
        _option.BackgroundColor = backgroundColor;
        return this;
    }

    /// <summary>
    /// 字体
    /// </summary>
    /// <param name="fontFamily"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder FontFamily(SKTypeface fontFamily)
    {
        _option.FontFamily = fontFamily;
        return this;
    }

    /// <summary>
    /// 字体大小
    /// </summary>
    /// <param name="fontSize"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder FontFamily(float fontSize)
    {
        _option.FontSize = fontSize;
        return this;
    }

    /// <summary>
    /// 验证码的宽高
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder Size(int width, int height)
    {
        _option.Width = width;
        _option.Height = height;
        return this;
    }

    /// <summary>
    /// 气泡数量
    /// </summary>
    /// <param name="bubbleCount"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder BubbleCount(int bubbleCount)
    {
        _option.BubbleCount = bubbleCount;
        return this;
    }

    /// <summary>
    /// 气泡边沿厚度
    /// </summary>
    /// <param name="bubbleThickness"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder BubbleCount(float bubbleThickness)
    {
        _option.BubbleThickness = bubbleThickness;
        return this;
    }

    /// <summary>
    /// 干扰线数量
    /// </summary>
    /// <param name="interferenceLineCount"></param>
    /// <returns></returns>
    public DefaultCaptchaImageOptionBuilder InterferenceLineCount(int interferenceLineCount)
    {
        _option.InterferenceLineCount = interferenceLineCount;
        return this;
    }


    public CaptchaImageGeneratorOption Build()
    {
        return _option;
    }
}
