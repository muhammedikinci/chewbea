using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace chewbea
{
    class Extras
    {
        string sourceFilePath = "C:\\xampp\\htdocs\\laravel\\Insider\\0026_Koctas";
        string sourceFileName = "koctas.js";
        string destFileName = "resss.js";
        int CurrentTabCount = 0;
        string[] Lines;
        List<String> WritingLines = new List<String>();
        string CurrentFunction = "";

        public void meyin()
        {
            Console.ReadKey();

            Lines = File.ReadAllLines(sourceFilePath + "\\" + sourceFileName);

            for (int i = 0; i < Lines.Length - 1; i++)
            {
                var currentLine = Lines[i];

                if (currentLine.Contains(": function() {"))
                {
                    CurrentFunction = "/* function -> " + currentLine.Split(":")[0].Trim() + " */";
                }

                if (currentLine.Contains("var funcString = \""))
                {
                    WritingLines.Add("");
                    WritingLines.Add("");
                    WritingLines.Add("/******************************************/");
                    WritingLines.Add(CurrentFunction);
                    WritingLines.Add("");

                    currentLine = currentLine.Trim().Substring(19);
                    currentLine = currentLine.Substring(0, currentLine.Length - 2);

                    while (currentLine.Contains("\\n") || currentLine.Contains(@"\\\") || currentLine.Contains("  "))
                    {
                        currentLine = currentLine.Replace("\\n", "\r\n");
                        currentLine = currentLine.Replace(@"\\\", "");
                        currentLine = currentLine.Replace("(\\", "(");
                        currentLine = currentLine.Replace("\\\")", "\")");
                        currentLine = currentLine.Replace("\\\",", "\",");
                        currentLine = currentLine.Replace("\\\" ,", "\" ,");
                        currentLine = currentLine.Replace(",\\\"", ",\"");
                        currentLine = currentLine.Replace(", \\\"", ", \"");
                        currentLine = currentLine.Replace("=\\\"", "= \"");
                        currentLine = currentLine.Replace("\\\";", "\";");
                        currentLine = currentLine.Replace("[\\\";", "[\"");
                        currentLine = currentLine.Replace("\\\"];", "\"]");
                        currentLine = currentLine.Replace("&&", " && ");
                        currentLine = currentLine.Replace("||", " || ");

                        if (!currentLine.Contains("'"))
                        {
                            currentLine = currentLine.Replace("\\\"", "\"");
                        }

                        currentLine = currentLine.Replace("  ", " ");
                    }

                    string newLine = "";
                    bool lineFull = false;

                    for (int k = 0; k < currentLine.Length - 1; k++)
                    {

                        bool tabNow = false;
                        string prefix = "";
                        for (int m = 0; m < CurrentTabCount; m++)
                        {
                            prefix += "\t";
                        }
                        var currentChar = currentLine[k].ToString();

                        if (currentChar == ";" && currentLine[k - 1] == ')' && currentLine[k - 2] == '}')
                        {
                            lineFull = true;
                            tabNow = true;
                            CurrentTabCount--;
                        }
                        else if (currentChar == ";")
                        {
                            lineFull = true;
                        }
                        else if (currentChar == "}" && currentLine[k + 1] == ',')
                        {
                            CurrentTabCount--;
                            currentChar = currentChar + ",";
                            k += 1;
                            lineFull = true;
                        }
                        else if (currentChar.EndsWith("}") && currentLine[k + 1] != ';' && currentLine[k + 1] != ')')
                        {
                            CurrentTabCount--;
                            lineFull = true;
                        }
                        else if (currentChar == "{" && currentLine[k + 1] != '}')
                        {
                            CurrentTabCount++;
                            tabNow = true;
                            lineFull = true;
                        }

                        newLine += currentChar;
                        if (lineFull)
                        {

                            newLine = newLine.Replace("!=", " != ");
                            newLine = newLine.Replace("&&", " && ");
                            newLine = newLine.Replace("||", " || ");
                            newLine = newLine.Replace("if(", "if (");
                            newLine = newLine.Replace("(){", " () { ");
                            newLine = newLine.Replace("+", " + ");
                            newLine = newLine.Replace(":", ": ");
                            newLine = newLine.Replace(")\n", ");\n");
                            newLine = newLine.Replace("\n", "");
                            newLine = newLine.Replace("\r", "");
                            newLine = newLine.Replace(">", " > ");
                            newLine = newLine.Replace("<", " < ");
                            newLine = newLine.Replace("===", " === ");
                            newLine = newLine.Replace("!==", " !== ");
                            newLine = newLine.Replace(">=", " <= ");

                            while (newLine.Contains("  ") || newLine.Contains(" ;"))
                            {
                                newLine = newLine.Replace("  ", " ");
                                newLine = newLine.Replace(" ;", ";");
                            }

                            if (tabNow)
                            {
                                for (int m = 0; m < CurrentTabCount; m++)
                                {
                                    newLine = "\t" + newLine;
                                }
                                WritingLines.Add(newLine);
                            }
                            else
                            {
                                WritingLines.Add(prefix + newLine);

                            }
                            newLine = "";
                            lineFull = false;
                        }
                    }
                    WritingLines.Add("}");
                    CurrentTabCount = 0;
                }
            }

            File.WriteAllLines(sourceFilePath + "\\" + destFileName, WritingLines.ToArray(), Encoding.UTF8);
        }
    }
}
