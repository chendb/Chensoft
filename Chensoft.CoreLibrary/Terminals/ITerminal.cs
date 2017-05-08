using System.IO;

namespace Chensoft.Terminals
{
    public interface ITerminal
	{
		#region 属性定义
		TerminalColor BackgroundColor
		{
			get;
			set;
		}

		TerminalColor ForegroundColor
		{
			get;
			set;
		}

		TextReader Input
		{
			get;
			set;
		}

		Stream InputStream
		{
			get;
		}

		TextWriter Output
		{
			get;
			set;
		}

		Stream OutputStream
		{
			get;
		}

		TextWriter Error
		{
			get;
			set;
		}

		Stream ErrorStream
		{
			get;
		}
		#endregion

		#region 方法定义
		void Clear();
		void Reset();
		void ResetStyles(TerminalStyles styles);

		void Write(string text);
		void Write(object value);
		void Write(string format, params object[] args);
		void Write(TerminalColor foregroundColor, string text);
		void Write(TerminalColor foregroundColor, object value);
		void Write(TerminalColor foregroundColor, string format, params object[] args);

		void WriteLine();
		void WriteLine(string text);
		void WriteLine(object value);
		void WriteLine(string format, params object[] args);
		void WriteLine(TerminalColor foregroundColor, string text);
		void WriteLine(TerminalColor foregroundColor, object value);
		void WriteLine(TerminalColor foregroundColor, string format, params object[] args);
		#endregion
	}
}
