using System;

namespace tianyanchaOpen.AppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClientHelper.Host = "https://open.tianyancha.com";

            var httpResult = HttpClientHelper.Get<dynamic>("/cloud-open-admin/_feign/interface/find.json?parentId=1000&opened=1");

            Console.WriteLine(httpResult);

            Console.ReadLine();
        }
    }
}
