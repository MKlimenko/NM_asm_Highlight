﻿using System.Collections.Generic;

namespace NM_asm_highlight
{
    static class Dictionary_asm
    {
        #region Keywords
        public static readonly List<string> Keywords = new List<string>(new[]
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
            "noflags",
            "not",
            "nul",
            "offset",
            "own",
            "pop",
            "push",
            "ref",
            "rep",
            "reserve",
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
        public static readonly List<string> Directives = new List<string>(new[]
        {
            ".align",
            ".branch",
            ".if",
            ".endif",
            ".wait",
            ".repeat",
            ".endrepeat"
        });
        #endregion

        #region Labels 
        public static readonly List<string> Labels = new List<string>(new[]
        {
            "begin",
            "data",
            "end",
            "extern",
            "from",
            "global",
            "import",
            "label",
            "local",
            "macro",
            "nobits",
            "struct"
        });
        #endregion

        #region Data_registers 
        public static readonly List<string> Data_registers = new List<string>(new[]
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

        private static readonly char[] auxiliary_symbols = new[]
        {
            '(',
            ')',
            '<',
            '>',
            ',',
            ';',
            ':',
            '+',
            '-',
            '[',
            ']',
            '*',
            '=',
            '\t'
        };
        #endregion

        #region assisting_methods
        public static string RemoveAux(string src, out int start, out int finish)
        {
            var dst = src;
            foreach (var el in auxiliary_symbols)
            {
                dst = dst.Replace(el, ' ');
            }
            start = 0; finish = 0;


            var multiple = dst.Trim().Split(' ');
            if(multiple.Length == 1)
            {
                var tmp = src.Replace('\t', ' ').Trim();
                if (tmp.Length > 2)
                {
                    if (tmp[0] == '<' && tmp[tmp.Length - 1] == '>')
                    {
                        return tmp; // check for label
                    }
                }
            }
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
            var tmp_str = src.Replace('\t', ' ').Trim();
            if (tmp_str.Length < 2)
            {
                return false;
            }
            else if (tmp_str[0] == '/' && tmp_str[1] == '/')
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
            long number;
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

        public static bool IsCustomLabel(string src)
        {
            var tmp = src.Replace('\t', ' ').Trim();
            if (tmp.Length <= 2)
            {
                return false;
            }
            if(tmp[0] == '<' && tmp[tmp.Length-1] == '>')
            {
                return true;
            }
            return false;
        }

        public static List<string> Macros = new List<string>();
        public static List<string> CustomLabel = new List<string>();

        public static void AddCustomLine(string src_Line)
        {
            var words = src_Line.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                return;
            }

            if (words[0].ToLower() == "macro")
            {
                var getmacro = words[1].Replace('(', ' ').Split(' ');
                if (!Macros.Contains(getmacro[0].ToLower()))
                {
                    Macros.Add(getmacro[0].ToLower());
                }
            }
            //else if (words.Length == 1)
            //{
            //    var curr_label = words[0];
            //    if (curr_label.Length > 2)
            //    {
            //        if (curr_label[0] == '<' && curr_label[curr_label.Length - 1] == '>')
            //        {
            //            curr_label = curr_label.Substring(1, curr_label.Length - 2).ToLower();
            //            if (!CustomLabel.Contains(curr_label))
            //            {
            //                CustomLabel.Add(curr_label);
            //            }
            //        }
            //    }
            //}
        }

        public static bool IsMacro(string src, out int size)
        {
            var tmp = src.Replace('(', ' ').Split(' ');
            if (Macros.Contains(tmp[0].ToLower())){
                size = tmp[0].Length;
                return true;
            }
            size = 0;
            return false;
        }
        #endregion
    }
}
