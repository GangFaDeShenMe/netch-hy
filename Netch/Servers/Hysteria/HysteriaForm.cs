using Netch.Forms;

namespace Netch.Servers;

[Fody.ConfigureAwait(true)]
public class HysteriaForm : ServerForm
{
    public HysteriaForm(HysteriaServer? server = default)
    {
        server ??= new HysteriaServer();
        Server = server;
        CreateComboBox("Protocol",
            "Protocol",
            new List<string> { "udp" },
            s => server.Protocol = s switch { _ => "udp" },
            server.Protocol
            );
        CreateTextBox("UpMbps", "Max Up Mbps", s => double.TryParse(s, out _), s => server.UpMbps = double.Parse(s), server.UpMbps.ToString(), 76);
        CreateTextBox("DownMbps", "Max Down Mbps", s => double.TryParse(s, out _), s => server.DownMbps = double.Parse(s), server.DownMbps.ToString(), 76);
        CreateTextBox("OBFS", "OBFS", s => true, s => server.Obfs = s, server.Obfs);
        CreateTextBox("AuthStr", "Auth String", s => true, s => server.AuthStr = s, server.AuthStr);
        CreateTextBox("Alpn", "QUIC TLS ALPN", s => true, s => server.Alpn = s, server.Alpn);
        CreateTextBox("ServerName", "Host", s => true, s => server.ServerName = s, server.ServerName);
        CreateComboBox("Insecure",
            "Ignore TLS error",
            new List<string> { "True", "False" },
            s => server.Insecure = s switch {  "True" => true, "False" => false, _ => true },
            server.Insecure.ToString()
            );
    }

    protected override string TypeName { get; } = "Hysteria";
}