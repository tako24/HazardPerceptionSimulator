using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelInfo : MonoBehaviour
{
    [SerializeField] private Image mapImage;
    private GameObject levelPrefab;
    [SerializeField] private TextMeshProUGUI description;

    private LevelInfo levelInfo;

    public LevelInfo LevelInfo
    {
        private get => levelInfo;
        set
        {
            if(value == null)
                return;

            mapImage.sprite = value.MapImage;
            levelPrefab = value.LevelPrefab;
            description.text = value.Description;
        }
    }
}

