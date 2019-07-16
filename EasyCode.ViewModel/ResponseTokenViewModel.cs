using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.ViewModel
{
    public class ResponseTokenViewModel
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime createtime { get; set; }
    }
}
