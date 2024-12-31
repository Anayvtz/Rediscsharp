using System;

namespace rediscsharp
{
    const string _DELIM = "\r\n";
    enum RespId : char { STRING='+',ERR='-',INT=':',BULK='$',ARR='*'};
    public class Srlz
    {
        public Srlz()
        {
        }
    }
}

