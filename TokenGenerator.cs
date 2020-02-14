using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    struct Lexemes
    {
       public  int lineNo;
       public  string lexemes;
    }
    class TokenGenerator
    {
        
        static void AddTempToList(List<string> list, ref string temp,int lineNo,List<Lexemes> listLexemes)
        {
            if (temp.CompareTo("") != 0)
            {
                Lexemes lexeme=new Lexemes();
                lexeme.lexemes=temp;
                lexeme.lineNo=lineNo;
                listLexemes.Add(lexeme);
                list.Add(temp);
                temp = "";
            }
        }
        public static List<Lexemes> splitFunc(string[] lines)
        {
         List<Lexemes> lexemesList = new List<Lexemes>();
            int l = 0;
            bool isMultiLineComment = false;
            List<char> num0_9 = new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            List<char> logicalAndOr = new List<char>() { '&', '|' };
            bool isString = false;
            string temp = "";
            List<string> tempList = new List<string>();
            while (l < lines.Length)
            {

                #region Loop Logic
                string code = lines[l];

                for (int i = 0; i < code.Length; i++)
                {


                    //code for character
                    #region
                    if ((i < code.Length - 4) && code[i].CompareTo('\'') == 0 && code[i + 1].CompareTo('\\') == 0 && code[i + 3].CompareTo('\'') == 0)
                    {
                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                        temp = temp + code[i] + code[i + 1] + code[i + 2] + code[i + 3];
                        i = i + 4;
                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                     
                    }
                    else if ((i < code.Length - 3) && code[i].CompareTo('\'') == 0 && code[i + 1].CompareTo('\\') == 0 && code[i + 3].CompareTo('\'') == 0)
                    {
                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                        temp = temp + code[i] + code[i + 1] + code[i + 2] + code[i + 3];
                        i = i + 4;
                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                    }
                    #endregion
                    ///////////code for multiline comments
                    #region MultiLineComments
                    else if ((i < code.Length - 1) && code[i].CompareTo('/') == 0 && code[i + 1].CompareTo('*') == 0 || isMultiLineComment)
                    {
                        isMultiLineComment = true;
                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                        if (code[i].CompareTo('*') == 0 && code[i + 1].CompareTo('/') == 0)
                        {
                            isMultiLineComment = false;
                            i = i + 1;
                        }
                    }
                    #endregion
                    #region Comments
                    //code for the comments
                    else if (code[i].CompareTo('/') == 0 && code[i + 1].CompareTo('/') == 0)
                    {
                        AddTempToList(tempList, ref temp, l+1, lexemesList);

                        i = code.Length;
                    }
                    #endregion
                    #region CodeforAndOr
                    //code for logical operators
                    else if (logicalAndOr.Contains(code[i]))
                    {
                        if (code[i].CompareTo(code[i + 1]) == 0)
                        {
                            AddTempToList(tempList, ref temp,l+1,lexemesList);
                            temp = temp + code[i] + code[i + 1];
                            i = i + 1;
                            AddTempToList(tempList, ref temp,l+1,lexemesList);

                        }
                        else
                        {
                            AddTempToList(tempList, ref temp,l,lexemesList);
                            temp = temp + code[i];
                            AddTempToList(tempList, ref temp,l,lexemesList);
                        }
                    }
                    #endregion
                    //Code for string
                    #region string
                    else if (code[i].CompareTo('"') == 0 || isString)
                    {
                        if (code[i].CompareTo('\\') == 0 && code[i + 1].CompareTo('"') == 0)
                        {
                            temp = temp + code[i + 1];
                            i = i + 1;
                        }
                        else if (i == code.Length - 1)
                        {
                            temp = temp + code[i];
                            AddTempToList(tempList, ref temp, l+1, lexemesList);
                            isString = false;
                        }
                        else if (temp.Contains('"') && code[i].CompareTo('"') == 0)
                        {

                            temp = temp + code[i];
                            isString = false;
                            AddTempToList(tempList, ref temp, l+1, lexemesList);
                        }

                        else
                        {
                            isString = true;
                            temp = temp + code[i];

                        }
                    }
                    #endregion
                    #region All operators
                    //  //code for all operators optimized
                    else if (code[i].CompareTo('!') == 0 || code[i].CompareTo('+') == 0 || code[i].CompareTo('-') == 0 || code[i].CompareTo('=') == 0 || code[i].CompareTo('/') == 0 || code[i].CompareTo('*') == 0 || code[i].CompareTo('%') == 0 || code[i].CompareTo('<') == 0 || code[i].CompareTo('>') == 0)
                    {
                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                        if (code[i].CompareTo('=') == 0)
                        {
                            if (code[i + 1].CompareTo('=') == 0)
                            {
                                temp = temp + code[i] + code[i + 1];
                                AddTempToList(tempList, ref temp, l+1, lexemesList);
                                i = i + 1;
                            }
                            else
                            {
                                temp = temp + code[i].ToString();

                                AddTempToList(tempList, ref temp, l+1, lexemesList);
                            }
                        }
                        else if (code[i].CompareTo('+') == 0 || code[i].CompareTo('-') == 0)
                        {
                            if (code[i - 1].CompareTo('=') == 0 || code[i - 1].CompareTo(' ') == 0)
                            {
                                temp = temp + code[i];
                            }
                            else
                            {
                                if (code[i + 1].CompareTo('=') == 0 || code[i + 1].CompareTo(code[i]) == 0)
                                {
                                    temp = temp + code[i] + code[i + 1];

                                    AddTempToList(tempList, ref temp, l+1, lexemesList);
                                    i = i + 1;
                                    
                                }
                                else
                                {
                                    temp = temp + code[i].ToString();

                                    AddTempToList(tempList, ref temp, l+1, lexemesList);
                                }
                            }
                        }
                        else
                        {
                            if (code[i + 1].CompareTo('=') == 0)
                            {
                                temp = temp + code[i] + code[i + 1];

                                AddTempToList(tempList, ref temp, l+1, lexemesList);
                                i = i + 1;
                            }
                            else
                            {
                                temp = temp + code[i].ToString();

                                AddTempToList(tempList, ref temp, l+1, lexemesList);
                            }
                        }


                    }
                    //Code for operators and the comparision operators such as <= >= < > and != 
                    else if (code[i].CompareTo('!') == 0 || code[i].CompareTo('+') == 0 || code[i].CompareTo('-') == 0 || code[i].CompareTo('=') == 0 || code[i].CompareTo('/') == 0 || code[i].CompareTo('*') == 0 || code[i].CompareTo('<') == 0 || code[i].CompareTo('>') == 0)
                    {

                        if (temp.CompareTo("") != 0 && !temp.Contains("+") && !temp.Contains("=") && !temp.Contains("-") && !temp.Contains("*")
                            && !temp.Contains("/") && !temp.Contains("<") && !temp.Contains(">") && !temp.Contains("!"))
                        {
                            AddTempToList(tempList, ref temp, l, lexemesList);
                            temp = temp + code[i];
                        }

                        else if (temp.Length > 0)
                        {
                            if (temp[temp.Length - 1].CompareTo(code[i]) == 0 && (code[i].CompareTo('+') == 0 || code[i].CompareTo('-') == 0 || code[i].CompareTo('=') == 0 || code[i].CompareTo('*') == 0 || code[i].CompareTo('<') == 0 || code[i].CompareTo('>') == 0))
                            {
                                temp = temp + code[i];
                                AddTempToList(tempList, ref temp, l, lexemesList);
                            }
                            else if (temp[temp.Length - 1].CompareTo('=') == 0)
                            {

                                AddTempToList(tempList, ref temp, l, lexemesList);
                                temp = temp + code[i];
                            }
                            else if (temp[temp.Length - 1].CompareTo('+') == 0 || temp[temp.Length - 1].CompareTo('-') == 0 || temp[temp.Length - 1].CompareTo('/') == 0 || temp[temp.Length - 1].CompareTo('*') == 0 || temp[temp.Length - 1].CompareTo('<') == 0 || temp[temp.Length - 1].CompareTo('>') == 0 || temp[temp.Length - 1].CompareTo('!') == 0)
                            {
                                if (code[i].CompareTo('=') == 0)
                                {
                                    temp = temp + code[i];

                                    AddTempToList(tempList, ref temp, l, lexemesList);
                                }
                            }
                        }
                        else
                        {
                            temp = temp + code[i];
                        }
                    }
                    #endregion
                    #region forDot
                    //code for .
                    else if (code[i].CompareTo('.') == 0)
                    {
                        int a;
                        if (num0_9.Contains(code[i + 1]) && !temp.Contains("."))
                        {
                            if (temp[0].CompareTo('+') != 0 && temp[0].CompareTo('-') != 0 && !num0_9.Contains(code[i]))
                            {
                                if (temp.CompareTo("") != 0 && !int.TryParse(temp, out a))
                                {
                                    AddTempToList(tempList, ref temp, l+1, lexemesList);
                                }


                                temp = temp + code[i];
                            }
                            else
                                temp = temp + code[i];
                        }

                        else
                        {


                            AddTempToList(tempList, ref temp, l+1, lexemesList);
                            temp = temp + code[i].ToString();
                            AddTempToList(tempList, ref temp, l+1, lexemesList);
                        }
                    }
                    #endregion
                    //Code for Punctuators operators apart from dot
                    #region forPunctuators
                    else if (code[i].CompareTo('(') == 0 || code[i].CompareTo(':') == 0 || code[i].CompareTo(')') == 0 || code[i].CompareTo(';') == 0 || code[i].CompareTo('{') == 0 || code[i].CompareTo('}') == 0 || code[i].CompareTo(',') == 0 || code[i].CompareTo('^') == 0 || code[i].CompareTo('|') == 0 || code[i].CompareTo('&') == 0)
                    {


                        AddTempToList(tempList, ref temp, l+1, lexemesList);

                        temp=temp+code[i].ToString();

                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                    }
                    #endregion
                    #region Spaces and end Of string
                    //Code for end of the string and spaces
                    else if (code[i].CompareTo(' ') == 0)
                    {

                        AddTempToList(tempList, ref temp, l+1, lexemesList);
                        
                    }
                    else if (i==code.Length-1)
                    {
                        temp = temp + code[i];
                        AddTempToList(tempList, ref temp, l+1, lexemesList);

                    }

                    #endregion
                    //If non of the above conditions matched then it will run :)
                    #region
                    else
                    {
                        temp = temp + code[i];
                    }
                    #endregion
                }
                #endregion
                l++;

            }
            return lexemesList;
        }
    }
}
