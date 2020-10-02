using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions.Examples;

namespace OverdoseTheGame
{
	/// <summary>
	/// Oyunu yöneten sınıftır.
	/// Tüm levelleri ve seçilen leveli tutar
	/// </summary>
	public class GameManager : MonoBehaviour
    {
        #region Editor Fields
        public GameObject MainMenuPanel;

        public GameObject GamePanel;

        public GameObject CreditsPanel;

        public GameObject LevelSelectionPanel;

        public RectTransform LevelContainer;

        public GameObject LevelPrefab;

        public GameObject WinImage;

        public GameObject LoseImage;
        #endregion

        private List<GameObject> LevelButtons { get; set; }

        private List<Level> _levels;
        public IEnumerable<Level> Levels => _levels;

        public Level SelectedLevel { get; private set; }

        public string LevelXmlPath { get; } = @"Levels";

        void Start()
        {
            LoadLevels();

            CreateLevelButtons();

            SelectMainPanel();
        }

        public void SelectMainPanel()
        {
            MainMenuPanel.SetActive(true);
            GamePanel.SetActive(false);
            CreditsPanel.SetActive(false);
            LevelSelectionPanel.SetActive(false);
            WinImage.SetActive(false);
            LoseImage.SetActive(false);
        }

        public void SelectLevelSelectionPanel()
        {
            MainMenuPanel.SetActive(false);
            GamePanel.SetActive(false);
            CreditsPanel.SetActive(false);
            LevelSelectionPanel.SetActive(true);
            WinImage.SetActive(false);
            LoseImage.SetActive(false);
        }

        public void SelectCreditsPanel()
        {
            MainMenuPanel.SetActive(false);
            GamePanel.SetActive(false);
            CreditsPanel.SetActive(true);
            LevelSelectionPanel.SetActive(false);
            WinImage.SetActive(false);
            LoseImage.SetActive(false);
        }

        internal void LoseLevel(Level currentLevel)
        {
            MainMenuPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            LevelSelectionPanel.SetActive(false);
            WinImage.SetActive(false);
            LoseImage.SetActive(true);
        }

        internal void WonLevel(Level currentLevel)
        {
            MainMenuPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            LevelSelectionPanel.SetActive(false);
            WinImage.SetActive(true);
            LoseImage.SetActive(false);
        }

        public void Exit()
        {
            Application.Quit();
        }

        private void CreateLevelButtons()
        {
            LevelButtons = new List<GameObject>();

            var width = LevelContainer.parent.parent.GetComponent<RectTransform>().rect.width;

            var prefabTransform = LevelPrefab.GetComponent<RectTransform>();
            var rowSize = (int)((width - 20f) / (prefabTransform.rect.width + 20f));
            var margin = (width - rowSize * prefabTransform.rect.width) / (rowSize + 1);

            float y = 0;
            for (int i = 0; i < Levels.Count(); i++)
            {
                var row = (int)(i / rowSize);
                var col = i % rowSize;

                y = (row + 1) * 20f + (row + 0.5f) * prefabTransform.rect.height;
                var x = (col + 1) * margin + (col + 0.5f) * prefabTransform.rect.width;

                var levelObj = Instantiate(LevelPrefab, LevelContainer);
                levelObj.transform.localPosition = new Vector3(x, -y);
                levelObj.GetComponent<LevelButtonBehaviour>().Level = Levels.ElementAt(i);
                
                LevelButtons.Add(levelObj);
            }

            y += margin + prefabTransform.rect.height / 2;

            LevelContainer.sizeDelta = new Vector2(0, y);
        }

