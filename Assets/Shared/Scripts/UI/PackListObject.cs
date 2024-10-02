using System;
using System.Net.Http;
using System.Numerics;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace HyperCasual.Runner
{
    /// <summary>
    ///     Represents an asset in the player's inventory
    /// </summary>
    public class PackListObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_DescriptionText;
        [SerializeField] private ImageUrlObject m_Image;

        private string m_Name;
        private string m_Description;
        private string m_ImageUrl;

        private void OnEnable()
        {
            UpdateData();
        }

        /// <summary>
        ///     Initialises the asset object with relevant data and updates the UI.
        /// </summary>
        public void Initialise(string name, string description, string imageUrl)
        {
            m_Name = name;
            m_Description = description;
            m_ImageUrl = imageUrl;
            
            UpdateData();
        }

        /// <summary>
        ///     Updates the text fields with data.
        /// </summary>
        private async void UpdateData()
        {
            m_NameText.text = m_Name;
            m_DescriptionText.text = m_Description;
            m_Image.LoadUrl(m_ImageUrl);
        }
    }
}