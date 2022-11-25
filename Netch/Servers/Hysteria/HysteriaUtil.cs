using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using Netch.Interfaces;
using Netch.Models;
using Netch.Utils;

namespace Netch.Servers;

public class HysteriaUtil : IServerUtil
{
    public ushort Priority { get; } = 3;

    public string TypeName { get; } = "Hysteria";

    public string FullName { get; } = "Hysteria";

    public string ShortName { get; } = "hy";

    public string[] UriScheme { get; } = { "hysteria" };

    public Type ServerType { get; } = typeof(HysteriaServer);

    public void Edit(Server s)
    {
        new HysteriaForm((HysteriaServer)s).ShowDialog();
    }

    public void Create()
    {
        new HysteriaForm().ShowDialog();
    }

    public string GetShareLink(Server s)
    {
        return GetHShareLink(s);
    }

    public IServerController GetController()
    {
        return new HysteriaController();
    }

    public IEnumerable<Server> ParseUri(string text)
    {
        var data = new HysteriaServer();

        string s;
        try
        {
            s = ShareLink.URLSafeBase64Decode(text.Substring(8));
        }
        catch
        {
            return ParseHUri(text);
        }

        throw new FormatException();
    
    }

    public bool CheckServer(Server s)
    {
        return true;
    }

    public static IEnumerable<Server> ParseHUri(string text)
    {
        var scheme = ShareLink.GetUriScheme(text).ToLower();
        var server = scheme switch { "hysteria" => new HysteriaServer(), _ => throw new ArgumentOutOfRangeException() };
        if (text.Contains("#"))
        {
            server.Remark = Uri.UnescapeDataString(text.Split('#')[1]);
            text = text.Split('#')[0];
        }

        if (text.Contains("?"))
        {
            var parameter = HttpUtility.ParseQueryString(text.Split('?')[1]);
            text = text.Substring(0, text.IndexOf("?", StringComparison.Ordinal));
            server.Protocol = parameter.Get("protocol") ?? "udp";
            server.AuthStr = parameter.Get("auth") ?? parameter.Get("auth_str" ?? "");
            server.ServerName = parameter.Get("peer") ?? "";

            server.Insecure = false;
            if (bool.TryParse(parameter.Get("insecure"), out bool insecure))
            {
                server.Insecure = insecure;
            }

            server.UpMbps = 10;
            if (double.TryParse(parameter.Get("upmbps"), out double upmbps))
            {
                server.UpMbps = upmbps;
            }

            server.DownMbps = 10;
            if (double.TryParse(parameter.Get("downmbps"), out double downmpbs))
            {
                server.DownMbps = downmpbs;
            }

            server.Alpn = parameter.Get("alpn") ?? "";
            server.Obfs = parameter.Get("obfs") ?? "";
            server.ObfsParam = parameter.Get("obfsParam") ?? "";

        }

        
        var finder = new Regex(@$"^{scheme}://(?<host>.+):(?<port>\d+)");
        var match = finder.Match(text.Split('?')[0]);
        if (!match.Success)
            throw new FormatException();

        server.Hostname = match.Groups["host"].Value;
        server.Port = ushort.Parse(match.Groups["port"].Value);

        return new[] { server };
    }

    public static string GetHShareLink(Server s, string scheme = "hysteria")
    {
        var server = (HysteriaServer)s;
        var parameter = new Dictionary<string, string>();
        // protocol-specific fields
        parameter.Add("protocol", server.Protocol);
        if (server.AuthStr != "") parameter.Add("auth", server.AuthStr ?? "");
        if (server.ServerName != "") parameter.Add("peer", server.ServerName ?? "");
        parameter.Add("insecure", server.Insecure.ToString());
        parameter.Add("upmbps", server.UpMbps.ToString() ?? "");
        parameter.Add("downmbps", server.DownMbps.ToString() ?? "");
        if (server.Alpn != "") parameter.Add("alpn", server.Alpn ?? "");
        if (server.Obfs != "") parameter.Add("obfs", server.Obfs ?? "");
        if (server.ObfsParam != "") parameter.Add("obfsParam", server.ObfsParam ?? "");

        return
            $"{scheme}://{server.Hostname}:{server.Port}?{string.Join("&", parameter.Select(p => $"{p.Key}={p.Value}"))}{(!server.Remark.IsNullOrWhiteSpace() ? $"#{Uri.EscapeDataString(server.Remark)}" : "")}";
    }
}