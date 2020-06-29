
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LEDController {

    public class Persistence : MonoBehaviour {

        private static string fileName = "settings.json";
        private static string tempFileName = "temp.json";
        public Settings settings;

        void Awake () {
            //LoadSettings ();
        }

        public void SaveSettings () {
            if (File.Exists (Path.Combine (Application.persistentDataPath, fileName))) {
                File.Copy (Path.Combine (Application.persistentDataPath, fileName), (Path.Combine (Application.persistentDataPath, tempFileName)));
            }
            File.Create (Path.Combine (Application.persistentDataPath, fileName));
            StreamWriter sw = new StreamWriter (Path.Combine (Application.persistentDataPath, fileName));
            try {
                sw.Write (JsonUtility.ToJson (settings, true));
            } catch (Exception e) {
                Debug.LogError (e);

            }
        }

        void LoadSettings () {
            if (File.Exists (Path.Combine (Application.persistentDataPath, fileName))) {
                StreamReader sr = new StreamReader (Path.Combine (Application.persistentDataPath, fileName));
                settings = JsonUtility.FromJson <Settings> (sr.ReadToEnd ());
                sr.Close ();
            }
        }

        void OnDestroy () {
            //SaveSettings ();
        }

    }

}
