using System;
using System.IO;
using System.Text;

namespace chewbea
{
    static class Program1
    {
        static int CurrentTabCount = 0;
        static string sourceFile;
        static string destPath;
        static string funcString = "";
        static void Mains(string[] args)
        {

            if (args.Length >= 1)
            {
                try
                {
                    sourceFile = args[0];
                    destPath = args[1];
                }
                catch (Exception)
                {
                    goto fn;
                }
            }

            if (sourceFile != null && destPath != null)
            {
                funcString = File.ReadAllText(sourceFile);
            }
            else
            {
                goto fn;
            }

            string relatedString = "";

            for (int i = 0; i < funcString.Length - 1; i++)
            {
                string currentChar = funcString[i].ToString();

                if (currentChar == "{" && funcString[i + 1] != '}')
                {
                    currentChar = " " + currentChar + "\n";
                    CurrentTabCount++;
                }

                if (currentChar == ";")
                {
                    currentChar = currentChar + "\n";
                    if (funcString[i + 1] == '}')
                    {
                        CurrentTabCount--;
                    }
                }

                if (currentChar == "," && funcString[i - 1] == '}')
                {
                    currentChar = currentChar + "\n\n";
                }
                else if (currentChar == "," && funcString[i - 1] == '"' && funcString[i + 1] == '"' && funcString[i - 2] != '(')
                {
                    currentChar = currentChar + " ";
                }

                if (currentChar == "f" && funcString[i - 1] == ':')
                {
                    currentChar = " " + currentChar;
                }

                if (currentChar == "}" && funcString[i - 1] == '}')
                {
                    currentChar = "\n" + currentChar;
                }
                else if (currentChar == "}" && funcString[i + 1] != ')')
                {
                    currentChar = currentChar + " ";
                    CurrentTabCount--;
                }

                if (currentChar == "|" && funcString[i + 1] == '|')
                {
                    currentChar = " " + currentChar;
                }
                else if (currentChar == "|" && funcString[i - 1] == '|')
                {
                    currentChar = currentChar + " ";
                }

                for (int j = 0; j < CurrentTabCount; j++)
                {
                    if (currentChar.Contains("{") || currentChar.Contains(";") || currentChar.Contains("}\n"))
                    {
                        currentChar = currentChar + "\t";
                    }
                }
 
                if (currentChar != "\n")
                {
                    relatedString += currentChar;
                }
            }

            using (FileStream file = File.Create(destPath+"\\result.js"))
            {
                file.Write(Encoding.UTF8.GetBytes(relatedString),0, relatedString.Length);
            }

            fn:
            Console.WriteLine("False arguments");
            Console.ReadKey();
        }
    }
}
