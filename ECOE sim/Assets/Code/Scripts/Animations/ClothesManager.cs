using System.Collections.Generic;
using UnityEngine;

public enum ClothType
{
    Hair,
    Chest,
    Legs,
    Feet
}

public class ClothesManager : MonoBehaviour
{
    [Header("Cuerpo")]
    [Tooltip("SkinnedMeshRenderer del cuerpo base")]
    public SkinnedMeshRenderer smrBody;
    [Tooltip("SkinnedMeshRenderer de la cabeza")]
    public SkinnedMeshRenderer smrHead;

    [Header("Catálogo de Ropa")]
    public List<GameObject> hairPrefabs;
    public List<GameObject> chestPrefabs;
    public List<GameObject> legPrefabs;
    public List<GameObject> feetPrefabs;

    [Header("Configuración de la Cara")]
    public int[] faceIndexList = { 0, 4, 8, 12, 16, 20, 24, 28 };
    public int[] symptomsIndexList = { 0, 1, 2, 3, 4, 5, 6, 7 };

    private string faceIndex = "_FaceIndex";
    private string symptomIndex = "_SymptomIndex";
    private string skinBlend = "_SkinBlend";

    [Header("Paletas de Colores")]
    public Color[] clothColors;
    public Color[] hairColors;

    private string clothColor = "_ClothColor";
    private string hairTipColor = "_TipColor";
    private string hairRootColor = "_RootColor";
    private string hairShineColor = "_LightColor";

    private Dictionary<string, Transform> boneMap;
    private Dictionary<ClothType, GameObject> equipedCloth = new Dictionary<ClothType, GameObject>();

    void Start()
    {
        MapBaseRig();
        RandomDressing();
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

            if (clothIndex != -1)
            {
                float actualWeight = smrBody.GetBlendShapeWeight(i);
                smrCloth.SetBlendShapeWeight(clothIndex, actualWeight);
            }
        }

        ApplyColor(smrCloth, cat);

        Transform clothArmature = clothInstance.transform.Find("Armature");
        if (clothArmature != null)
        {
            Destroy(clothArmature.gameObject);
        }

        equipedCloth.Add(cat, clothInstance);
    }

    private void ApplyColor(SkinnedMeshRenderer smrCloth, ClothType cat)
    {
        if (smrCloth == null || smrCloth.material == null) return;

        if (cat == ClothType.Hair)
        {
            if (hairColors == null || hairColors.Length == 0) return;
            Color hairChoosenColor = hairColors[Random.Range(0, hairColors.Length)];

            if (smrCloth.material.HasProperty(hairTipColor))
            {
                smrCloth.material.SetColor(hairTipColor, hairChoosenColor);
                smrCloth.material.SetColor(hairRootColor, hairChoosenColor * 0.6f);

                Color shineColor = Color.Lerp(hairChoosenColor, Color.white, 0.4f);
                smrCloth.material.SetColor(hairShineColor, shineColor);
            }
        }
        else
        {
            if (clothColors == null || clothColors.Length == 0) return;
            Color clothChoosenColor = clothColors[Random.Range(0, clothColors.Length)];

            if (smrCloth.material.HasProperty(clothColor))
            {
                smrCloth.material.SetColor(clothColor, clothChoosenColor);
            }
        }
    }

    public void RandomDressing()
    {
        float randomSkin = Random.Range(0f, 1f);
        int randomFace = faceIndexList.Length > 0 ? faceIndexList[Random.Range(0, faceIndexList.Length)] : 0;
        int randomSymptom = symptomsIndexList.Length > 0 ? symptomsIndexList[Random.Range(0, symptomsIndexList.Length)] : 0;

        SkinnedMeshRenderer[] bodyParts = { smrBody, smrHead };


        foreach (SkinnedMeshRenderer part in bodyParts)
        {
            if (part != null && part.materials != null)
            {
                Material[] materialInstances = part.materials;

                foreach (Material mat in materialInstances)
                {
                    if (mat == null) continue;
                    if (mat.HasProperty(skinBlend)) mat.SetFloat(skinBlend, randomSkin);
                    if (mat.HasProperty(faceIndex)) mat.SetFloat(faceIndex, randomFace);
                    if (mat.HasProperty(symptomIndex)) mat.SetFloat(symptomIndex, randomSymptom);
                }

                part.materials = materialInstances;
            }
        }

        RandomEquip(hairPrefabs, ClothType.Hair);
        RandomEquip(chestPrefabs, ClothType.Chest);
        RandomEquip(legPrefabs, ClothType.Legs);
        RandomEquip(feetPrefabs, ClothType.Feet);
    }

    private void RandomEquip(List<GameObject> prefabsList, ClothType category)
    {
        if (prefabsList == null || prefabsList.Count == 0) return;

        int randomIndex = Random.Range(0, prefabsList.Count);
        EquipCloth(prefabsList[randomIndex], category);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomDressing();
        }
    }
}
