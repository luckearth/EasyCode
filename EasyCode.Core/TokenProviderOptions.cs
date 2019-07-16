using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCode.Core
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "account/token";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiration { get; set; } = 60;
        public string SecretKey { get; set; }
    }
}
