using System;
using System.IO;
using UnityEngine;

namespace ViveTrackerSerialManager
{
    public static class TrackerManager
    {
        private static string folderPath = "C:/Users/User/AppData/LocalLow/yoyo/ViveTrackerSerialManager";
        private static string fileName = "TrackerData.json";

        /// <summary>
        /// "TrackerData.json" 파일에서 1~4번 트래커에 할당된 TrackerData 객체 배열 가져오기
        /// </summary>
        /// <returns>1~4번 트래커에 할당된 TrackerData 배열</returns>
        public static TrackerData[] GetTrackers()
        {
            string path = Path.Combine(folderPath, fileName);
            string jsonData = File.ReadAllText(path);
            return JsonHelper.FromJson<TrackerData>(jsonData);
        }
    }

    public static class JsonHelper
    {
        /// <summary>
        /// JSON 문자열을 역직렬화하여 T 형식의 배열로 반환합니다.
        /// </summary>
        /// <typeparam name="T">역직렬화할 객체의 형식</typeparam>
        /// <param name="json">JSON 문자열</param>
        /// <returns>T 형식의 배열</returns>
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.data;
        }

        /// <summary>
        /// 배열을 직렬화하여 JSON 문자열로 반환합니다.
        /// </summary>
        /// <typeparam name="T">직렬화할 객체의 형식</typeparam>
        /// <param name="array">직렬화할 배열</param>
        /// <returns>JSON 문자열</returns>
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.data = array;
            return JsonUtility.ToJson(wrapper, true);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] data;
        }
    }
}
