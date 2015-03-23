using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

using GHI.Pins;

namespace GadgeteerApp1 {
	using extra;
	using System.Text;

	public partial class Program {

		PWM pwm2;
		DispatcherTimer timer;
		//async void X() {

		//}

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
		}

		public class MyUiElement : UIElement {
			protected override void MeasureOverride(int availableWidth, int availableHeight, out int desiredWidth, out int desiredHeight) {
				desiredWidth = 50;
				desiredHeight = 50;
			}
			public override void OnRender(DrawingContext dc) {
				base.OnRender(dc);
			}
			protected override void RenderRecursive(DrawingContext dc) {
				base.RenderRecursive(dc);
			}
		}

		class Button: ContentControl {
			readonly Border root;
			readonly Text text;
			
			//public delegate void Action();
			//public delegate void Action<T>(T arg);

			//public event Action<Button> onClick;

			public Margin margin {
				get {
					Margin res;
					root.GetMargin(out res.left, out res.top, out res.right, out res.bottom);
					return res;
				}
				set {
					root.SetMargin(value.left, value.top, value.right, value.bottom);
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
					Font = Fonts.ninaB,
					TextContent = caption,
					ForeColor = Colors.Black
				};
				text.TouchDown += (s, a) => {
					root.Background = new SolidColorBrush(Colors.Brown);
				};

				var textQ = new MyUiElement() {

				};
				text.SetMargin(16);
				root = new Border() {
					Child = textQ,
                    Background = new SolidColorBrush(Colors.White),
					Foreground = new SolidColorBrush(Colors.Black)
				};

				root.TouchDown += OnTouchDown;
				root.TouchUp += OnTouchUp;
				Child = root;
				//root.TouchGestureEnd += OnTouchGestureEnd;
				//root.TouchGestureStart += OnTouchGestureStart;
				//text.TouchDown += (s, a) => {
				//	root.Background = new SolidColorBrush(Colors.);
				//};
			}

			bool touched = false;

			SolidColorBrush white = new SolidColorBrush(Colors.White);
			SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
			SolidColorBrush blue = new SolidColorBrush(Colors.Blue);

			void OnTouchDown(object sender, TouchEventArgs args) {
				args.Handled = true;
				root.Background = touched ? white : blue;
				touched = !touched;
				TouchCapture.Capture(root);
            }
			void OnTouchUp(object sender, EventArgs args) {
				root.Background = yellow;
				TouchCapture.Capture(root, CaptureMode.None);
            }

			//void OnTouchGestureStart(object sender, EventArgs args) {
			//	root.Background = new SolidColorBrush(Colors.Green);
			//}

			//void OnTouchGestureEnd(object sender, EventArgs args) {
			//	root.Background = new SolidColorBrush(Colors.Red);
			//}

			public void SetMargin(int left, int top, int right, int bottom) {
				root.SetMargin(left, top, right, bottom);
            }



		}

		public class InputButton {
			public delegate void Action();
			bool clickOnUp = false;
			InputPort port;
			public event Action onClick;
			public InputButton(Cpu.Pin cpuPin) {
				port = new InputPort(cpuPin, false, Port.ResistorMode.PullUp);
            }

			void NotifyOnClick() {
				var cb = onClick;
				if(cb!= null) {
					cb();
				}
			}

			public void Process() {
				var val = port.Read();
				if (clickOnUp && val) {
					NotifyOnClick();
				}
				clickOnUp = !val;
			}
		}

