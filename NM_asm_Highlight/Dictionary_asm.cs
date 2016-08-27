using System.Collections.Generic;

namespace NM_asm_highlight
{
    class Dictionary_asm
    {
        #region Keywords
        public static List<string> Keywords = new List<string>(new string[]
        {
            "activate",
            "addr",
            "and",
            "branch",
            "call",
            "callrel",
            "carry",
            "cfalse",
            "clear",
            "code",
            "common",
            "const",
            "ctrue",
            "data",
            "delayed",
            "double",
            "dup",
            "endif",
            "else",
            "false",
            "flag",
            "float",
            "ftw",
            "goto",
            "hiword",
            "if",
            "ireturn",
            "loword",
            "mask",
            "nobits",
            "noflags",
            "not",
            "nul",
            "offset",
            "own",
            "pop",
            "push",
            "ref",
            "rep",
            "return",
            "true",
            "shift",
            "sizeof",
            "skip",
            "store",
            "string",
            "vfalse",
            "vnul",
            "vregs",
            "vsum",
            "vtrue",
            "weak",
            "with",
            "wtw",
            "xor",
            "wait",
            "set",
            "or"
        });
        #endregion

        #region Directives 
        public static List<string> Directives = new List<string>(new string[]
        {
            ".align",
            ".branch",
            ".wait",
            ".repeat",
            ".endrepeat"
        });
        #endregion

        #region Labels 
        public static List<string> Labels = new List<string>(new string[]
        {
            "begin",
            "local",
            "global",
            "import",
            "from",
            "struct",
            "end",
            "label",
            "macro",
            "extern",
        });
        #endregion

        #region Data_registers 
        public static List<string> Data_registers = new List<string>(new string[]
        {
            "ar0",
            "ar1",
            "ar2",
            "ar3",
            "ar4",
            "ar5",
            "ar6",
            "ar7",
            "gr0",
            "gr1",
            "gr2",
            "gr3",
            "gr4",
            "gr5",
            "gr6",
            "gr7",
            "pc",
            "pswr",
            "intr",
            "f1cr",
            "f1crh",
            "f1crl",
            "f2cr",
            "f2crh",
            "f2crl",
            "vr",
            "vrh",
            "vrl",
            "nb1",
            "nb1h",
            "nb1l",
            "sb",
            "sbh",
            "sbl",
            "sp",
            "gmicr",
            "lmicr",
            "oca0",
            "oca1",
            "ica0",
            "ica1",
            "occ0",
            "occ1",
            "icc0",
            "icc1",
            "dor0",
            "dor1",
            "dir0",
            "dir1",
            "t0",
            "t1",
            "wfifo",
            "afifo",
            "ram",
            "word",
            "long"
        });
        #endregion

        #region Auxiliary_symbols 
        public static char[] auxiliary_symbols = new char[]
        {
            '(',
            ')',
            ',',
            ';',
            '+',
            '-',
            '[',
            ']',
            '*',
            '='
        };
        #endregion

        #region assisting_methods
        public static string RemoveAux(string src, ref int start, ref int finish)
        {
            string dst = src;
            foreach (var el in auxiliary_symbols)
            {
                dst = dst.Replace(el, ' ');
            }
            start = 0; finish = 0;
            for (int i = 0; i < dst.Length; ++i)
            {
                if (dst[i] != ' ' && dst[i] != '\t')
                {
                    start = i;
                    for (finish = start; finish < dst.Length; ++finish)
                    {
                        if (dst[finish] == ' ')
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            dst = dst.Trim();
            return dst;
        }

        public static bool IsComment(string src)
        {
            if (src.Length < 2)
            {
                return false;
            }
            else if (src[0] == '/' && src[1] == '/')
            {
                return true;
            }
            
            return false;
        }

        public static bool IsQuoted(string src)
        {
            if (src.Length < 2)
            {
                return false;
            }
            else if (src[0] == '"' && src[src.Length - 1] == '"')
            {
                return true;
            }

            return false;
        }

        public static bool IsNumber(string src)
        { 
            long number = 0;
            bool res = long.TryParse(src, out number);
            if (res)
                return true;

            if (src.Length < 2)
                return false;

            string tmp = src.Substring(src.Length - 1);
            if (tmp.ToLower() == "h")
            {
                res = long.TryParse(src.Substring(0, src.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out number);
            }
            if (res)
                return true;

            if (src.Length < 3)
                return false;

            tmp = src.Substring(src.Length - 2);
            if (tmp.ToLower() == "hl")
            {
                res = long.TryParse(src.Substring(0, src.Length - 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out number);
            }
            if (res)
                return true;

            tmp = src.Substring(0, 2);
            if (tmp.ToLower() == "0x")
            {
                res = long.TryParse(src.Substring(2, src.Length - 2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out number);
            }
            if (res)
                return true;

            return false;
        }

        public static bool IsCustomLabel(ref string src)
        {
            if (src.Length <= 2)
            {
                return false;
            }
            if(src[0] == '<' && src[src.Length-1] == '>')
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
