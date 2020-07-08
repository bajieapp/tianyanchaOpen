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
            //HttpClientHelper.Host = "https://open.tianyancha.com";

            //var httpResult = HttpClientHelper.Get("/cloud-open-admin/_feign/interface/find.json?parentId=1000&opened=1");

            //dynamic json = JObject.Parse(httpResult);

            //Console.WriteLine(json.state);

            //foreach (var str in json.data.items)
            //{
            //    Console.WriteLine(str.id);
            //    Console.WriteLine(str.fname);
            //    Console.WriteLine(str.furl);
            //    Console.WriteLine(str.fdesc);

            //    var pars = JObject.Parse(str.requestParam.Value);

            //    foreach (var p in pars.Properties())
            //    {
            //        var a = JObject.Parse(p.Value.ToString());
            //        Console.WriteLine(a.name);
            //        Console.WriteLine(a.remark);
            //        Console.WriteLine(a.require);
            //        Console.WriteLine(a.type);
            //    }

            //    Console.WriteLine("---------------------------------------------");
            //}


            Core core = new Core();
            core.LoadRoot().LoadParent();

           var result= core.RootDTOs;

            Console.ReadLine();
        }
    }
}
