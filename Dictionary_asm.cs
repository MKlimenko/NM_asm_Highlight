namespace NM_asm_Language
{
    class Dictionary_asm
    {
        public void GetDictionary(string[] Keywords, string[] Directives, string[] Labels, string[] Data_registers)
        {
          /*  Keywords = this.Keywords;
            Directives = this.Directives;
            Labels = this.Labels;
            Data_registers = this.Data_registers;*/
        }

        #region Keywords
        public static string[] Keywords = new string[]
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
       };
        #endregion

        #region Directives 
        public static string[] Directives = new string[]
        {
            ".align",
            ".branch",
            ".wait",
            ".repeat",
            ".endrepeat"
        };
        #endregion

        #region Labels 
        public static string[] Labels = new string[]
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
            "word",
            "long"
        };
        #endregion

        #region Data_registers 
        public static string[] Data_registers = new string[]
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
            "data",
            "ram"
        };
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
            '/',
            '=',
            ':'
        };
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
                if (dst[i] != ' ')
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
        #endregion
    }
}
