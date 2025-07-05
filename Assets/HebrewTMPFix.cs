using TMPro;
using UnityEngine;
using System.Globalization;
using System.Linq;
using System.Text;

public class HebrewTMPFix : MonoBehaviour
{
    private TMP_Text textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        if (textMeshPro != null)
        {
            textMeshPro.isRightToLeftText = true;
            string original = textMeshPro.text;
            string fixedText = FixRTL(original);
            textMeshPro.text = fixedText;
        }
    }

    string FixRTL(string input)
    {
        string normalized = input.Normalize(NormalizationForm.FormC);
        var words = normalized.Split(' ');
        System.Array.Reverse(words);
        return string.Join(" ", words);
    }

}
