﻿using Hoff.Core.Hardware.Common.Structs;

namespace Hoff.Core.Hardware.Common.Interfaces.Displays

{
    public interface IDisplay
    {
        #region Public Methods

        /// <summary>
        /// Clears a directly drawn line
        /// </summary>
        /// <param name="line"></param>
        void ClearDirectLine(DirectLine line);

        /// <summary>
        /// Clears the display.
        /// </summary>
        void ClearScreen();

        /// <summary>
        /// writes a directly drawn line.
        /// </summary>
        /// <param name="line">The line to draw.</param>
        void DrawDirectLine(DirectLine line);

        /// <summary>
        /// Draws a  Horizontal line of x length
        /// </summary>
        /// <param name="line">The line to draw.</param>
        /// <param name="draw">Bool to write (true) or clear (false)</param>
        void HorizontalLine(Line line, bool draw = true);

        void UpdateDisplay();

        /// <summary>
        /// Draws a Vertical line of x length
        /// </summary>
        /// <param name="line">The line to draw.</param>
        /// <param name="draw">Bool to write (true) or clear (false)</param>
        void VerticalLine(Line line, bool draw = true);

        /// <summary>
        /// Writes a string to the display.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="center"></param>
        /// <param name="font"></param>
        void WriteLine(int x, int y, string text, byte fontSize = 1, bool center = false, object font = null);

        #endregion Public Methods
    }
}
