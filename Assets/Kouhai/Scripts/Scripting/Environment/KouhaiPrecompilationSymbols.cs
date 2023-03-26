namespace Kouhai.Scripting.Interpretter
{
    public static class KouhaiPrecompilationSymbols
    {
        public const string KOHAI_PRECOMP_HEADER = "\n-- Added by kouhai pre-compiler --\n";
        
        public const string VARIABLE_START = "::vars::";
        public const string VARIABLE_END = "::endvars::";
        
        public const string SCRIPT_END = "::scriptend::";
    }
}