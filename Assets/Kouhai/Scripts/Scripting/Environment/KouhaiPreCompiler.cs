using UnityEngine;
using System.Text.RegularExpressions;

namespace Kouhai.Scripting.Interpretter
{
    public static class KouhaiPreCompiler
    {
        public static string PreCompile(string luaCode)
        {
            return TransformSourceCode(luaCode);
        }

        private static string TransformSourceCode(string input)
        {
            var transformed = input;
            //attach script end tag to script as pre-transformation process
            transformed += $"{KouhaiPrecompilationSymbols.KOHAI_PRECOMP_HEADER}" +
                           $"{KouhaiPrecompilationSymbols.SCRIPT_END}";
            transformed = KouhaiTransformations.AddKouhaiVariableJumpers(transformed);
            transformed = CleanEmptySpaces(transformed);
            return transformed;
        }

        private static string CleanEmptySpaces(string input)
        {
            return Regex.Replace(input, @"^\s*$[\r\n]*", string.Empty, RegexOptions.Multiline);
        }
    }
}