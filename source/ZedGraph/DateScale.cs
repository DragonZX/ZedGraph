//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright (C) 2005  John Champion
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections;
using System.Text;
using System.Drawing;

namespace ZedGraph
{
	/// <summary>
	/// The DateScale class inherits from the <see cref="Scale" /> class, and implements
	/// the features specific to <see cref="AxisType.Date" />.
	/// </summary>
	/// <remarks>
	/// DateScale is a cartesian axis with calendar dates or times.  The actual data values should
	/// be created with the <see cref="XDate" /> type, which is directly translatable to a
	/// <see cref="System.Double" /> type for storage in the point value arrays.
	/// </remarks>
	/// 
	/// <author> John Champion  </author>
	/// <version> $Revision: 1.3 $ $Date: 2006-02-08 05:35:12 $ </version>
	class DateScale : Scale
	{

	#region constructors

		public DateScale( Axis parentAxis )
			: base( parentAxis )
		{
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see cref="DateScale" /> object from which to copy</param>
		public DateScale( Scale rhs )
			: base( rhs )
		{
		}
		
		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the <see cref="DateScale" /> object.</returns>
		public object Clone()
		{
			return new DateScale( this ); 
		}

	#endregion

	#region properties

		/// <summary>
		/// Return the <see cref="AxisType" /> for this <see cref="Scale" />, which is
		/// <see cref="AxisType.Date" />.
		/// </summary>
		public override AxisType Type
		{
			get { return AxisType.Date; }
		}

		/// <summary>
		/// Gets or sets the minimum value for this scale.
		/// </summary>
		/// <remarks>
		/// The set property is specifically adapted for <see cref="AxisType.Date" /> scales,
		/// in that it automatically limits the value to the range of valid dates for the
		/// <see cref="XDate" /> struct.
		/// </remarks>
		public override double Min
		{
			get { return this.min; }
			set { this.min = XDate.MakeValidDate( value ); }
		}

		/// <summary>
		/// Gets or sets the maximum value for this scale.
		/// </summary>
		/// <remarks>
		/// The set property is specifically adapted for <see cref="AxisType.Date" /> scales,
		/// in that it automatically limits the value to the range of valid dates for the
		/// <see cref="XDate" /> struct.
		/// </remarks>
		public override double Max
		{
			get { return this.max; }
			set { this.max = XDate.MakeValidDate( value ); }
		}
	#endregion

	#region methods

		/// <summary>
		/// Determine the value for any major tic.
		/// </summary>
		/// <remarks>
		/// This method properly accounts for <see cref="Scale.IsLog"/>, <see cref="Scale.IsText"/>,
		/// and other axis format settings.
		/// </remarks>
		/// <param name="baseVal">
		/// The value of the first major tic (floating point double)
		/// </param>
		/// <param name="tic">
		/// The major tic number (0 = first major tic).  For log scales, this is the actual power of 10.
		/// </param>
		/// <returns>
		/// The specified major tic value (floating point double).
		/// </returns>
		override internal double CalcMajorTicValue( double baseVal, double tic )
		{
			XDate xDate = new XDate( baseVal );

			switch ( this.majorUnit )
			{
				case DateUnit.Year:
				default:
					xDate.AddYears( tic * this.step );
					break;
				case DateUnit.Month:
					xDate.AddMonths( tic * this.step );
					break;
				case DateUnit.Day:
					xDate.AddDays( tic * this.step );
					break;
				case DateUnit.Hour:
					xDate.AddHours( tic * this.step );
					break;
				case DateUnit.Minute:
					xDate.AddMinutes( tic * this.step );
					break;
				case DateUnit.Second:
					xDate.AddSeconds( tic * this.step );
					break;
			}

			return xDate.XLDate;
		}

		/// <summary>
		/// Determine the value for any minor tic.
		/// </summary>
		/// <remarks>
		/// This method properly accounts for <see cref="Scale.IsLog"/>, <see cref="Scale.IsText"/>,
		/// and other axis format settings.
		/// </remarks>
		/// <param name="baseVal">
		/// The value of the first major tic (floating point double).  This tic value is the base
		/// reference for all tics (including minor ones).
		/// </param>
		/// <param name="iTic">
		/// The major tic number (0 = first major tic).  For log scales, this is the actual power of 10.
		/// </param>
		/// <returns>
		/// The specified minor tic value (floating point double).
		/// </returns>
		override internal double CalcMinorTicValue( double baseVal, int iTic )
		{
			XDate xDate = new XDate( baseVal );

			switch ( this.minorUnit )
			{
				case DateUnit.Year:
				default:
					xDate.AddYears( (double) iTic * this.minorStep );
					break;
				case DateUnit.Month:
					xDate.AddMonths( (double) iTic * this.minorStep );
					break;
				case DateUnit.Day:
					xDate.AddDays( (double) iTic * this.minorStep );
					break;
				case DateUnit.Hour:
					xDate.AddHours( (double) iTic * this.minorStep );
					break;
				case DateUnit.Minute:
					xDate.AddMinutes( (double) iTic * this.minorStep );
					break;
				case DateUnit.Second:
					xDate.AddSeconds( (double) iTic * this.minorStep );
					break;
			}

			return xDate.XLDate;
		}

		/// <summary>
		/// Internal routine to determine the ordinals of the first minor tic mark
		/// </summary>
		/// <param name="baseVal">
		/// The value of the first major tic for the axis.
		/// </param>
		/// <returns>
		/// The ordinal position of the first minor tic, relative to the first major tic.
		/// This value can be negative (e.g., -3 means the first minor tic is 3 minor step
		/// increments before the first major tic.
		/// </returns>
		override internal int CalcMinorStart( double baseVal )
		{
			switch ( this.minorUnit )
			{
				case DateUnit.Year:
				default:
					return (int) ( ( this.min - baseVal ) / ( 365.0 * this.minorStep ) );
				case DateUnit.Month:
					return (int) ( ( this.min - baseVal ) / ( 28.0 * this.minorStep ) );
				case DateUnit.Day:
					return (int) ( ( this.min - baseVal ) / this.minorStep );
				case DateUnit.Hour:
					return (int) ( ( this.min - baseVal ) * XDate.HoursPerDay / this.minorStep );
				case DateUnit.Minute:
					return (int) ( ( this.min - baseVal ) * XDate.MinutesPerDay / this.minorStep );
				case DateUnit.Second:
					return (int) ( ( this.min - baseVal ) * XDate.SecondsPerDay / this.minorStep );
			}
		}

		/// <summary>
		/// Determine the value for the first major tic.
		/// </summary>
		/// <remarks>
		/// This is done by finding the first possible value that is an integral multiple of
		/// the step size, taking into account the date/time units if appropriate.
		/// This method properly accounts for <see cref="Scale.IsLog"/>, <see cref="Scale.IsText"/>,
		/// and other axis format settings.
		/// </remarks>
		/// <returns>
		/// First major tic value (floating point double).
		/// </returns>
		override internal double CalcBaseTic()
		{
			if ( this.baseTic != PointPair.Missing )
				return this.baseTic;
			else
			{
				int year, month, day, hour, minute, second;
				XDate.XLDateToCalendarDate( this.min, out year, out month, out day, out hour, out minute,
											out second );
				switch ( this.majorUnit )
				{
					case DateUnit.Year:
					default:
						month = 1; day = 1; hour = 0; minute = 0; second = 0;
						break;
					case DateUnit.Month:
						day = 1; hour = 0; minute = 0; second = 0;
						break;
					case DateUnit.Day:
						hour = 0; minute = 0; second = 0;
						break;
					case DateUnit.Hour:
						minute = 0; second = 0;
						break;
					case DateUnit.Minute:
						second = 0;
						break;
					case DateUnit.Second:
						break;

				}

				double xlDate = XDate.CalendarDateToXLDate( year, month, day, hour, minute, second );
				if ( xlDate < this.min )
				{
					switch ( this.majorUnit )
					{
						case DateUnit.Year:
						default:
							year++;
							break;
						case DateUnit.Month:
							month++;
							break;
						case DateUnit.Day:
							day++;
							break;
						case DateUnit.Hour:
							hour++;
							break;
						case DateUnit.Minute:
							minute++;
							break;
						case DateUnit.Second:
							second++;
							break;

					}

					xlDate = XDate.CalendarDateToXLDate( year, month, day, hour, minute, second );
				}

				return xlDate;
			}
		}
		
		/// <summary>
		/// Internal routine to determine the ordinals of the first and last major axis label.
		/// </summary>
		/// <returns>
		/// This is the total number of major tics for this axis.
		/// </returns>
		override internal int CalcNumTics()
		{
			int nTics = 1;

			int year1, year2, month1, month2, day1, day2, hour1, hour2, minute1, minute2;
			double second1, second2;

			XDate.XLDateToCalendarDate( this.min, out year1, out month1, out day1,
										out hour1, out minute1, out second1 );
			XDate.XLDateToCalendarDate( this.max, out year2, out month2, out day2,
										out hour2, out minute2, out second2 );

			switch ( this.majorUnit )
			{
				case DateUnit.Year:
				default:
					nTics = (int) ( ( year2 - year1 ) / this.step + 1.001 );
					break;
				case DateUnit.Month:
					nTics = (int) ( ( month2 - month1 + 12.0 * ( year2 - year1 ) ) / this.step + 1.001 );
					break;
				case DateUnit.Day:
					nTics = (int) ( ( this.max - this.min ) / this.step + 1.001 );
					break;
				case DateUnit.Hour:
					nTics = (int) ( ( this.max - this.min ) / ( this.step / XDate.HoursPerDay ) + 1.001 );
					break;
				case DateUnit.Minute:
					nTics = (int) ( ( this.max - this.min ) / ( this.step / XDate.MinutesPerDay ) + 1.001 );
					break;
				case DateUnit.Second:
					nTics = (int) ( ( this.max - this.min ) / ( this.step / XDate.SecondsPerDay ) + 1.001 );
					break;
			}

			if ( nTics < 1 )
				nTics = 1;
			else if ( nTics > 500 )
				nTics = 500;

			return nTics;
		}

		/// <summary>
		/// Select a reasonable date-time axis scale given a range of data values.
		/// </summary>
		/// <remarks>
		/// This method only applies to <see cref="AxisType.Date"/> type axes, and it
		/// is called by the general <see cref="PickScale"/> method.  The scale range is chosen
		/// based on increments of 1, 2, or 5 (because they are even divisors of 10).
		/// Note that the <see cref="Scale.Step"/> property setting can have multiple unit
		/// types (<see cref="Scale.MajorUnit"/> and <see cref="Scale.MinorUnit" />),
		/// but the <see cref="Scale.Min"/> and
		/// <see cref="Scale.Max"/> units are always days (<see cref="XDate"/>).  This
		/// method honors the <see cref="Scale.MinAuto"/>, <see cref="Scale.MaxAuto"/>,
		/// and <see cref="Scale.StepAuto"/> autorange settings.
		/// In the event that any of the autorange settings are false, the
		/// corresponding <see cref="Scale.Min"/>, <see cref="Scale.Max"/>, or <see cref="Scale.Step"/>
		/// setting is explicitly honored, and the remaining autorange settings (if any) will
		/// be calculated to accomodate the non-autoranged values.  The basic default for
		/// scale selection is defined with
		/// <see cref="Scale.Default.TargetXSteps"/> and <see cref="Scale.Default.TargetYSteps"/>
		/// from the <see cref="Scale.Default"/> default class.
		/// <para>On Exit:</para>
		/// <para><see cref="Scale.Min"/> is set to scale minimum (if <see cref="Scale.MinAuto"/> = true)</para>
		/// <para><see cref="Scale.Max"/> is set to scale maximum (if <see cref="Scale.MaxAuto"/> = true)</para>
		/// <para><see cref="Scale.Step"/> is set to scale step size (if <see cref="Scale.StepAuto"/> = true)</para>
		/// <para><see cref="Scale.MinorStep"/> is set to scale minor step size (if <see cref="Scale.MinorStepAuto"/> = true)</para>
		/// <para><see cref="Scale.ScaleMag"/> is set to a magnitude multiplier according to the data</para>
		/// <para><see cref="Scale.ScaleFormat"/> is set to the display format for the values (this controls the
		/// number of decimal places, whether there are thousands separators, currency types, etc.)</para>
		/// </remarks>
		/// <param name="pane">A reference to the <see cref="GraphPane"/> object
		/// associated with this <see cref="Axis"/></param>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		/// <seealso cref="Scale.PickScale"/>
		/// <seealso cref="AxisType.Date"/>
		/// <seealso cref="Scale.MajorUnit"/>
		/// <seealso cref="Scale.MinorUnit"/>
		override public void PickScale( GraphPane pane, Graphics g, float scaleFactor )
		{
			// call the base class first
			base.PickScale( pane, g, scaleFactor );

			// Test for trivial condition of range = 0 and pick a suitable default
			if ( this.max - this.min < 1.0e-20 )
			{
				if ( this.maxAuto )
					this.max = this.max + 0.2 * ( this.max == 0 ? 1.0 : Math.Abs( this.max ) );
				if ( this.minAuto )
					this.min = this.min - 0.2 * ( this.min == 0 ? 1.0 : Math.Abs( this.min ) );
			}

			// Calculate the new step size
			if ( this.stepAuto )
			{
				double targetSteps = ( parentAxis is XAxis ) ? Default.TargetXSteps : Default.TargetYSteps;

				// Calculate the step size based on target steps
				this.step = CalcDateStepSize( this.max - this.min, targetSteps );

				if ( this.isPreventLabelOverlap )
				{
					// Calculate the maximum number of labels
					double maxLabels = (double) this.CalcMaxLabels( g, pane, scaleFactor );

					if ( maxLabels < this.CalcNumTics() )
						this.step = CalcDateStepSize( this.max - this.min, maxLabels );
				}
			}

			// Calculate the scale minimum
			if ( this.minAuto )
				this.min = CalcEvenStepDate( this.min, -1 );

			// Calculate the scale maximum
			if ( this.maxAuto )
				this.max = CalcEvenStepDate( this.max, 1 );

			this.scaleMag = 0;		// Never use a magnitude shift for date scales
			//this.numDec = 0;		// The number of decimal places to display is not used

		}

		/// <summary>
		/// Calculate a step size for a <see cref="AxisType.Date"/> scale.
		/// This method is used by <see cref="PickScale"/>.
		/// </summary>
		/// <param name="range">The range of data in units of days</param>
		/// <param name="targetSteps">The desired "typical" number of steps
		/// to divide the range into</param>
		/// <returns>The calculated step size for the specified data range.  Also
		/// calculates and sets the values for <see cref="Scale.MajorUnit"/>,
		/// <see cref="Scale.MinorUnit"/>, <see cref="Scale.MinorStep"/>, and
		/// <see cref="Scale.ScaleFormat"/></returns>
		protected double CalcDateStepSize( double range, double targetSteps )
		{
			return CalcDateStepSize( range, targetSteps, this );
		}

		/// <summary>
		/// Calculate a step size for a <see cref="AxisType.Date"/> scale.
		/// This method is used by <see cref="PickScale"/>.
		/// </summary>
		/// <param name="range">The range of data in units of days</param>
		/// <param name="targetSteps">The desired "typical" number of steps
		/// to divide the range into</param>
		/// <param name="scale">
		/// The <see cref="Scale" /> object on which to calculate the Date step size.</param>
		/// <returns>The calculated step size for the specified data range.  Also
		/// calculates and sets the values for <see cref="Scale.MajorUnit"/>,
		/// <see cref="Scale.MinorUnit"/>, <see cref="Scale.MinorStep"/>, and
		/// <see cref="Scale.ScaleFormat"/></returns>
		internal static double CalcDateStepSize( double range, double targetSteps, Scale scale )
		{
			// Calculate an initial guess at step size
			double tempStep = range / targetSteps;

			if ( range > Default.RangeYearYear )
			{
				scale.MajorUnit = DateUnit.Year;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatYearYear;

				tempStep = Math.Ceiling( tempStep / 365.0 );

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Year;
					if ( tempStep == 1.0 )
						scale.MinorStep = 0.25;
					else
						scale.MinorStep = Scale.CalcStepSize( tempStep, targetSteps );
				}
			}
			else if ( range > Default.RangeYearMonth )
			{
				scale.MajorUnit = DateUnit.Year;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatYearMonth;
				tempStep = Math.Ceiling( tempStep / 365.0 );

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Month;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale.MinorStep = Math.Ceiling( range / ( targetSteps * 3 ) / 30.0 );
					// make sure the minorStep is 1, 2, 3, 6, or 12 months
					if ( scale.MinorStep > 6 )
						scale.MinorStep = 12;
					else if ( scale.MinorStep > 3 )
						scale.MinorStep = 6;
				}
			}
			else if ( range > Default.RangeMonthMonth )
			{
				scale.MajorUnit = DateUnit.Month;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatMonthMonth;
				tempStep = Math.Ceiling( tempStep / 30.0 );

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Month;
					scale.MinorStep = tempStep * 0.25;
				}
			}
			else if ( range > Default.RangeDayDay )
			{
				scale.MajorUnit = DateUnit.Day;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatDayDay;
				tempStep = Math.Ceiling( tempStep );

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Day;
					scale.MinorStep = tempStep * 0.25;
					// make sure the minorStep is 1, 2, 3, 6, or 12 hours
				}
			}
			else if ( range > Default.RangeDayHour )
			{
				scale.MajorUnit = DateUnit.Day;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatDayHour;
				tempStep = Math.Ceiling( tempStep );

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Hour;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale.MinorStep = Math.Ceiling( range / ( targetSteps * 3 ) * XDate.HoursPerDay );
					// make sure the minorStep is 1, 2, 3, 6, or 12 hours
					if ( scale.MinorStep > 6 )
						scale.MinorStep = 12;
					else if ( scale.MinorStep > 3 )
						scale.MinorStep = 6;
					else
						scale.MinorStep = 1;
				}
			}
			else if ( range > Default.RangeHourHour )
			{
				scale.MajorUnit = DateUnit.Hour;
				tempStep = Math.Ceiling( tempStep * XDate.HoursPerDay );
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatHourHour;

				if ( tempStep > 12.0 )
					tempStep = 24.0;
				else if ( tempStep > 6.0 )
					tempStep = 12.0;
				else if ( tempStep > 2.0 )
					tempStep = 6.0;
				else if ( tempStep > 1.0 )
					tempStep = 2.0;
				else
					tempStep = 1.0;

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Hour;
					if ( tempStep <= 1.0 )
						scale.MinorStep = 0.25;
					else if ( tempStep <= 6.0 )
						scale.MinorStep = 1.0;
					else if ( tempStep <= 12.0 )
						scale.MinorStep = 2.0;
					else
						scale.MinorStep = 4.0;
				}
			}
			else if ( range > Default.RangeHourMinute )
			{
				scale.MajorUnit = DateUnit.Hour;
				tempStep = Math.Ceiling( tempStep * XDate.HoursPerDay );

				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatHourMinute;

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Minute;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale.MinorStep = Math.Ceiling( range / ( targetSteps * 3 ) * XDate.MinutesPerDay );
					// make sure the minorStep is 1, 5, 15, or 30 minutes
					if ( scale.MinorStep > 15.0 )
						scale.MinorStep = 30.0;
					else if ( scale.MinorStep > 5.0 )
						scale.MinorStep = 15.0;
					else if ( scale.MinorStep > 1.0 )
						scale.MinorStep = 5.0;
					else
						scale.MinorStep = 1.0;
				}
			}
			else if ( range > Default.RangeMinuteMinute )
			{
				scale.MajorUnit = DateUnit.Minute;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatMinuteMinute;

				tempStep = Math.Ceiling( tempStep * XDate.MinutesPerDay );
				// make sure the minute step size is 1, 5, 15, or 30 minutes
				if ( tempStep > 15.0 )
					tempStep = 30.0;
				else if ( tempStep > 5.0 )
					tempStep = 15.0;
				else if ( tempStep > 1.0 )
					tempStep = 5.0;
				else
					tempStep = 1.0;

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Minute;
					if ( tempStep <= 1.0 )
						scale.MinorStep = 0.25;
					else if ( tempStep <= 5.0 )
						scale.MinorStep = 1.0;
					else
						scale.MinorStep = 5.0;
				}
			}
			else if ( range > Default.RangeMinuteSecond )
			{
				scale.MajorUnit = DateUnit.Minute;
				tempStep = Math.Ceiling( tempStep * XDate.MinutesPerDay );

				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatMinuteSecond;

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Second;
					// Calculate the minor steps to give an estimated 4 steps
					// per major step.
					scale.MinorStep = Math.Ceiling( range / ( targetSteps * 3 ) * XDate.SecondsPerDay );
					// make sure the minorStep is 1, 5, 15, or 30 seconds
					if ( scale.MinorStep > 15.0 )
						scale.MinorStep = 30.0;
					else if ( scale.MinorStep > 5.0 )
						scale.MinorStep = 15.0;
					else if ( scale.MinorStep > 1.0 )
						scale.MinorStep = 5.0;
					else
						scale.MinorStep = 1.0;
				}
			}
			else // SecondSecond
			{
				scale.MajorUnit = DateUnit.Second;
				if ( scale.ScaleFormatAuto )
					scale.ScaleFormat = Default.FormatSecondSecond;

				tempStep = Math.Ceiling( tempStep * XDate.SecondsPerDay );
				// make sure the second step size is 1, 5, 15, or 30 seconds
				if ( tempStep > 15.0 )
					tempStep = 30.0;
				else if ( tempStep > 5.0 )
					tempStep = 15.0;
				else if ( tempStep > 1.0 )
					tempStep = 5.0;
				else
					tempStep = 1.0;

				if ( scale.MinorStepAuto )
				{
					scale.MinorUnit = DateUnit.Second;
					if ( tempStep <= 1.0 )
						scale.MinorStep = 0.25;
					else if ( tempStep <= 5.0 )
						scale.MinorStep = 1.0;
					else
						scale.MinorStep = 5.0;
				}
			}

			return tempStep;
		}

		/// <summary>
		/// Calculate a date that is close to the specified date and an
		/// even multiple of the selected
		/// <see cref="Scale.MajorUnit"/> for a <see cref="AxisType.Date"/> scale.
		/// This method is used by <see cref="PickScale"/>.
		/// </summary>
		/// <param name="date">The date which the calculation should be close to</param>
		/// <param name="direction">The desired direction for the date to take.
		/// 1 indicates the result date should be greater than the specified
		/// date parameter.  -1 indicates the other direction.</param>
		/// <returns>The calculated date</returns>
		protected double CalcEvenStepDate( double date, int direction )
		{
			int year, month, day, hour, minute, second;

			XDate.XLDateToCalendarDate( date, out year, out month, out day,
										out hour, out minute, out second );

			// If the direction is -1, then it is sufficient to go to the beginning of
			// the current time period, .e.g., for 15-May-95, and monthly steps, we
			// can just back up to 1-May-95
			if ( direction < 0 )
				direction = 0;

			switch ( majorUnit )
			{
				case DateUnit.Year:
				default:
					// If the date is already an exact year, then don't step to the next year
					if ( direction == 1 && month == 1 && day == 1 && hour == 0
						&& minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year + direction, 1, 1,
														0, 0, 0 );
				case DateUnit.Month:
					// If the date is already an exact month, then don't step to the next month
					if ( direction == 1 && day == 1 && hour == 0
						&& minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month + direction, 1,
												0, 0, 0 );
				case DateUnit.Day:
					// If the date is already an exact Day, then don't step to the next day
					if ( direction == 1 && hour == 0 && minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month,
											day + direction, 0, 0, 0 );
				case DateUnit.Hour:
					// If the date is already an exact hour, then don't step to the next hour
					if ( direction == 1 && minute == 0 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month, day,
													hour + direction, 0, 0 );
				case DateUnit.Minute:
					// If the date is already an exact minute, then don't step to the next minute
					if ( direction == 1 && second == 0 )
						return date;
					else
						return XDate.CalendarDateToXLDate( year, month, day, hour,
													minute + direction, 0 );
				case DateUnit.Second:
					return XDate.CalendarDateToXLDate( year, month, day, hour,
													minute, second + direction );

			}
		}

		/// <summary>
		/// Make a value label for an <see cref="AxisType.Date" /> <see cref="Axis" />.
		/// </summary>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="index">
		/// The zero-based, ordinal index of the label to be generated.  For example, a value of 2 would
		/// cause the third value label on the axis to be generated.
		/// </param>
		/// <param name="dVal">
		/// The numeric value associated with the label.  This value is ignored for log (<see cref="Axis.IsLog"/>)
		/// and text (<see cref="Axis.IsText"/>) type axes.
		/// </param>
		/// <param name="label">
		/// Output only.  The resulting value label.
		/// </param>
		override internal void MakeLabel( GraphPane pane, int index, double dVal, out string label )
		{
			if ( this.scaleFormat == null )
				this.scaleFormat = Scale.Default.ScaleFormat;

			label = XDate.ToString( dVal, this.scaleFormat );
		}

	#endregion

	}
}