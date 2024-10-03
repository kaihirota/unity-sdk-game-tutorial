using Immutable.Search.Model;
using TMPro;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    ///     Represents a pack's item
    /// </summary>
    public class PackItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Name;
        [SerializeField] private TextMeshProUGUI m_Amount;

        public void Initialise(PackItem item)
        {
            m_Name.text = item.name;
            m_Amount.text = item.amount.ToString();
        }
    }
}