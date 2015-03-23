using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;

namespace extra {

	public class StackPanelEx: Panel {
		Orientation orientation;

		public StackPanelEx() : this(Orientation.Horizontal) {
		}

		public StackPanelEx(Orientation orientation) {
			this.orientation = orientation;
		}

		public Margin margin {
			get {
				Margin res;
				GetMargin(out res.left, out res.top, out res.right, out res.bottom);
				return res;
			}
			set {
				SetMargin(value.left, value.top, value.right, value.bottom);
			}
		}

		protected override void ArrangeOverride(int width, int height) {
			bool fHorizontal = orientation == Orientation.Horizontal;
			int childPosition = 0;
			int nChildren = Children.Count;
			for (int i = 0; i < nChildren; i++) {
				var fLast = (i + 1) == nChildren;
				var child = Children[i];
				if (child.Visibility == Visibility.Collapsed) {
					continue;
				}
				var size = child.GetDesiredSize();
				if (fHorizontal) {
					child.Arrange(childPosition, 0, fLast ? width - childPosition : size.w, height);
					childPosition += size.w;
				} else {
					child.Arrange(0, childPosition, width, fLast ? height - childPosition : size.h);
					childPosition += size.h;
				}
			}
		}

		protected override void MeasureOverride(int availableWidth, int availableHeight, out int desiredWidth, out int desiredHeight) {
			desiredWidth = 0;
			desiredHeight = 0;
			bool fHorizontal = orientation == Orientation.Horizontal;
			int nChildren = Children.Count;
			for (int i = 0; i < nChildren; i++) {
				var child = Children[i];
				if (child.Visibility == Visibility.Collapsed) {
					continue;
				}
				
                if (fHorizontal) {
					child.Measure(availableWidth - desiredWidth, availableHeight);
					var size = child.GetDesiredSize();
					desiredWidth += size.w;
					desiredHeight = System.Math.Max(desiredHeight, size.h);
				} else {
					child.Measure(availableWidth, availableHeight - desiredHeight);
					var size = child.GetDesiredSize();
					desiredWidth = System.Math.Max(desiredWidth, size.w);
					desiredHeight += size.h;
				}
			}
			if (HorizontalAlignment == HorizontalAlignment.Stretch) {
				desiredWidth = availableWidth; //?
			}
		}
	}


}
