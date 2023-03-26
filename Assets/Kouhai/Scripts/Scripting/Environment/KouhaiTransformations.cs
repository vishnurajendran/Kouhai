using System.Linq;
using UnityEngine;

namespace Kouhai.Scripting.Interpretter
{
    public static class KouhaiTransformations
    {
        public static string AddKouhaiVariableJumpers(string source)
        {
            bool hasStart = source.Contains(KouhaiPrecompilationSymbols.VARIABLE_START);
            bool hasEnd = source.Contains(KouhaiPrecompilationSymbols.VARIABLE_END);
            
            var startInstances = source.Split(KouhaiPrecompilationSymbols.VARIABLE_START);
            var endInstances = source.Split(KouhaiPrecompilationSymbols.VARIABLE_END);

            if (hasStart)
                hasStart = !source.Contains($"--{KouhaiPrecompilationSymbols.VARIABLE_START}");

            if (hasEnd)
                hasEnd = !source.Contains($"--{KouhaiPrecompilationSymbols.VARIABLE_END}");

            if (!hasStart && !hasEnd)
            {
                source = $"{KouhaiPrecompilationSymbols.KOHAI_PRECOMP_HEADER}\n" +
                         $"{KouhaiPrecompilationSymbols.VARIABLE_START}\n" +
                         $"-- Added by kouhai pre-compiler\n" +
                         $"{KouhaiPrecompilationSymbols.VARIABLE_END}\n" +
                         $"\n{source}\n";

                hasEnd = hasStart = true;
                startInstances = new[] { "a", "b" };
                endInstances = new[] { "a", "b" };
            }
            
            if ((hasStart && !hasEnd) || (!hasStart && hasEnd))
            {
                Debug.LogError("Kouhai script variable declarations must end with a closing tag");
                return string.Empty;
            }
            
            if (startInstances.Length -1 > 1 || endInstances.Length -1 > 1)
            {
                Debug.LogError("Kouhai script has too many variable declarations symbols detected, maske sure to have only one declaration block");
                return string.Empty;
            }
            
            source = JumpToVariableBlock(source);
            source = JumpToEndAfterVariableRead(source);
            return source.TrimStart().TrimEnd();
        }

        private static string JumpToVariableBlock(string input)
        {
            var code =
                        $"{KouhaiPrecompilationSymbols.KOHAI_PRECOMP_HEADER}" + 
                        $"if not {KouhaiScriptEnvVars.ENV_PLAYING} then" + 
                        $"\n\tgoto {GetTagName(KouhaiPrecompilationSymbols.VARIABLE_START)}" +
                        "\nend\n\n";
                   
            return $"{code}" + 
                   $"{input}";
        }
        
        private static string JumpToEndAfterVariableRead(string input)
        {
            var code = 
                       $"{KouhaiPrecompilationSymbols.VARIABLE_END}\n"+
                       $"{KouhaiPrecompilationSymbols.KOHAI_PRECOMP_HEADER}" +
                       $"if not {KouhaiScriptEnvVars.ENV_PLAYING} then" +
                       $"\n\tgoto {GetTagName(KouhaiPrecompilationSymbols.SCRIPT_END)}" +
                       "\nend\n\n";

            return input.Replace(KouhaiPrecompilationSymbols.VARIABLE_END, code);
        }

        private static string GetTagName(string tag)
        {
            return tag.Replace("::", "");
        }
    }
}