        private void LoadLevels()
        {
            if (!Directory.Exists(LevelXmlPath))
            {
                Console.WriteLine($"Level path'i ({LevelXmlPath}) bulunamadi.");
                return;
            }

            var levelConfs = new List<LevelConfiguration>();
            foreach (var file in Directory.GetFiles(LevelXmlPath, "*.xml"))
            {
                XDocument document = null;
                try
                {
                    document = XDocument.Load(file);
                }
                catch (Exception)
                {
                    Console.WriteLine($"{file} okunamadi.");
                    return;
                }

                var levelConf = new LevelConfiguration();
                levelConfs.Add(levelConf);

                //Number
                var numberValue = document.Root.Attribute("Number")?.Value;
                if (numberValue == null)
                {
                    Console.WriteLine("Number null.");
                    return;
                }
                int number;
                if (!int.TryParse(numberValue, out number))
                {
                    Console.WriteLine("Number okunamadi.");
                    return;
                }
                levelConf.LevelNumber = number;

                //PillCount
                var pillCountValue = document.Root.Element("PillCount")?.Value;
                if (pillCountValue == null)
                {
                    Console.WriteLine("PillCount null.");
                    return;
                }
                int pillCount;
                if (!int.TryParse(pillCountValue, out pillCount))
                {
                    Console.WriteLine("PillCount okunamadi.");
                    return;
                }
                levelConf.PillCount = pillCount;

                //Range
                var rangeValue = document.Root.Element("Range")?.Value;
                if (rangeValue == null)
                {
                    Console.WriteLine("Range null.");
                    return;
                }
                int[] range = new int[2];
                var rangeSplit = rangeValue.Split(',');
                for (int i = 0; i < 2; i++)
                {
                    int rangeSplitInt;
                    if (!int.TryParse(rangeSplit[i], out rangeSplitInt))
                    {
                        Console.WriteLine("Range okunamadi.");
                        return;
                    }
                    range[i] = rangeSplitInt;
                }
                levelConf.Range = range;

                //HealthyThreshold
                var healthyThresholdValue = document.Root.Element("HealthyThreshold")?.Value;
                if (healthyThresholdValue == null)
                {
                    Console.WriteLine("HealthyThreshold null.");
                    return;
                }
                int healthyThreshold;
                if (!int.TryParse(healthyThresholdValue, out healthyThreshold))
                {
                    Console.WriteLine("HealthyThreshold okunamadi.");
                    return;
                }
                levelConf.HealthyThreshold = healthyThreshold;

                //Diseases
                var diseaseElements = document.Root.Element("Diseases")?.Elements("Disease");
                if (diseaseElements == null)
                {
                    Console.WriteLine("Diseases null");
                    return;
                }
                levelConf.Diseases = new List<DiseaseConfiguration>();
                foreach (var diseaseElement in diseaseElements)
                {
                    //Type
                    var disease = new DiseaseConfiguration();
                    levelConf.Diseases.Add(disease);

                    var typeValue = diseaseElement.Attribute("Type")?.Value;
                    if (typeValue == null)
                    {
                        Console.WriteLine("Type null.");
                        return;
                    }
                    disease.Type = typeValue;

                    //Range
                    var dRangeValue = diseaseElement.Element("Range")?.Value;
                    if (dRangeValue == null)
                    {
                        Console.WriteLine("Range null.");
                        return;
                    }
                    int[] dRange = new int[2];
                    var dRangeSplit = dRangeValue.Split(',');
                    for (int i = 0; i < 2; i++)
                    {
                        int dRangeSplitInt;
                        if (!int.TryParse(dRangeSplit[i], out dRangeSplitInt))
                        {
                            Console.WriteLine("Range okunamadi.");
                            return;
                        }
                        dRange[i] = dRangeSplitInt;
                    }
                    disease.Range = dRange;


                    //InitialPoint
                    var initialPointValue = diseaseElement.Element("InitialPoint")?.Value;
                    if (initialPointValue == null)
                    {
                        Console.WriteLine("InitialPoint null.");
                        return;
                    }
                    int initialPoint;
                    if (!int.TryParse(initialPointValue, out initialPoint))
                    {
                        Console.WriteLine("InitialPoint okunamadi.");
                        return;
                    }
                    disease.InitialPoint = initialPoint;
                }
            }

            _levels = new List<Level>();
            foreach (var levelConf in levelConfs)
            {
                _levels.Add(Level.Load(levelConf));
            }
        }

        public void SelectLevel(int number)
        {
            SelectedLevel = Levels.FirstOrDefault(x => x.Number == number);

            GetComponent<Example02Scene>().SelectLevel(SelectedLevel);

            MainMenuPanel.SetActive(false);
            GamePanel.SetActive(true);
            CreditsPanel.SetActive(false);
            LevelSelectionPanel.SetActive(false);
            WinImage.SetActive(false);
            LoseImage.SetActive(false);
        }
	}
}
