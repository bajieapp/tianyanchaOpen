using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace tianyanchaOpen.AppConsole
{
    public class Core
    {
        /// <summary>
        /// 主机
        /// </summary>
        private readonly string _Host = @"https://open.tianyancha.com";
        /// <summary>
        /// 根目录
        /// </summary>
        private readonly string _RootUrl = @"/cloud-open-admin/_feign/interface/find/one.json?opened=1";
        /// <summary>
        /// 节点
        /// </summary>
        private readonly string _ParentUrl = @"/cloud-open-admin/_feign/interface/find.json?parentId={0}&opened={1}";

        public List<RootDTO> RootDTOs { get; set; }


        public Core()
        {
            HttpClientHelper.Host = _Host;
            RootDTOs = new List<RootDTO>();
        }

        /// <summary>
        /// 加载根目录
        /// </summary>
        public Core LoadRoot()
        {
            var httpResult = HttpClientHelper.Get(_RootUrl);

            dynamic json = JObject.Parse(httpResult);

            foreach (var root in json.data)
            {
                RootDTOs.Add(new RootDTO
                {
                    id= root.id,
                    opened = root.opened,
                    fname = root.fname,
                });
            }

            return this;
        }

        /// <summary>
        /// 加载节点
        /// </summary>
        public Core LoadParent()
        {
            foreach(var rootDTO in RootDTOs)
            {
                Thread.Sleep(new Random().Next(500,2000));

                rootDTO.ParentDTOs = new List<ParentDTO>();

                string url = string.Format(_ParentUrl, rootDTO.id, rootDTO.opened);
                var httpResult = HttpClientHelper.Get(url);

                dynamic json = JObject.Parse(httpResult);

                foreach (var item in json.data.items)
                {
                    ParentDTO parentDTO = new ParentDTO()
                    {
                        id = item.id,  //
                        parentId = item.parentId,  //
                        parentFName = item.parentFName,  //
                        fname = item.fname,  //
                        openUrl = item.openUrl,  //
                        furl = item.furl,  //
                        fmethod = item.fmethod,  //
                        fdesc = item.fdesc,  //
                    };
                    parentDTO.RequestParamDTOs = new List<RequestParamDTO>();
                    var pars = JObject.Parse(item.requestParam.Value);
                    foreach (var par in pars.Properties())
                    {
                        var value = JObject.Parse(par.Value.ToString());
                        RequestParamDTO requestParamDTO = new RequestParamDTO()
                        {
                            sample = value.sample,  //
                            name = value.name,  //
                            remark = value.remark,  //
                            notice = value.notice,  //
                            require = value.require,  //
                            type = value.type,  //

                        };
                        parentDTO.RequestParamDTOs.Add(requestParamDTO);
                    }
                    rootDTO.ParentDTOs.Add(parentDTO);
                }
            }
            return this;
        }
    }

    public class RootDTO
    {
        public string id { get; set; }
        public string opened { get; set; }
        public string fname { get; set; }
        public List<ParentDTO> ParentDTOs { get; set; }
    }


    public class ParentDTO
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string parentFName { get; set; }
        public string fname { get; set; }
        public string openUrl { get; set; }
        public string furl { get; set; }
        public string fmethod { get; set; }
        public string fdesc { get; set; }
        public List<RequestParamDTO> RequestParamDTOs { get; set; }
    }

    public class RequestParamDTO
    {
        public string sample { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public string notice { get; set; }
        public bool require { get; set; }
        public string type { get; set; }
    }
}
