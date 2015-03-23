namespace extra {
	public struct Margin {

		public int left;
		public int top;
		public int right;
		public int bottom;

		public Margin(int left, int top, int right, int bottom) {
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public Margin(int leftRight, int topBottom) {
			this.left = leftRight;
			this.top = topBottom;
			this.right = leftRight;
			this.bottom = topBottom;
		}

		public Margin(int val) {
			this.left = val;
			this.top = val;
			this.right = val;
			this.bottom = val;
		}
	}
}