using System.Collections.Generic;
using UnityEngine;

public enum ClothType
{
    Hair,
    Chest,
    Legs,
    Feet,
    Accessorie
}

public class ClothesManager : MonoBehaviour
{
    [Header("Cuerpo")]
    [Tooltip("SkinnedMeshRenderer del cuerpo base")]
    public SkinnedMeshRenderer smrBody;
    [Tooltip("SkinnedMeshRenderer de la cabeza")]
    public SkinnedMeshRenderer smrHead;

    [Header("Complexión Física")]
    public float blendShapeMin = 0f;
    public float blendShapeMax = 100f;
    private float[] blendShapesWeights;

    [Header("Catálogo de Ropa")]
    private List<GameObject> hairPrefabs;
    private List<GameObject> chestPrefabs;
    private List<GameObject> legPrefabs;
    private List<GameObject> feetPrefabs;
    private List<GameObject> accessoriePrefabs;

    [Header("Probabilidades")]
    [Range(0f, 1f)]
    [Tooltip("0 = Nunca lleva accesorio, 1 = Siempre lleva, 0.5 = 50% de probabilidad")]
    public float accesorieChance = 0.5f;

    [Header("Configuración de la Cara")]
    [Tooltip("Índice mínimo en el Atlas")]
    public int minFaceIndex = 0;
    [Tooltip("Índice máximo en el Atlas")]
    public int maxFaceIndex = 32;

    [Header("Configuración de Síntomas")]
    [Tooltip("Índice mínimo en el Atlas")]
    public int minSymptomIndex = 0;
    [Tooltip("Índice máximo en el Atlas")]
    public int maxSymptomIndex = 7;

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

    void Awake()
    {
        LoadPrefabs();
    }

    void Start()
    {
        MapBaseRig();
        RandomDressing();
    }

    private void LoadPrefabs()
    {
        hairPrefabs = new List<GameObject>();
        chestPrefabs = new List<GameObject>();
        legPrefabs = new List<GameObject>();
        feetPrefabs = new List<GameObject>();
        accessoriePrefabs = new List<GameObject>();

        ClothItem[] allItems = Resources.LoadAll<ClothItem>("");

        if (allItems.Length == 0)
        {
            Debug.LogWarning("No se han encontrado prendas");
            return;
        }

        foreach (ClothItem item in allItems)
        {
            switch (item.category)
            {
                case ClothType.Hair:
                    hairPrefabs.Add(item.gameObject);
                    break;
                case ClothType.Chest:
                    chestPrefabs.Add(item.gameObject);
                    break;
                case ClothType.Legs:
                    legPrefabs.Add(item.gameObject);
                    break;
                case ClothType.Feet:
                    feetPrefabs.Add(item.gameObject);
                    break;
                case ClothType.Accessorie:
                    accessoriePrefabs.Add(item.gameObject);
                    break;
            }
        }

        Debug.Log($"Catálogo cargado: {hairPrefabs.Count} Pelos, {chestPrefabs.Count} Torsos, {legPrefabs.Count} Piernas, {feetPrefabs.Count} Zapatos, {accessoriePrefabs.Count} Accesorios.");
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

    public void RemoveCloth(ClothType cat)
    {
        if (equipedCloth.ContainsKey(cat))
        {
            if (equipedCloth[cat] != null)
            {
                Destroy(equipedCloth[cat]);
            }
            equipedCloth.Remove(cat);
        }
    }

    public void EquipCloth(GameObject clothPrefab, ClothType cat)
    {
        if (clothPrefab == null) return;
        if (boneMap == null || boneMap.Count == 0) MapBaseRig();

        RemoveCloth(cat);

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

        if (cat == ClothType.Accessorie) return;

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
        int randomFace = Random.Range(minFaceIndex, maxFaceIndex + 1);
        int randomSymptom = Random.Range(minSymptomIndex, maxSymptomIndex + 1);

        if (smrBody != null)
        {
            int blendShapesNum = smrBody.sharedMesh.blendShapeCount;
            blendShapesWeights = new float[blendShapesNum];

            for (int i = 0; i < blendShapesNum; i++)
            {
                float randomWeight = Random.Range(blendShapeMin, blendShapeMax);
                blendShapesWeights[i] = randomWeight;
                smrBody.SetBlendShapeWeight(i, randomWeight);
            }
        }

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

        if (Random.value <= accesorieChance)
        {
            RandomEquip(accessoriePrefabs, ClothType.Accessorie);
        }
        else
        {
            RemoveCloth(ClothType.Accessorie);
        }
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

    void LateUpdate()
    {
        if (smrBody != null && blendShapesWeights != null)
        {
            for (int i = 0; i < blendShapesWeights.Length; i++)
            {
                smrBody.SetBlendShapeWeight(i, blendShapesWeights[i]);
            }
        }

        if (smrHead != null && blendShapesWeights != null)
        {
            for (int i = 0; i < blendShapesWeights.Length; i++)
            {
                string nombreBs = smrBody.sharedMesh.GetBlendShapeName(i);
                int headIndex = smrHead.sharedMesh.GetBlendShapeIndex(nombreBs);

                if (headIndex != -1)
                {
                    smrHead.SetBlendShapeWeight(headIndex, blendShapesWeights[i]);
                }
            }
        }
    }
}
