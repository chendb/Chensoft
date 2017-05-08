
using System;

namespace Chensoft.Terminals
{
    [Flags]
	public enum TerminalStyles
	{
		BackgroundColor = 1,
		ForegroundColor = 2,
		Color = 3,
		FontName = 4,
		FontStyle = 8,
		FontSize = 16,
		Font = 28,
	}
}
