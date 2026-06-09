using System.Collections.Generic;
using UnityEngine;

public class StaticClothing : MonoBehaviour
{
    [Header("Cuerpo Base")]
    [Tooltip("El SkinnedMeshRenderer del personaje fijo")]
    public SkinnedMeshRenderer smrBody;

    [Header("Prendas Fijas a Equipar")]
    public GameObject staticHair;
    public GameObject staticChest;
    public GameObject staticLegs;
    public GameObject staticFeet;
    public GameObject staticAccessorie;

    private Dictionary<string, Transform> boneMap;

    void Start()
    {
        BoneMapping();

        EquipCloth(staticHair);
        EquipCloth(staticChest);
        EquipCloth(staticLegs);
        EquipCloth(staticFeet);
        EquipCloth(staticAccessorie);
    }

    private void BoneMapping()
    {
        if (smrBody == null) return;

        boneMap = new Dictionary<string, Transform>();

        foreach (Transform bone in smrBody.bones)
        {
            if (!boneMap.ContainsKey(bone.name))
            {
                boneMap.Add(bone.name, bone);
            }
        }
    }

    private void EquipCloth(GameObject clothPrefab)
    {
        if (clothPrefab == null) return;

        GameObject instance = Instantiate(clothPrefab, transform);
        SkinnedMeshRenderer smrCloth = instance.GetComponentInChildren<SkinnedMeshRenderer>();

        if (smrCloth == null) return;

        Transform[] newBones = new Transform[smrCloth.bones.Length];

        for (int i = 0; i < smrCloth.bones.Length; i++)
        {
            string boneName = smrCloth.bones[i].name;

            if (boneMap.TryGetValue(boneName, out Transform foundBone))
            {
                newBones[i] = foundBone;
            }
            else
            {
                Debug.LogWarning($"El hueso '{boneName}' no existe en este personaje.");
            }
        }

        smrCloth.bones = newBones;
        smrCloth.rootBone = smrBody.rootBone;

        for (int i = 0; i < smrBody.sharedMesh.blendShapeCount; i++)
        {
            string blendShapeName = smrBody.sharedMesh.GetBlendShapeName(i);
            int clothIndex = smrCloth.sharedMesh.GetBlendShapeIndex(blendShapeName);

            if (clothIndex != -1)
            {
                float actualWeight = smrBody.GetBlendShapeWeight(i);
                smrCloth.SetBlendShapeWeight(clothIndex, actualWeight);
            }
        }

        Transform clothArmature = instance.transform.Find("Armature");
        if (clothArmature != null)
        {
            Destroy(clothArmature.gameObject);
        }
    }
}
