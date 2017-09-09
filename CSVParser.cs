using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// This Script was written by huseong Lee.
/// with MIT License
/// </summary>
namespace GameUtilSD {
    public class CSVReader {
        /// <summary>
        /// Load the asset in the path.
        /// </summary>
        /// <param name="path">The path of the Asset</param>
        /// <returns></returns>
        private static string[] loadAsset(string path) {
            TextAsset asset = (Resources.Load(path) as TextAsset);
            if (asset == null) {
                throw new Exception("Asset Load Failed");
            }
            string[] stringAsset = (asset.text).Split('\n');
            return stringAsset;
        }

        /// <summary>
        /// return dictionary that represent the asset in the path.
        /// You can use this such as translation.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getDic(string path) {
            Dictionary<string, string> hashMap = new Dictionary<string, string>();
            string[] asset = loadAsset(path);
            for (int i = 0; i < asset.Length; i++) {
                string[] temp = asset[i].Split(',');
                if (temp.Length < 2) {
                    Debug.LogError("Can Not Find Value line : " + (i + 1));
                    continue;
                } else if (hashMap.ContainsKey(temp[0])) {
                    Debug.Log("This Key is already exist. Key : " + temp[0]);
                    continue;
                }
                hashMap.Add(temp[0], temp[1]);
            }
            return hashMap;
        }

        /// <summary>
        /// return list that represent the asset in the path
        /// </summary>
        /// <param name="path">The path that asset is located</param>
        /// <returns></returns>
        public static List<T> getList<T>(string path) {
            List<T> list = new List<T>();
            string[] asset = loadAsset(path);
            for (int i = 0; i < asset.Length; i++) {
                list.Add((T)Convert.ChangeType(asset[i], typeof(T)));
            }
            return list;
        }

        /// <summary>
        /// return dictionary which key is string and value is List
        /// </summary>
        /// <typeparam name="T"> type of List </typeparam>
        /// <param name="path"> path of ResourceFile </param> 
        /// <returns></returns>
        public static Dictionary<string, List<T>> getListDic<T>(string path) {
            Dictionary<string, List<T>> hashMap = new Dictionary<string, List<T>>();
            string[] asset = loadAsset(path);
            string[] keys = asset[0].Remove(asset[0].Length - 1).Split(',');
            List<T>[] values = new List<T>[keys.Length];
            for (int i = 0; i < keys.Length; i++) {
                values[i] = new List<T>();
            }
            for (int i = 1; i < asset.Length; i++) {
                T[] array = customSplit<T>(keys.Length, asset[i]);
                for (int j = 0; j < keys.Length; j++) {
                    values[j].Add(array[j]);
                }
            }
            for (int i = 0; i < keys.Length; i++) {
                hashMap.Add(keys[i], values[i]);
            }
            return hashMap;
        }

        private static T[] customSplit<T>(int length, string input) {
            string temp = "";
            T[] returnArr = new T[length];
            int count = 0;
            for (int i = 0; i < input.Length; i++) {
                if (input[i] == ',') {
                    if (temp == "") {
                        count++;
                        continue;
                    }
                    returnArr[count++] = (T)Convert.ChangeType(temp, typeof(T));
                    temp = "";
                    continue;
                }
                temp += input[i];
            }
            try {
                returnArr[length - 1] = (T)Convert.ChangeType(temp, typeof(T));
            } catch (FormatException) {
                return returnArr;
            }
            return returnArr;
        }
    }
}


