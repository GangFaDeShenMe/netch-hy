#nullable disable
// ReSharper disable InconsistentNaming

using System.Security.Cryptography.X509Certificates;

namespace Netch.Servers;

public class HysteriaConfig
{
    public Socks5Object socks5 { get; set; }

    /// <summary>
    ///     服务器地址
    /// </summary>
    public string? server { get; set; }

    /// <summary>
    ///     传输协议 只支持udp
    /// </summary>
    public string protocol { get; set; } = HysteriaGlobal.protocol[0];

    /// <summary>
    ///     最大上传速度 mbps
    /// </summary>
    public double up_mbps { get; set; }

    /// <summary>
    ///     最大下载速度 mbps
    /// </summary>
    public double down_mbps { get; set; }

    /// <summary>
    ///     混淆密码
    /// </summary>
    public string? obfs { get; set; }

    /// <summary>
    ///     验证密钥
    /// </summary>
    public string? auth_str { get; set; }

    /// <summary>
    ///     QUIC TLS ALPN
    /// </summary>
    public string? alpn { get; set; }

    /// <summary>
    ///     用于验证服务端证书的 hostname
    /// </summary>
    public string? server_name { get; set; }

    /// <summary>
    ///     忽略一切证书错误
    /// </summary>
    public bool insecure { get; set; }
}

public class Socks5Object
{
    public string listen { get; set; }
}

//public class HysteriaOutbound
//{
//    public string Protocol { get; set; }
//    public double UpMpbs { get; set; }
//    public double DownMbps { get; set; }
//    public string Obfs { get; set; }
//    public string ObfsParam { get; set; }
//    public string Authstr { get; set; }
//    public string Alpn { get; set; }
//    public string ServerName { get; set; }
//    public bool Inserure { get; set; }
//}
