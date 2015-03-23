using Microsoft.SPOT.Presentation;

namespace extra {

	public static class UIElementExtensions {
		public static Size GetDesiredSize(this UIElement element) {
			int w, h;
			element.GetDesiredSize(out w, out h);
			return new Size(w, h);
		}
	}

}
