using System;

namespace Kouhai.Scripts.Runtime.Utils
{
    public static class StringNotations
    {
        public static string ToFileSize(long val)
        {
            var bytes = val * Math.Pow(1024, 0);
            var kb = val * Math.Pow(1024, 1);
            var mb = val * Math.Pow(1024, 2);
            var gb = val * Math.Pow(1024, 3);

            if (gb > 1)
            {
                return gb.ToString("F2");
            }
            if (mb > 1)
            {
                return mb.ToString("F2");
            }
            if (kb > 1)
            {
                return kb.ToString("F2");
                
            }
            return bytes.ToString("F2");
        } 
    }
}