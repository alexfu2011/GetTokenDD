using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetTokenDD
{
    public class Token
    {
        public bool success;
        public Content content;
    }

    public class Content
    {
        public Data data;
    }

    public class Data
    {
        public int expiresIn;
        public string accessToken;
    }
}
