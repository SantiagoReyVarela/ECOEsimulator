using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Code.Scripts.Core.SaveLoad
{
    [Serializable]
    public class SaveData
    {
        public string formatVersion;
        public string savedAt;
        public string sceneName;
        public Dictionary<string, JToken> objects = new Dictionary<string, JToken>();
    }
}