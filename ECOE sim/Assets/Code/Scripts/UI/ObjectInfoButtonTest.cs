using UnityEngine;

public class ObjectInfoButtonTest : MonoBehaviour
{
    public ObjectsDatabaseSO database;
    public int objectID;

    private void Start()
    {
        var btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(() =>
        {
            var data = database.objectsData.Find(x => x.ID == objectID);
            GameplayUIManager.Instance.OpenInfoPanel(data);
        });
    }
}