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

namespace cooling.system.controller {
	using extra;
	using System.Text;
	using ghi = GHIElectronics.Gadgeteer;

	public partial class Program {

		PWM pwm2;
		DispatcherTimer timer;
		//async void X() {

		//}

		
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

			//var mainFanView = new PropValEdit("fan main");
			//var intFanView = new PropValEdit("fan internal", "off");
			//var sysTempView = new PropVal("temp system");
			//var cpuTempView = new PropVal("temp cpu");
			//var p1TempView = new PropVal("temp p1");

			disp.SimpleGraphics.Clear();
			disp.SimpleGraphics.DisplayEllipse(GT::Color.Red, 1, GT::Color.Green, 10, 10, 10, 10);
			disp.SimpleGraphics.Redraw();
			Window window = disp.WPFWindow;
			//var root = new StackPanelEx(Orientation.Vertical) {
			//	Children = {
			//		mainFanView, intFanView, sysTempView, cpuTempView, p1TempView
			//	}
			//};

			//window.Child = root;

			var view = new MainView();
			view.Show(window);
			

			
		

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


			//root.Children.Add(speedContorl);
			
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

				view.sysTempView.valTxt.TextContent = String.Concat(
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
				view.mainFanView.valTxt.TextContent = mainFanVal.ToString();

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
			//timer.Start();
        }

		void OnRender(object sender, EventArgs args) {

		}
	}
}
