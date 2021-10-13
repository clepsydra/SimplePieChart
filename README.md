# SimplePieChart
A WPF Pie Chart which can also used to display rings and gauges

The radius is calculated as half of the width.
The center is the center of the control.

DataPoints are set as a `IReadOnlyList<DataPoint>`. They contain the values and colors.

By default the `InnerRadius` is set to 0, the `StartAngle` is 0 and the `EndAngle` is set to 360. By this it shows up as a circle.

By changing the `InnerRadius` a ring can be drawn.

And by changing the `StartAngle` and `EndAngle` to e.g. -130 and 130 a gauge like appearance can be achieved. 

_____

Usage
-----

To use it copy the content of the `Control` folder: It contains the control and the `DataPoint` class.
