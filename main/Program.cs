using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerApp1
{
	public partial class Program
	{
		// This method is run when the mainboard is powered up or reset.   
		void ProgramStarted()
		{
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
		}
	}
}
