using Netch.Models;

namespace Netch.Servers;

public class HysteriaServer : Server
{
    public override string Type { get; } = "Hysteria";

    public override string MaskedData()
    {
        var maskedData = string.Empty;

        return maskedData;
    }

    /// <summary>
    ///     传输协议 只支持udp
    /// </summary>
    public string Protocol { get; set; } = HysteriaGlobal.protocol[0];

    /// <summary>
    ///     最大上传速度 mbps
    /// </summary>
    public double UpMbps { get; set; }

    /// <summary>
    ///     最大下载速度 mbps
    /// </summary>
    public double DownMbps { get; set; }

    /// <summary>
    ///     混淆密码
    /// </summary>
    public string? Obfs { get; set; }

    public string? ObfsParam { get; set; }

    /// <summary>
    ///     验证密钥
    /// </summary>
    public string? AuthStr { get; set; }

    /// <summary>
    ///     QUIC TLS ALPN
    /// </summary>
    public string? Alpn { get; set; }

    /// <summary>
    ///     用于验证服务端证书的 hostname
    /// </summary>
    public string? ServerName { get; set; }

    /// <summary>
    ///     忽略一切证书错误
    /// </summary>
    public bool Insecure { get; set; }
}

public class HysteriaGlobal
{
    public static readonly List<string> protocol = new()
    {
        "udp"
    };
}