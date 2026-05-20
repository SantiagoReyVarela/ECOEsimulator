using System.Collections.Generic;
using UnityEngine;

public enum ClothType
{
    Cabeza,
    Torso,
    Piernas,
    Pies,
    Manos
}

public class ClothesManager : MonoBehaviour
{
    [Header("Cuerpo")]
    [Tooltip("SkinnedMeshRenderer del cuerpo base")]
    public SkinnedMeshRenderer smrBody;

    [Header("Prueba de Equipado")]
    public GameObject camisetaPruebaPrefab;

    private Dictionary<string, Transform> boneMap;
    private Dictionary<ClothType, GameObject> equipedCloth = new Dictionary<ClothType, GameObject>();

    void Start()
    {
        MapBaseRig();
    }

    private void MapBaseRig()
    {
        boneMap = new Dictionary<string, Transform>();

        if (smrBody == null) return;

        foreach (Transform bone in smrBody.bones)
        {
            if (!boneMap.ContainsKey(bone.name))
            {
                boneMap.Add(bone.name, bone);
            }
        }
    }

    public void EquipCloth(GameObject clothPrefab, ClothType cat)
    {
        if (clothPrefab == null) return;
        if (boneMap == null || boneMap.Count == 0) MapBaseRig();

        if (equipedCloth.ContainsKey(cat))
        {
            if (equipedCloth[cat] != null)
            {
                Destroy(equipedCloth[cat]);
            }
            equipedCloth.Remove(cat);
        }

        GameObject clothInstance = Instantiate(clothPrefab, transform);
        SkinnedMeshRenderer smrCloth = clothInstance.GetComponentInChildren<SkinnedMeshRenderer>();

        Transform[] newBones = new Transform[smrCloth.bones.Length];

        for (int i = 0; i < smrCloth.bones.Length; i++)
        {
            string boneNameRequired = smrCloth.bones[i].name;

            if (boneMap.TryGetValue(boneNameRequired, out Transform foundBone))
            {
                newBones[i] = foundBone;
            }
            else
            {
                Debug.LogWarning($"Hueso '{boneNameRequired}' requerido por la prenda no existe en el esqueleto base.");
            }
        }

        smrCloth.bones = newBones;
        smrCloth.rootBone = smrBody.rootBone;

        for (int i = 0; i < smrBody.sharedMesh.blendShapeCount; i++)
        {
            string blendShapeName = smrBody.sharedMesh.GetBlendShapeName(i);
            int clothIndex = smrCloth.sharedMesh.GetBlendShapeIndex(blendShapeName);

            if (clothIndex != -1) // Si la prenda tiene este Blend Shape
            {
                float actualWeight = smrBody.GetBlendShapeWeight(i);
                smrCloth.SetBlendShapeWeight(clothIndex, actualWeight);
            }
        }

        Transform clothArmature = clothInstance.transform.Find("Armature");
        if (clothArmature != null)
        {
            Destroy(clothArmature.gameObject);
        }

        equipedCloth.Add(cat, clothInstance);
    }

    // Prueba de uso
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && camisetaPruebaPrefab != null)
        {
            EquipCloth(camisetaPruebaPrefab, ClothType.Torso);
        }
    }
}
