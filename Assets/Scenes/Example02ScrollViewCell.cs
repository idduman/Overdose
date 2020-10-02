using OverdoseTheGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples
{
    public class Example02ScrollViewCell
        : FancyScrollViewCell<Card, Example02ScrollViewContext>
    {
        [SerializeField]
        Animator animator;
        [SerializeField]
        Text message;
        [SerializeField]
        Image image;
        [SerializeField]
        Button button;
        [SerializeField]
        Vector3 scale;

        public Sprite Heart;
        public Sprite Stomach;
        public Sprite Brain;
        public Sprite Bone;
        public Sprite Lung;

        readonly int scrollTriggerHash = Animator.StringToHash("scroll");
        Example02ScrollViewContext context;

        public Sprite[] CardIcons;
        
        void Start()
        {
            var rectTransform = transform as RectTransform;
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.localScale = scale;
            UpdatePosition(0);

            button.onClick.AddListener(OnPressedCell);

            image.rectTransform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// コンテキストを設定します
        /// </summary>
        /// <param name="context"></param>
        public override void SetContext(Example02ScrollViewContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// セルの内容を更新します
        /// </summary>
        /// <param name="itemData"></param>
        public override void UpdateContent(Card itemData)
        {
            if (!itemData.IsInitialized)
            {
                var iconIndex = (int)Random.Range(0, CardIcons.Length);

                image.transform.Find("Icon").GetComponent<Image>().sprite = CardIcons[iconIndex];

                var imageTextPair = new List<Tuple<Text, Image>>();
                imageTextPair.Add(new Tuple<Text, Image>(
                    image.transform.Find("Text0").GetComponent<Text>()
                    , image.transform.Find("Image0").GetComponent<Image>()));

                imageTextPair.Add(new Tuple<Text, Image>(
                    image.transform.Find("Text1").GetComponent<Text>()
                    , image.transform.Find("Image1").GetComponent<Image>()));

                imageTextPair.Add(new Tuple<Text, Image>(
                    image.transform.Find("Text2").GetComponent<Text>()
                    , image.transform.Find("Image2").GetComponent<Image>()));

                if (itemData.Pill != null)
                {
                    imageTextPair.ForEach(x =>
                    {
                        x.Item1.gameObject.SetActive(false);
                        x.Item2.gameObject.SetActive(false);
                    });

                    var effects = itemData.Pill.Effects.Where(x => x.Item2 != 0).ToList();
                    for (int i = 0; i < effects.Count; i++)
                    {
                        var effect = effects[i];
                        var pair = imageTextPair[i];
                        if (effect.Item2 != 0)
                        {
                            pair.Item1.gameObject.SetActive(true);
                            pair.Item2.gameObject.SetActive(true);

                            pair.Item1.text = $"{(effect.Item2 > 0 ? "+" : "")}{effect.Item2.ToString()}";
                            pair.Item1.color = effect.Item2 > 0 ? new Color(0xC4 / 255f, 0x2E / 255f, 0x26 / 255f) : new Color(0x00 / 255f, 0xA5 / 255f, 0x6D / 255f);
                            switch (effect.Item1)
                            {
                                case DiseaseType.Bone:
                                    pair.Item2.sprite = Bone;
                                    break;
                                case DiseaseType.Brain:
                                    pair.Item2.sprite = Brain;
                                    break;
                                case DiseaseType.Heart:
                                    pair.Item2.sprite = Heart;
                                    break;
                                case DiseaseType.Lung:
                                    pair.Item2.sprite = Lung;
                                    break;
                                case DiseaseType.Stomach:
                                    pair.Item2.sprite = Stomach;
                                    break;
                            }
                        }
                    }
                }
                itemData.IsInitialized = true;
            }

            if (context != null)
            {
                var isSelected = context.SelectedIndex == DataIndex;
                if (isSelected)
                {
                    image.color = new Color32(255, 255, 255, 255);
                    transform.SetAsLastSibling();
                }
                else
                {
                    image.color = new Color32(255, 255, 255, 120);
                    transform.SetAsFirstSibling();
                }
                /*image.color = isSelected
                    ? new Color32(0, 255, 255, 100)
                    : new Color32(255, 255, 255, 77);*/

            }
        }

        /// <summary>
        /// セルの位置を更新します
        /// </summary>
        /// <param name="position"></param>
        public override void UpdatePosition(float position)
        {
            animator.Play(scrollTriggerHash, -1, position);
            animator.speed = 0;
        }

        public void OnPressedCell()
        {
            if (context != null)
            {
                context.OnPressedCell(this);
            }
        }
    }
}
