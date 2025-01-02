using System;
using System.ComponentModel.DataAnnotations;

namespace rediscsharp
{
    class Convention
    {
        public const string _DELIM = "\r\n";
    }
    
    enum RespId  { STR='+',ERR='-',INT=':',BULK='$',ARR='*'};
    public class Srlz
    {
        public Srlz()
        {
        }
        public static string srlz_simple_str(string str)
        {
            string firstCh = ((char)RespId.STR).ToString();
            return firstCh + str + Convention._DELIM;
            
        }
        public static string srlz_error(string err)
        {
            string firstCh = ((char)RespId.ERR).ToString();
            return firstCh + err + Convention._DELIM;
        }

        public static string srlz_integer(int val) 
        {
            string firstCh = ((char)RespId.INT).ToString();
            return firstCh + val.ToString() + Convention._DELIM;
        }

        public static string srlz_bulk(string str)
        {
            string firstCh = ((char)RespId.BULK).ToString();
            int len = str.Length;
            return firstCh + len.ToString() + Convention._DELIM + str + Convention._DELIM;
        }
        public static string srlz_arr(List<string> list)
        {
            string firstCh = ((char)RespId.ARR).ToString();
            string output = firstCh + list.Count.ToString() + Convention._DELIM;
            foreach (string str in list)
            {
                if (str.Length == 0) continue;
                if (int.TryParse(str, out int val))
                {
                    output += srlz_integer(val);
                }
                else
                {
                    output += srlz_bulk(str);
                }
            }
            return output;
        }
    }
    public class Dsrlz 
    {
        public Dsrlz()
        {
        }

        public static (string?,string) dsrlz_str(string str)
        {
            switch(str[0])
            {
                case (char)RespId.STR:
                    return Dsrlz.dsrlz_simple_str(str);

                case (char)RespId.ERR:
                    return Dsrlz.dsrlz_error(str);

                case (char)RespId.BULK:
                    return Dsrlz.dsrlz_bulk(str);

                default:
                    break;
            }
            return (null,str);
        }

        public static (string?,string) dsrlz_int(string str)
        {
            if (str[0] != (char)RespId.INT) { return (null,str); }
            int ix = str.IndexOf(Convention._DELIM);
            if (ix == -1) { return (null,str); }
            string num = str.Substring(1, ix - 1);
            string remain = str.Substring(ix + Convention._DELIM.Length);
            return (num,remain);
        }

        public static (List<string>?,string) dsrlz_arr(string str)
        {
            if (str[0] != (char)RespId.ARR) { return (null,str); }
            int aix = str.IndexOf(Convention._DELIM);
            if (aix == -1) { return (null,str); }
            string lenstr = str.Substring(1, aix - 1);
            int alen = 0;
            int.TryParse(lenstr, out alen);
            if (alen <= 0) { return (null,str); }
            string remain = str.Substring(aix + Convention._DELIM.Length);
            List<string> output = new List<string>();
            for (int i=0; i < alen; i++)
            {
                (string? number, string remaini) = dsrlz_int(remain);
                if (number != null) { remain = remaini; output.Add(number); continue; }
                else
                {
                    (string? strs, string remains) = dsrlz_str(remain);
                    remain = remains;
                    if (strs != null)
                    {
                        output.Add(strs); continue;
                    }
                    
                }

            }
            return (output, remain);
        }
        public static (string?,string) dsrlz_simple_str(string str)
        {
            int ix = str.IndexOf(Convention._DELIM);
            string simple = str.Substring(1, ix - 1);
            string remain = str.Substring(ix + Convention._DELIM.Length);
            return (simple,remain);
        }
        public static (string?,string) dsrlz_error(string str)
        {
            int ix = str.IndexOf(Convention._DELIM);
            string err = str.Substring(1, ix - 1);
            string remain = str.Substring(ix + Convention._DELIM.Length);
            return (err, remain);
        }
        public static (string?,string) dsrlz_bulk(string str)
        {
            int len = 0;
            int delimIx = str.IndexOf(Convention._DELIM);
            if (delimIx == -1 || delimIx <= 1)
            {
                return (null,str);
            }
            
            string lenstr = str.Substring(1, delimIx - 1);
            int.TryParse(lenstr,out len);
            string bulk = str.Substring(delimIx + Convention._DELIM.Length,len);
            string remain = str.Substring(delimIx + Convention._DELIM.Length + len + Convention._DELIM.Length);
            return (bulk,remain);
            
        }
    }
}

