using UnityEditor;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;

public class ScriptBatch
{
    [MenuItem("MyTools/Windows Build With Postprocess")]
    public static void BuildGame()
    {
        // Get filename.
        string path = @"c:\temp\InjectionTest\";//  EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Scenes/InjectionTest.unity" };// Scene1.unity", "Assets/Scene2.unity" };

        // Build player.
        //BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.apk", BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);

        //string apkPath = @"C:\temp\InjectionTest\BuiltGame.apk";//\assets\bin\Data\Managed\Assembly-CSharp.dll";
        //string apkPath2 = @"C:\temp\InjectionTest\BuiltGame2.apk";//\assets\bin\Data\Managed\Assembly-CSharp.dll";
        string androidDllPath = @"C:\temp\InjectionTest\BuiltGame.apk\AkkaUnityClientSample\assets\bin\Data\Managed\Assembly-CSharp.dll";

        //UnzipApk(apkPath);

        AssemblyPostProcessor.PostProcess(androidDllPath);

        //ZipApk(Path.GetFullPath(apkPath), apkPath2);

        //AssemblyPostProcessor.PostProcess(@"C:\temp\InjectionTest\BuiltGame_Data\Managed\Assembly-CSharp.dll");

        // Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = path + "BuiltGame.exe";
        proc.Start();
    }

    private static void UnzipApk(string path)
    {
        UnityEngine.Debug.Log("UnzipApk");

        using (ZipInputStream s = new ZipInputStream(File.OpenRead(path)))
        {

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                //UnityEngine.Debug.Log(theEntry.Name);

                string directoryName = Path.GetDirectoryName(theEntry.Name);
                directoryName = Path.Combine(Path.GetDirectoryName(path), directoryName);
                string fileName = Path.GetFileName(theEntry.Name);

                UnityEngine.Debug.Log("Unzipping " + theEntry.Name);
                // create directory
                if (directoryName.Length > 0)
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (fileName != String.Empty)
                {
                    var outputPath = Path.Combine(directoryName, fileName);
                    using (FileStream streamWriter = File.Create(outputPath))
                    {
                        UnityEngine.Debug.Log("CreateStream Open " + theEntry.Name);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private static void ZipApk(string path, string filename)
    {
        try
        {
            // Depending on the directory this could be very large and would require more attention
            // in a commercial package.
            string[] filenames = Directory.GetFiles(path);

            // 'using' statements guarantee the stream is closed properly which is a big source
            // of problems otherwise.  Its exception safe as well which is great.
            using (ZipOutputStream s = new ZipOutputStream(File.Create(filename)))
            {

                s.SetLevel(9); // 0 - store only to 9 - means best compression

                byte[] buffer = new byte[4096];

                foreach (string file in filenames)
                {

                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));

                    // Setup the entry data as required.

                    // Crc and size are handled by the library for seakable streams
                    // so no need to do them here.

                    // Could also use the last write time or similar for the file.
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);

                    using (FileStream fs = File.OpenRead(file))
                    {

                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                s.Finish();

                // Close is important to wrap things up and unlock the file.
                s.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception during processing {0}", ex);

            // No need to rethrow the exception as for our purposes its handled.
        }
    }
}