
using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;

namespace extra {
	
	public class PropValEdit: PropVal {
		protected readonly Button btn;
		public PropValEdit(string name, string val = null) : base(name, val) {
			btn = new Button("...") {
				//margin = new Margin(3, 3, 3, 3),
				HorizontalAlignment = HorizontalAlignment.Right
			};
			root.Children.Add(btn);
		}
	}
}