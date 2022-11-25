using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text.Json;
using Netch.Controllers;
using Netch.Interfaces;
using Netch.Models;
using Socks5.Models;

namespace Netch.Servers;

public class HysteriaController : Guard, IServerController
{
    public HysteriaController() : base("hysteria.exe")
    {

    }

    protected override IEnumerable<string> StartedKeywords => new[] { "Client configuration loaded" };

    protected override IEnumerable<string> FailedKeywords => new[] { " Failed to read configuration", "Failed to", "Out of retries, exiting..." };

    public override string Name => "Hysteria";

    public ushort? Socks5LocalPort { get; set; }

    public string? LocalAddress { get; set; }

    public virtual async Task<Socks5Server> StartAsync(Server s)
    {
        var server = (HysteriaServer)s;
        var hysteriaconfig = new HysteriaConfig
        {
            server = server.Hostname + ':' + server.Port,
            protocol = server.Protocol,
            up_mbps = server.UpMbps,
            down_mbps = server.DownMbps,
            obfs = server.Obfs,
            auth_str = server.AuthStr,
            alpn = server.Alpn,
            server_name = server.ServerName,
            insecure = server.Insecure,
            socks5 = new Socks5Object
            {
                listen = Global.Settings.LocalAddress + ":" + Global.Settings.Socks5LocalPort
            }
            
        };
        //hysteriaconfig.socks5 = new Socks5Object
        //{
        //    listen = Global.Settings.LocalAddress + ":" + Global.Settings.Socks5LocalPort
        //};

        await using (var fileStream = new FileStream(Constants.TempConfig, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            await JsonSerializer.SerializeAsync(fileStream, hysteriaconfig, Global.NewCustomJsonSerializerOptions());
        }

        await StartGuardAsync("-config ..\\data\\last.json");
        return new Socks5Server(IPAddress.Loopback.ToString(), this.Socks5LocalPort(), server.Hostname);
    }
}