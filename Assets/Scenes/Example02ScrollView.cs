using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	public class Example02ScrollView : FancyScrollView<Card, Example02ScrollViewContext>
	{
		[SerializeField]
		ScrollPositionController scrollPositionController;
		[SerializeField]
		GameObject GameManager;

		OverdoseTheGame.GameManager _manager;

		public Example02Scene scene;

		new void Awake()
		{
			scrollPositionController.OnUpdatePosition.AddListener(UpdatePosition);
			// Add OnItemSelected event listener
			scrollPositionController.OnItemSelected.AddListener(CellSelected);

			SetContext(new Example02ScrollViewContext { OnPressedCell = OnPressedCell });
			_manager = GameManager.GetComponent<OverdoseTheGame.GameManager>();
			base.Awake();
		}

		public void UpdateData(List<Card> data)
		{
			cellData = data;
			scrollPositionController.SetDataCount(cellData.Count);
			UpdateContents();
		}

		void OnPressedCell(Example02ScrollViewCell cell)
		{
			scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
			context.SelectedIndex = cell.DataIndex;
			UpdateContents();
		}

		// An event triggered when a cell is selected.
		void CellSelected(int cellIndex)
		{
			//scene.PillSelected(cellData[cellIndex].Pill);
			// Update context.SelectedIndex and call UpdateContents for updating cell's content.
			context.SelectedIndex = cellIndex;
			//_manager.SelectedCardIndex = cellIndex;
			UpdateContents();
		}

		public void ConfirmSelect()
		{
			scene.PillSelected(cellData[context.SelectedIndex].Pill);
			UpdateContents();
		}


	}
}
