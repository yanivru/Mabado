using System;
using System.Diagnostics;
using Mabado.View.Domain;

namespace PlayGround
{
    class Program
    {
        private static void Main()
        {
            ConnectionInfo info = new ConnectionInfo {Ip = "10.10.230.30"};

            ConnectionStringGueser guesser = new ConnectionStringGueser();

            Stopwatch swatch = new Stopwatch();
            swatch.Start();

            ConnectionStringResolver resolver = new ConnectionStringResolver(guesser)
                {
                    ConnectionFailed = (connectionInfo, exception) => Console.WriteLine(@"Failed: {0} Exception: {1}", connectionInfo.DataSource, exception.InnerException.Message),
                    ConnectionSucceeded = connectionInfo => Console.WriteLine(@"Success: {0}", connectionInfo.DataSource),
                    ResolveCompleted = () => Console.WriteLine(@"Finished after: " + swatch.Elapsed)
                };

            resolver.Resolve(info);

            Console.ReadLine();
        }
    }
}
