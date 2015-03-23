using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;

namespace extra {

	public class PropVal: ContentControl {
		public readonly Text nameTxt;
		public readonly Text valTxt;
		public readonly StackPanelEx root;

		public PropVal(string name, string val = null) {

			nameTxt = new Text {
				Font = Fonts.small,
				TextContent = name + ":",
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left
			};

			valTxt = new Text {
				Font = Fonts.small,
				TextContent = val,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Trimming = TextTrimming.CharacterEllipsis
			};

			valTxt.SetMargin(3, 0, 0, 0);
			root = new StackPanelEx(Orientation.Horizontal) {
				margin = new Margin(3),
				Children = { nameTxt, valTxt },
				HorizontalAlignment = HorizontalAlignment.Stretch
			};

			Child = root;
		}


	}

}