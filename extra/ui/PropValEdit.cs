using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;

namespace extra {

	public class Button: ContentControl {
		public event EventHandler onClick;
		
		readonly Border root;
		readonly Text text;
		bool captured;
		bool clickOnUp;

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

		public string caption {
			get {
				return text.TextContent;
			}
			set {
				text.TextContent = value;
			}
		}

		

		public Button(string caption) {
			text = new Text() {
				//Font = Resources.GetFont(Resources.FontResources.NinaB),
				Font = Fonts.small,
				Trimming = TextTrimming.CharacterEllipsis,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				TextContent = caption,
				ForeColor = Colors.Black
			};

			//text.SetMargin(6);
			root = new Border() {
				Height = 20,
				Width = 40,
				Child = text,
				//HorizontalAlignment = HorizontalAlignment.Left,
				Background = Brushes.lightGray,
				Foreground = Brushes.black
			};

			root.TouchDown += OnTouchDown;
			root.TouchUp += OnTouchUp;
			root.TouchMove += OnTouchMove;
            Child = root;
		}

		protected override void MeasureOverride(int availableWidth, int availableHeight, out int desiredWidth, out int desiredHeight) {
			base.MeasureOverride(availableWidth, availableHeight, out desiredWidth, out desiredHeight);
		}

		void NotifyOnClick() {
			var cb = onClick;
			if (cb != null) {
				try {
					cb(this, new EventArgs());
				} catch {
					//swallow exception
				}
				
			}
		}

		void OnTouchDown(object sender, TouchEventArgs args) {
			args.Handled = true;
			root.Background = Brushes.darkGray;
			TouchCapture.Capture(root);
			captured = true;
			clickOnUp = true;
        }

		void OnTouchUp(object sender, TouchEventArgs args) {
			if(!captured || TouchCapture.Captured != root) {
				return;
			}
			args.Handled = true;
			TouchCapture.Capture(root, CaptureMode.None);
			captured = false;
			if (clickOnUp) {
				root.Background = Brushes.lightGray;
				clickOnUp = false;
				NotifyOnClick();
            }
		}

		void OnTouchMove(object sender, TouchEventArgs args) {
			if (TouchCapture.Captured != root) {
				return;
			}
			args.Handled = true;
			var touches = args.Touches;
            if (touches == null || touches.Length == 0) {
				return;
			}
			var ti = touches[touches.Length - 1];
			int x = ti.X;
			int y = ti.Y;
			root.PointToClient(ref x, ref y);
            if(root.ContainsPoint(x,y)) {
				if (!clickOnUp) {
					root.Background = Brushes.darkGray;
					clickOnUp = true;
				}
			}else {
				if (clickOnUp) {
					root.Background = Brushes.lightGray;
					clickOnUp = false;
				}
			}
		}
	}
}