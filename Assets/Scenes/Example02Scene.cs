using OverdoseTheGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples
{
	public class Example02Scene : MonoBehaviour
	{
		[SerializeField]
		Example02ScrollView scrollView;

		public Transform GamePanel;

		public GameObject BarPrefab;

		private List<Card> _cellData;

		private Level _currentLevel;

        public Sprite Heart;
        public Sprite Stomach;
        public Sprite Brain;
        public Sprite Bone;
        public Sprite Lung;

        private List<GameObject> _bars = new List<GameObject>();
        
        public Example02Scene()
		{
			_cellData = Enumerable.Range(0, 3)
				.Select(i => new Card())
				.ToList();
		}

		void Start()
		{
			scrollView.UpdateData(_cellData);
		}

		public void SelectLevel(Level level)
		{
			_currentLevel = level;
			//Önceki level'ın bar ve kartları destory edilmeldir

			//Barlar üretilecek
			GenerateBarUI();

			NextTurn();
		}

		public void NextTurn()
		{

			var pills = _currentLevel.GeneratePills();

			for (int i = 0; i < 3; i++)
			{
				_cellData[i].Pill = pills.ElementAt(i);
			}
		}

        public void UpdateBars()
        {
            for (int i = 0; i < _bars.Count; i++)
            {
                var bar = _bars[i];
                var disease = _currentLevel.Diseases.Values.ElementAt(i);

                var perc = (float)(disease.Point / disease.Range.Max);
                bar.GetComponent<Slider>().value = perc;

                SetBarImgColor(bar.transform.Find("Fill Area").GetComponentInChildren<Image>(), perc);
            }
        }

		internal void PillSelected(Pill pill)
		{
			var result = _currentLevel.ApplyEffects(pill);
            UpdateBars();

            if (result == DiseaseStatus.Healty)
            {
                GetComponent<GameManager>().WonLevel(_currentLevel);
                return;
            }
            else if (result == DiseaseStatus.Overdose)
            {
                GetComponent<GameManager>().LoseLevel(_currentLevel);
                return;
            }

            NextTurn();
        }

		private void GenerateBarUI()
		{
			var count = _currentLevel.Diseases.Count;
			float _xmin = 0.16f + (5 - count) * 0.04f;
			float _xmax = 0.84f - (5 - count) * 0.04f;
			for (int i = 0; i < count; i++)
			{
                var disease = _currentLevel.Diseases.ElementAt(i);

				//Instantiate
				var obj = GameObject.Instantiate(BarPrefab, GamePanel);

                _bars.Add(obj);

				//Linearly interpolate and set the RectTransform of the generated bars
				var trans = obj.GetComponent<RectTransform>();
				float perc = i / (float)(count - 1);
				float _x = Mathf.Lerp(_xmin, _xmax, perc);
				trans.anchorMin = new Vector2(_x, 0.766f);
				trans.anchorMax = new Vector2(_x, 0.766f);
				trans.anchoredPosition3D = Vector3.zero;

				//Set slider value to the current diseae value
				obj.GetComponent<Slider>().value = (float)(disease.Value.Point / disease.Value.Range.Max);

                switch (disease.Key)
                {
                    case DiseaseType.Bone:
                        obj.transform.GetChild(2).GetComponent<Image>().sprite = Bone;
                        break;
                    case DiseaseType.Brain:
                        obj.transform.GetChild(2).GetComponent<Image>().sprite = Brain;
                        break;
                    case DiseaseType.Heart:
                        obj.transform.GetChild(2).GetComponent<Image>().sprite = Heart;
                        break;
                    case DiseaseType.Lung:
                        obj.transform.GetChild(2).GetComponent<Image>().sprite = Lung;
                        break;
                    case DiseaseType.Stomach:
                        obj.transform.GetChild(2).GetComponent<Image>().sprite = Stomach;
                        break;
                }
			}
		}

		private void SetBarImgColor(Image img, float perc)
		{
			float r = Mathf.Lerp(50, 255, 1-perc);
			float g = Mathf.Lerp(50, 255, perc);
			img.color = new Color(r, g, 0, 140);
		}
	}
}
