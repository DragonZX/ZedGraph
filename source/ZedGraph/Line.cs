using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ZedGraph
{
	/// <summary>
	/// A class representing all the characteristics of the <see cref="Line"/>
	/// segments that make up a curve on the graph.
	/// </summary>
	public class Line : ICloneable
	{
		/// <summary>
		/// Private field that stores the pen width for this
		/// <see cref="Line"/>.  Use the public
		/// property <see cref="Width"/> to access this value.
		/// </summary>
		private float width;
		/// <summary>
		/// Private field that stores the <see cref="DashStyle"/> for this
		/// <see cref="Line"/>.  Use the public
		/// property <see cref="Style"/> to access this value.
		/// </summary>
		private DashStyle style;
		/// <summary>
		/// Private field that stores the visibility of this
		/// <see cref="Line"/>.  Use the public
		/// property <see cref="IsVisible"/> to access this value.
		/// </summary>
		private bool isVisible;
		/// <summary>
		/// Private field that stores the color of this
		/// <see cref="Line"/>.  Use the public
		/// property <see cref="Color"/> to access this value.  If this value is
		/// false, the line will not be shown (but the <see cref="Symbol"/> may
		/// still be shown).
		/// </summary>
		private Color color;
		/// <summary>
		/// Private field that stores the <see cref="ZedGraph.StepType"/> for this
		/// <see cref="CurveItem"/>.  Use the public
		/// property <see cref="StepType"/> to access this value.
		/// </summary>
		private StepType	stepType;
		
		/// <summary>
		/// Default constructor that sets all <see cref="Line"/> properties to default
		/// values as defined in the <see cref="Def"/> class.
		/// </summary>
		public Line()
		{
			this.width = Def.Lin.Width;
			this.style = Def.Lin.Style;
			this.isVisible = Def.Lin.IsVisible;
			this.color = Def.Lin.Color;
			this.stepType = Def.Lin.Type;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The Line object from which to copy</param>
		public Line( Line rhs )
		{
			width = rhs.Width;
			style = rhs.Style;
			isVisible = rhs.IsVisible;
			color = rhs.Color;
			stepType = rhs.StepType;
		}

		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the Line</returns>
		public object Clone()
		{ 
			return new Line( this ); 
		}

		/// <summary>
		/// The color of the <see cref="Line"/>
		/// </summary>
		public Color Color
		{
			get { return color; }
			set { color = value; }
		}
		/// <summary>
		/// The style of the <see cref="Line"/>, defined as a <see cref="DashStyle"/> enum.
		/// This allows the line to be solid, dashed, or dotted.
		/// </summary>
		public DashStyle Style
		{
			get { return style; }
			set { style = value;}
		}
		/// <summary>
		/// The pen width used to draw the <see cref="Line"/>, in pixel units
		/// </summary>
		public float Width
		{
			get { return width; }
			set { width = value; }
		}
		/// <summary>
		/// Gets or sets a property that shows or hides the <see cref="Line"/>.
		/// </summary>
		/// <value>true to show the line, false to hide it</value>
		public bool IsVisible
		{
			get { return isVisible; }
			set { isVisible = value; }
		}
		/// <summary>
		/// Determines if the <see cref="CurveItem"/> will be drawn by directly connecting the
		/// points from the <see cref="CurveItem.X"/> and <see cref="CurveItem.Y"/> data arrays,
		/// or if the curve will be a "stair-step" in which the points are
		/// connected by a series of horizontal and vertical lines that
		/// represent discrete, constant values.  Note that the values can
		/// be forward oriented <c>ForwardStep</c> (<see cref="ZedGraph.StepType"/>) or
		/// rearward oriented <c>RearwardStep</c>.
		/// That is, the points are defined at the beginning or end
		/// of the constant value for which they apply, respectively.
		/// </summary>
		/// <value><see cref="ZedGraph.StepType"/> enum value</value>
		public StepType StepType
		{
			get { return stepType; }
			set { stepType = value;}
		}		

		/// <summary>
		/// Render a single <see cref="Line"/> segment to the specified
		/// <see cref="Graphics"/> device.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="x1">The x position of the starting point that defines the
		/// line segment in screen pixel units</param>
		/// <param name="y1">The y position of the starting point that defines the
		/// line segment in screen pixel units</param>
		/// <param name="x2">The x position of the ending point that defines the
		/// line segment in screen pixel units</param>
		/// <param name="y2">The y position of the ending point that defines the
		/// line segment in screen pixel units</param>
		public void Draw( Graphics g, float x1, float y1, float x2, float y2 )
		{
			if ( this.isVisible )
			{
				Pen pen = new Pen( this.color, this.width );
				pen.DashStyle = this.Style;
				g.DrawLine( pen, x1, y1, x2, y2 );
			}
		}

		/// <summary>
		/// Render a series of <see cref="Line"/> segments to the specified
		/// <see cref="Graphics"/> device.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="x">The array of x position values that define the
		/// line segments in screen pixel units</param>
		/// <param name="y">The array of y position values that define the
		/// line segments in screen pixel units</param>

		public void DrawMany( Graphics g, float[] x, float[] y )
		{
			if ( this.isVisible )
			{
				Pen pen = new Pen( this.color, this.width );
				pen.DashStyle = this.Style;
				int nSeg = x.Length - 1;
				int iPlus = 0;

				for ( int i=0; i<nSeg; i++ )
				{
					iPlus++;
					if ( x[i] != System.Single.MaxValue && 
						x[iPlus] != System.Single.MaxValue && 
						y[i] != System.Single.MaxValue && 
						y[iPlus] != System.Single.MaxValue )
					{
						if ( this.StepType == StepType.NonStep )
						{
							g.DrawLine( pen, x[i], y[i], x[iPlus], y[iPlus] );
						}
						else if ( this.StepType == StepType.ForwardStep )
						{
							g.DrawLine( pen, x[i], y[i], x[iPlus], y[i] );
							g.DrawLine( pen, x[iPlus], y[i], x[iPlus], y[iPlus] );
						}
						else if ( this.StepType == StepType.RearwardStep )
						{
							g.DrawLine( pen, x[i], y[i], x[i], y[iPlus] );
							g.DrawLine( pen, x[i], y[iPlus], x[iPlus], y[iPlus] );
						}
					}
				}
			}
		}
	}
}