using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF2BossEditor.Core
{
    public class CommonFunctions
    {
        public static bool IsStringInFormat(string Input, string Format = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (Input == null || Format == null)
                return false;
            string inputLower = Input.ToLower();
            string formatLower = Format.ToLower();

            for (int i = 0; i < inputLower.Length; i++)
            {
                bool hasChar = false;
                for (int o = 0; o < formatLower.Length; o++)
                {
                    if (inputLower[i] == formatLower[o])
                    {
                        hasChar = true;
                        break;
                    }
                }

                if (!hasChar)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
