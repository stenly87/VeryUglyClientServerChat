// клиент
/*using System.Net.Sockets;

TcpClient tcpClient = new("192.168.4.100", 50000);
using var ns = tcpClient.GetStream();
ns.ReadTimeout = Timeout.Infinite;
using var tr = new StreamReader(ns);
using var sw = new StreamWriter(ns);
Thread thread = new Thread(StartListener);
thread.Start(tr);

string message = null;
while (message != "exit")
{ 
    message = Console.ReadLine();
    sw.WriteLine(message);
    sw.Flush();
}

void StartListener(object arg)
{
    StreamReader tr = (StreamReader)arg;
    string message = null;
    while (message != "exit")
    {
        message = tr.ReadLine();
        Console.WriteLine(message);
    }
}*/


using ConsoleApp10;
using System.Net;
using System.Net.Sockets;

TcpListener tcpListener = new TcpListener(
    IPAddress.Parse("192.168.4.100"), 50000);
tcpListener.Start();

while (true)
{
    var tcpClient = tcpListener.AcceptTcpClient();
    Thread thread = new Thread(StartWork);
    thread.Start(tcpClient);
}


void StartWork(object arg)
{
    TcpClient tcpClient = (TcpClient)arg;
    using var ns = tcpClient.GetStream();
    using var sr = new StreamReader(ns);
    using var sw = new StreamWriter(ns);
    Clients.allClients.TryAdd(sw, true);
    ns.ReadTimeout = Timeout.Infinite;
    string message = null;
    while (message != "exit")
    {
        try
        {
            message = sr.ReadLine();
            Thread.Sleep(1000);
        }
        catch
        {
            Clients.allClients.Remove(sw, out _);
            break;
        }
        foreach (var client in Clients.allClients)
        {
            if (client.Key != sw)
            {
                try
                {
                    if (client.Key != null)
                    {
                        client.Key.WriteLine(message);
                        client.Key.Flush();
                    }
                    else
                        break;
                }
                catch
                {
                    break;
                }
            }
        }
    }
    Clients.allClients.Remove(sw, out _ );
}