		// This method is run when the mainboard is powered up or reset.   
		void ProgramStarted() {
			//Configuration.LCD.Set(lcdConfig);

			/*******************************************************************************************
			Modules added in the Program.gadgeteer designer view are used by typing 
			their name followed by a period, e.g.  button.  or  camera.
			
			Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
				button.ButtonPressed +=<tab><tab>
			
			If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
				GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
				timer.Tick +=<tab><tab>
				timer.Start();
			*******************************************************************************************/


			// Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
			//disp.DebugPrintEnabled = true;
			//Debug.Print("Program Started");
			//disp.WPFWindow.Child = new Text() {
			//	TextContent = "hhsdsadhh"
			//};

			//disp.WPFWindow.Visibility = Visibility.Visible;
			//Bitmap LCD = new Bitmap(SystemMetrics.ScreenWidth, SystemMetrics.ScreenHeight);
			//byte red = 0;
			//int x = 0;
			//while (true) {
			//	for (x = 30; x < SystemMetrics.ScreenWidth - 30; x += 10) {
			//		LCD.DrawEllipse(ColorUtility.ColorFromRGB(red, 10, 10), x, 100, 30, 40);
			//		LCD.Flush();
			//		red += 3;
			//		Thread.Sleep(10);
			//	}

			//}

			disp.SimpleGraphics.Clear();
			disp.SimpleGraphics.DisplayEllipse(GT::Color.Red, 1, GT::Color.Green, 10, 10, 10, 10);
			disp.SimpleGraphics.Redraw();
			Window window = disp.WPFWindow;
			var root = new StackPanelEx(Orientation.Vertical);
			window.Child = root;

			//system info
			var sysInfo = new Text() {
				Font = Fonts.small,
				TextContent = String.Concat(
					Mainboard.MainboardName, " ",
					Mainboard.MainboardVersion, ", ",
					(Cpu.SystemClock / 1000 / 1000).ToString() + " Mhz"
				),
				ForeColor = Colors.Blue
			};
			sysInfo.SetMargin(3);
            root.Children.Add(sysInfo);
			
			var mainFanView = new PropValEdit("fan main");
			root.Children.Add(mainFanView);

			var intFanView = new PropValEdit("fan internal", "off");
			root.Children.Add(intFanView);

			var sysTempView = new PropVal("temp system");
			root.Children.Add(sysTempView);

			var cpuTempView = new PropVal("temp cpu");
			root.Children.Add(cpuTempView);

			var p1TempView = new PropVal("temp p1");
			root.Children.Add(p1TempView);

			//var analogLbl = new Text() {
			//	Font = Resources.GetFont(Resources.FontResources.NinaB),
			//	TextContent = "analog: unknown",
			//	ForeColor = Colors.Black,
			//	VerticalAlignment = VerticalAlignment.Center
			//};
			//analogLbl.SetMargin(10, 10, 10, 10);

			//root.Children.Add(analogLbl);

			//var fanPwmLbl = new Text() {
			//	Font = Resources.GetFont(Resources.FontResources.NinaB),
			//	TextContent = "fan PWM load: unknown",
			//	ForeColor = Colors.Black
			//};
			//root.Children.Add(fanPwmLbl);

			var speedContorl = new StackPanel(Orientation.Horizontal);
			var incSpeedBtn = new Text() {
				Font = Fonts.ninaB,
				TextContent = "+",
				ForeColor = Colors.Red
			};
			incSpeedBtn.TouchUp += (s, a) => {
				pwm2.Duration += 1;
			};
			speedContorl.Children.Add(incSpeedBtn);
			var decSpeedBtn = new Text() {
				Font = Fonts.ninaB,
				TextContent = "-",
				ForeColor = Colors.Red
			};
			decSpeedBtn.TouchUp += (s, a) => {
				pwm2.Duration -= 1;
			};
			speedContorl.Children.Add(decSpeedBtn);

			var tttt = new Button("+") {
				margin = new Margin(10, 10, 0, 0)
			};
			speedContorl.Children.Add(tttt);


			root.Children.Add(speedContorl);
			
			window.Visibility = Visibility.Visible;

			var lightSensor = new AnalogInput(Cpu.AnalogChannel.ANALOG_7);
			var analog = new AnalogInput(Cpu.AnalogChannel.ANALOG_6);
			pwm2 = new PWM(Cpu.PWMChannel.PWM_2, 1000, 800, PWM.ScaleFactor.Nanoseconds, false);
			pwm2.Start();


			var btn3 = new InputButton(G120.P2_10);
			btn3.onClick += () => {
				if (pwm2.Duration < pwm2.Period) {
					pwm2.Duration += pwm2.Period / 100;
				}
			};
			var btn4 = new InputButton(G120.P0_22);
			btn4.onClick += () => {
				if (pwm2.Duration > 0) {
					pwm2.Duration -= pwm2.Period / 100;
				}
			};

			var btn2 = new InputPort(G120.P0_25, false, Port.ResistorMode.PullUp);
			var btn1 = new InputPort(G120.P0_26, false, Port.ResistorMode.PullUp);

			//while (true) {
			//	if (btn1.Read() == true) {
			//		LED.Write(true);
			//		Thread.Sleep(500);
			//		LED.Write(false);
			//		Thread.Sleep(500);
			//	}
			//}

			var b1_click_when_up = false;
			var b2_click_when_up = false;

		
			var dispatcher = window.Dispatcher;
			timer = new DispatcherTimer(dispatcher);
			timer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 100);
			var analogBuf = new double[20];
			for(var i=0; i<analogBuf.Length; i+=1) {
				analogBuf[i] = analog.Read();
			}
			var analogNext = 0;
			timer.Tick += (s, a) => {

				btn3.Process();
				btn4.Process();

				//update temperature
				analogBuf[analogNext] = analog.Read();
				analogNext = (analogNext + 1) % analogBuf.Length;
				var analogAvg = 0.0;
				for (var i = 0; i < analogBuf.Length; i += 1) {
					analogAvg += analogBuf[i];
				}
				analogAvg /= analogBuf.Length;

				var Temp = System.Math.Log(10000/analogAvg - 10000);
				var TempK = 1 / (0.001129148 + (0.000234125 + 0.0000000876741 * Temp * Temp) * Temp);
				var TempC = TempK - 273.15;
				
                sysTempView.valTxt.TextContent = String.Concat(
					TempC.ToString("F1"), " C°",
					" (", (analogAvg * 3.3).ToString("F2"), "V)"
				);

				//????????
				double lightSensorReading = lightSensor.Read();
				var str = lightSensorReading.ToString("F4");
				//Debug.Print(str);
				//memTxt.TextContent = (Debug.GC(true)/1024).ToString() + " KB";
				var b1 = btn1.Read();
				var b2 = btn2.Read();


				//main fan (5000 rpm, 100 mAm, 2.6V, 80%, 100KHz)
				var mainFanVal = new StringBuilder();
                var mainFanLoad = pwm2.Duration * 100 / pwm2.Period;

				mainFanVal.Append(mainFanLoad);
				mainFanVal.Append("%, ");

				var mainFanPwmFreq = pwm2.Frequency;
				if (mainFanPwmFreq < 1000) {
					mainFanVal.Append((int)pwm2.Frequency);
					mainFanVal.Append(" Hz");
				} else if (mainFanPwmFreq < 1000000) {
					mainFanVal.Append((int)(pwm2.Frequency / 1000.0));
					mainFanVal.Append(" Khz");
				} else if (mainFanPwmFreq < 1000000000) {
					mainFanVal.Append((int)(pwm2.Frequency / 1000000.0));
					mainFanVal.Append(" Mhz");
				} else {
					mainFanVal.Append((int)(pwm2.Frequency / 1000000000.0));
					mainFanVal.Append(" Ghz");
				}
				mainFanView.valTxt.TextContent = mainFanVal.ToString();

				if (b1_click_when_up && b1) {
					if (pwm2.Duration < pwm2.Period) {
						var step = pwm2.Period / 100;
						if (pwm2.Duration + step < pwm2.Period) {
							pwm2.Duration += step;
						} else {
							pwm2.Duration = pwm2.Period;
						}
						
                    }
                }
				b1_click_when_up = !b1;

				if (b2_click_when_up && b2) {
					if (pwm2.Duration > 0) {
						var step = pwm2.Period / 100;
						if (pwm2.Duration > step) {
							pwm2.Duration -= step;
						} else {
							pwm2.Duration = 0;
						}
					}
				}
				b2_click_when_up = !b2;

			};
			timer.Start();
        }

		void OnRender(object sender, EventArgs args) {

		}
	}
}
