using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    public class ResourcesGenerator
    {
        [MenuItem("Tools/Create Resources")]
        private static void Create()
        {
            List<string> names = new List<string>(1000);
            List<string> childNames = new List<string>(50);

            var objects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            
            var copyPath = "Assets/R.cs";
            Debug.Log("Regenerating R: " + copyPath);
            using (var outfile =
                new StreamWriter(copyPath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("");
                outfile.WriteLine("public static class R {");
                
                foreach (var o in objects)
                {
                    names.Add(o.name);

                    string fieldName = o.name.Replace(" ", "");
                    fieldName = fieldName.Replace("(", "_");
                    fieldName = fieldName.Replace(")", "_");
                    
                    outfile.WriteLine("public static string " + fieldName + " =  \"" + o.name + "\";");
                    
                    //if it has children write them as a subclass, the method will return the parent too
                    var children = o.GetComponentsInChildren<Transform>(true);

                    if (children.Length > 1)
                    { 
                        outfile.WriteLine("\t public static class " + fieldName + "Children");
                        outfile.WriteLine("\t {");
                        
                        foreach (var child in children)
                        {
                            if(child.gameObject == o) continue;
                            
                            fieldName = child.name.Replace(" ", "");
                            fieldName = fieldName.Replace("(", "_");
                            fieldName = fieldName.Replace(")", "_");

                            if (childNames.Contains(fieldName))
                                fieldName += ArrayUtility.IndexOf(children,child);
                            
                            outfile.WriteLine("\t \t public static string " + fieldName + " =  \"" + child.name + "\";");
                            
                            childNames.Add(fieldName);
                        }
                        outfile.WriteLine("\t }");
                    }

                }

                outfile.WriteLine("}");
            }
            
            AssetDatabase.Refresh();
        }


    }
}
