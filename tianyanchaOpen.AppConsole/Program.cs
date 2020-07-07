using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.Text.Json;

namespace tianyanchaOpen.AppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClientHelper.Host = "https://open.tianyancha.com";

            var httpResult = HttpClientHelper.Get("/cloud-open-admin/_feign/interface/find.json?parentId=1000&opened=1");

            dynamic json = JObject.Parse(httpResult);

            Console.WriteLine(json.state);

            foreach (var str in json.data.items)
            {
                Console.WriteLine(str.id);
                Console.WriteLine(str.fname);
                Console.WriteLine(str.furl);
                Console.WriteLine(str.fdesc);
            }

            Console.ReadLine();
        }
    }
}
