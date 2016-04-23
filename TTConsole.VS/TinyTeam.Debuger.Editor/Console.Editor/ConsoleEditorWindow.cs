using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TinyTeam.Debuger.Editor
{
    /// <summary>
    /// Console Log View Window
    /// @TinyTeam,chiuanwei
    /// </summary>
    public class ConsoleEditorWindow : EditorWindow
    {
        static ConsoleEditorWindow window = null;

        string currentFilePath = string.Empty;
        string currentContent = string.Empty;
        string content = string.Empty;

        Vector2 scroll;

        int currentPage = 1;
        int maxPage = 1;
        int maxNum = 15000;

        int contentIndexBegin = 0; 
        int contentIndexEnd = 0;   

        void Init()
        {
            content = System.IO.File.ReadAllText(currentFilePath);

            //refresh text
            currentContent = RefreshPage();
        }


        public void OnGUI()
        {
            GUI.skin.GetStyle("TextArea").richText = true;
            EditorGUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll,false,true,GUILayout.MinHeight(740));
            EditorGUILayout.TextArea(currentContent, GUI.skin.GetStyle("TextArea"));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            GUI.skin.GetStyle("TextArea").richText = false;
            //page button
            EditorGUILayout.BeginHorizontal();
            {
                int temp = currentPage;
                currentPage = EditorGUILayout.IntField(currentPage, GUILayout.Width(30));
                if (currentPage > maxPage)
                {
                    currentPage = maxPage;
                }
                else if (currentPage < 1)
                {
                    currentPage = 1;
                }

                if (temp != currentPage)
                {
                    //refresh text
                    currentContent = RefreshPage();
                }

                EditorGUILayout.LabelField("/" + maxPage);

                Rect rect = GUILayoutUtility.GetRect(new GUIContent("<back"), EditorStyles.miniButton, GUILayout.Width(200), GUILayout.Height(25));
                if (GUI.Button(rect, "<back", EditorStyles.miniButton))
                {
                    contentIndexEnd = contentIndexBegin;
                    contentIndexBegin = FindBeginMsgIndexFromEnd();

                    if(contentIndexBegin == 0)
                    {
                        contentIndexEnd = FindEndMsgIndexFromBegin();
                    }

                    //refresh text
                    currentContent = RefreshPage();
                }
                rect = GUILayoutUtility.GetRect(new GUIContent("next>"), EditorStyles.miniButton, GUILayout.Width(200), GUILayout.Height(25));
                if (GUI.Button(rect, "next>", EditorStyles.miniButton))
                {
                    contentIndexBegin = contentIndexEnd;
                    contentIndexEnd = FindEndMsgIndexFromBegin();

                    if (contentIndexBegin >= content.Length - 1)
                    {
                        contentIndexBegin = FindBeginMsgIndexFromEnd();
                    }

                    //refresh text
                    currentContent = RefreshPage();
                }
            }
            EditorGUILayout.EndHorizontal();
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                GUILayout.FlexibleSpace();
                Rect rect = GUILayoutUtility.GetRect(new GUIContent("Open a New log file"), EditorStyles.miniButton, GUILayout.ExpandWidth(true), GUILayout.Height(25));
                GUI.backgroundColor = Color.green;
                if (GUI.Button(rect, "Open a New log file", EditorStyles.miniButton))
                {
                    OpenFile();
                }
            }
        }

        string RefreshPage()
        {
            //from
            //int from = FindColorLeftIndex((currentPage - 1) * maxNum);
            //int to = FindColorRightIndex((currentPage) * maxNum);

            int from = contentIndexBegin;
            int to = FindEndMsgIndexFromBegin();

            int length = 1;
            if(to <= content.Length )
            {
                length = to - from;
            }
            else
            {
                length = content.Length - from;
            }

            return content.Substring(from, length);
        }

        /// <summary>
        /// back to search the first <color></color> left index paired.
        /// FIX:no more left like: <color=#7F7F7F><i>
        /// </summary>
        int FindColorLeftIndex(int beginIndex)
        {
            //no more char in the left.
            if(beginIndex == 0)
            {
                return beginIndex;
            }

            while(beginIndex >= 0)
            {
                if(content[beginIndex] == '<' && beginIndex + 6 < content.Length)
                {
                    if(content.Substring(beginIndex+1,6) == "color=")
                    {
                        if (beginIndex - 1 >= 0 && content[beginIndex - 1] == '>' && beginIndex - 3 >= 0 && content.Substring(beginIndex - 3, 3) == "<i>")
                        {
                            //do nothing
                        }
                        else if (beginIndex - 1 >= 0 && content[beginIndex - 1] == '>' && beginIndex - 15 >= 0 && content.Substring(beginIndex - 15, 7) == "<color=")
                        {
                            //do nothing
                        }
                        else
                        {
                            return beginIndex;
                        }
                    }
                }
                beginIndex--;
            }

            return beginIndex;
        }

        /// <summary>
        /// FIX:no more right end like </i></color>
        /// </summary>
        int FindColorRightIndex(int endIndex)
        {
            if(endIndex >= content.Length - 1)
            {
                return content.Length - 1;
            }

            while (endIndex <= content.Length - 1)
            {
                endIndex++;
                if (endIndex - 1 >= 0 && content[endIndex - 1] == '>' && endIndex - 8 >= 0)
                {
                    if (content.Substring(endIndex - 8, 8) == "</color>")
                    {
                        ///</color>|</i></color>
                        ///
                        if (content[endIndex] == '<' && endIndex + 3 < content.Length && content.Substring(endIndex, 4) == "</i>")
                        {
                            //do nothing.
                        }
                        else if (content[endIndex] == '<' && endIndex + 7 < content.Length && content.Substring(endIndex, 8) == "</color>")
                        {
                            //do nothing
                        }
                        else
                        {
                            return endIndex;
                        }
                    }
                }
            }

            return endIndex;
        }

        // found the begin index from end 
        // need less than 65000 chars.
        private int FindBeginMsgIndexFromEnd()
        {
            for (int i = contentIndexEnd; i > 0;)
            {
                int temp = i;

                // get the previous item string..
                i = FindColorLeftIndex(i);

                if (i == temp) return i;

                if (contentIndexEnd - i >= 10000)
                {
                    return i;
                }
            }
            return 0;
        }

        private int FindEndMsgIndexFromBegin()
        {
            for (int i = contentIndexBegin; i < content.Length - 1;)
            {
                int temp = i;

                i = FindColorRightIndex(i);

                if (i == temp) return i;

                if (i - contentIndexBegin >= 10000)
                {
                    return i;
                }
            }

            return content.Length - 1;
        }

        int currentPersent
        {
            get
            {
                if (content.Length == 0)
                {
                    return 0;
                }
                else if (contentIndexEnd == content.Length - 1)
                {
                    return 100;
                }
                return (int)((contentIndexEnd * 1.0f / (content.Length - 1)) * 100);
            }
        }

        #region Editor Control

        [MenuItem("TinyTeam/Console/Open a Log File %l")]
        static void OpenFile()
        {
            string logFolder = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1) + "Log";
            
            if(!Directory.Exists(logFolder))
            {
                Debug.LogError("Cant found '/Log' at Project folder.No Log Exist!");
                return;
            }

            var path = EditorUtility.OpenFilePanel(
            "Choose a Log File",
            logFolder,
            "log");
            if (!string.IsNullOrEmpty(path))
            {
                OpenWindow(path);
            }
        }

        static void OpenWindow(string filePath)
        {
            if (window != null) window.Close();
            window = (ConsoleEditorWindow)GetWindow(
                typeof(ConsoleEditorWindow),
                true,
                "Console Log View",
                true
            );
            window.minSize = new Vector2(1024, 768);
            window.currentFilePath = filePath;
            window.Init();
            window.Show();
        }

        #endregion
    }
}
