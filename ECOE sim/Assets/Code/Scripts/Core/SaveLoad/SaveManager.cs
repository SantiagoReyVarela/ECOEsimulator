using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Code.Scripts.Core.SaveLoad.Interfaces;
using Code.Scripts.Patterns.Singleton;
using UnityEngine;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Code.Scripts.Core.SaveLoad
{
    public class SaveManager : Singleton<SaveManager>
    {
        [Header("Save Load Settings")] [SerializeField]
        private string saveFilePrefix = "save_slot_";

        [SerializeField] private string saveFileExtension = ".json";
        [SerializeField] private string savesFolderName = "Saves";
        [SerializeField] private string currentFormatVersion = "1.0";
        
        public int selectedSlot = 0;

        string SavesFolderPath => System.IO.Path.Combine(Application.persistentDataPath, savesFolderName);

        void Awake()
        {
            base.Awake();
            EnsureSavesFolder();
        }

        void EnsureSavesFolder()
        {
            if (!System.IO.Directory.Exists(SavesFolderPath))
            {
                System.IO.Directory.CreateDirectory(SavesFolderPath);
            }
        }

        string GetSlotFilePath(int slot)
        {
            return System.IO.Path.Combine(SavesFolderPath, $"{saveFilePrefix}{slot}{saveFileExtension}");
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncFiles();
#endif

        public async Task SaveAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR

                SaveSlotSync();
#else
            await SaveSlotInternalAsync();
#endif
        }

        async Task SaveSlotInternalAsync()
        {
            EnsureSavesFolder();

            var saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();
            var saveData = new SaveData
            {
                formatVersion = currentFormatVersion,
                savedAt = DateTime.UtcNow.ToString("o"),
                sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            };

            foreach (var saveable in saveables)
            {
                try
                {
                    var id = saveable.GetSaveId();
                    if (string.IsNullOrEmpty(id)) continue;
                    var state = saveable.CaptureState();
                    saveData.objects[id] = state ?? JValue.CreateNull();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error capturing state for {saveable.GetType().Name}: {e}");
                }
            }

            var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            var filePath = GetSlotFilePath(selectedSlot);
            try
            {
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveManager: error writing save file: {e}");
            }
        }

        // Síncrono (para WebGL)
        void SaveSlotSync()
        {
            EnsureSavesFolder();

            var saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>();
            var saveData = new SaveData
            {
                formatVersion = currentFormatVersion,
                savedAt = DateTime.UtcNow.ToString("o"),
                sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            };

            foreach (var s in saveables)
            {
                try
                {
                    var id = s.GetSaveId();
                    if (string.IsNullOrEmpty(id)) continue;
                    var state = s.CaptureState();
                    saveData.objects[id] = state ?? JValue.CreateNull();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"SaveManager: error capturing state for {s.GetSaveId()}: {ex}");
                }
            }

            var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            var path = GetSlotFilePath(selectedSlot);
            try
            {
                File.WriteAllText(path, json);
                Debug.Log($"SaveManager: Saved slot {selectedSlot} -> {path} (sync)");
#if UNITY_WEBGL && !UNITY_EDITOR
            try { SyncFiles(); } catch (Exception ex) { Debug.LogWarning($"SaveManager: SyncFiles failed: {ex}"); }
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveManager: error writing save file: {ex}");
            }
        }

        // Carga asíncrona (usa síncrono en WebGL)
        public async Task<bool> LoadSlotAsync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        return LoadSlotSync();
#else
            return await LoadSlotInternalAsync();
#endif
        }

        async Task<bool> LoadSlotInternalAsync()
        {
            var path = GetSlotFilePath(selectedSlot);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"SaveManager: slot file not found: {path}");
                return false;
            }

            try
            {
                var json = await File.ReadAllTextAsync(path);
                var saveData = JsonConvert.DeserializeObject<SaveData>(json);

                var saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>()
                    .ToDictionary(s => s.GetSaveId(), s => s);

                foreach (var kv in saveData.objects)
                {
                    if (saveables.TryGetValue(kv.Key, out var saveable))
                    {
                        try
                        {
                            saveable.RestoreState(kv.Value);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"SaveManager: error restoring {kv.Key}: {ex}");
                        }
                    }
                    else
                    {
                        Debug.Log($"SaveManager: no objeto en escena corresponde a id guardado: {kv.Key}");
                    }
                }

                Debug.Log($"SaveManager: Loaded slot {selectedSlot} from {path}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveManager: error reading/parsing save file: {ex}");
                return false;
            }
        }

        // Síncrono load (WebGL)
        bool LoadSlotSync()
        {
            var path = GetSlotFilePath(selectedSlot);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"SaveManager: slot file not found: {path}");
                return false;
            }

            try
            {
                var json = File.ReadAllText(path);
                var saveData = JsonConvert.DeserializeObject<SaveData>(json);

                var saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>()
                    .ToDictionary(s => s.GetSaveId(), s => s);

                foreach (var kv in saveData.objects)
                {
                    if (saveables.TryGetValue(kv.Key, out var saveable))
                    {
                        try
                        {
                            saveable.RestoreState(kv.Value);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"SaveManager: error restoring {kv.Key}: {ex}");
                        }
                    }
                    else
                    {
                        Debug.Log($"SaveManager: no objeto en escena corresponde a id guardado: {kv.Key}");
                    }
                }

                Debug.Log($"SaveManager: Loaded slot {selectedSlot} from {path} (sync)");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveManager: error reading/parsing save file: {ex}");
                return false;
            }
        }

        public bool SlotExists() => File.Exists(GetSlotFilePath(selectedSlot));

        public bool DeleteSlot()
        {
            var path = GetSlotFilePath(selectedSlot);
            if (!File.Exists(path)) return false;
            try
            {
                File.Delete(path);
#if UNITY_WEBGL && !UNITY_EDITOR
            try { SyncFiles(); } catch { }
#endif
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveManager: error deleting slot {selectedSlot}: {ex}");
                return false;
            }
        }

        public List<SaveSlotInfo> ListSlots(int maxSlotsToProbe = 20)
        {
            var files = Directory.Exists(SavesFolderPath)
                ? Directory.GetFiles(SavesFolderPath, $"{saveFilePrefix}*{saveFileExtension}")
                : Array.Empty<string>();
            var list = new List<SaveSlotInfo>();
            foreach (var f in files)
            {
                try
                {
                    var json = File.ReadAllText(f);
                    var sd = JsonConvert.DeserializeObject<SaveData>(json);
                    var name = Path.GetFileNameWithoutExtension(f);
                    var indexPart = name.Replace(saveFilePrefix, "");
                    int.TryParse(indexPart, out int idx);
                    list.Add(new SaveSlotInfo
                    {
                        slotIndex = idx,
                        filePath = f,
                        savedAt = sd?.savedAt,
                        sceneName = sd?.sceneName,
                        formatVersion = sd?.formatVersion
                    });
                }
                catch
                {
                    // ignorar archivos rotos
                }
            }

            return list.OrderBy(i => i.slotIndex).ToList();
        }

        public void SaveSlotCoroutine()
        {
            _ = SaveAsync();
        }

    }
}