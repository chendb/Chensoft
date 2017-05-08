
using System;
using System.ComponentModel.Composition;
using System.IO;

namespace Chensoft.Terminals
{
    public class ConsoleTerminal : MarshalByRefObject, ITerminal
    {
        #region 同步变量
        private readonly object _syncRoot;
        #endregion

        #region 构造函数
        public ConsoleTerminal()
        {
            _syncRoot = new object();

        }


        #endregion

        #region 公共属性


        public TerminalColor BackgroundColor
        {
            get
            {
                return this.ConvertColor(Console.BackgroundColor, TerminalColor.Black);
            }
            set
            {
                Console.BackgroundColor = ConvertColor(value, ConsoleColor.Black);
            }
        }

        public TerminalColor ForegroundColor
        {
            get
            {
                return this.ConvertColor(Console.ForegroundColor, TerminalColor.White);
            }
            set
            {
                Console.ForegroundColor = ConvertColor(value, ConsoleColor.White);
            }
        }
        #endregion

        #region 公共方法
        public void Clear()
        {
            Console.Clear();
        }

        public void Reset()
        {
            //恢复默认的颜色设置
            Console.ResetColor();

            try
            {
                if (Console.CursorLeft > 0)
                    Console.WriteLine();
            }
            catch
            {
            }


        }

        public void ResetStyles(TerminalStyles styles)
        {
            if ((styles & TerminalStyles.Color) == TerminalStyles.Color)
                Console.ResetColor();
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void Write(object value)
        {
            Console.Write(value);
        }

        public void Write(TerminalColor foregroundColor, string text)
        {
            lock (_syncRoot)
            {
                var originalColor = this.ForegroundColor;
                this.ForegroundColor = foregroundColor;

                Console.Write(text);

                this.ForegroundColor = originalColor;
            }
        }

        public void Write(TerminalColor foregroundColor, object value)
        {
            lock (_syncRoot)
            {
                var originalColor = this.ForegroundColor;
                this.ForegroundColor = foregroundColor;

                Console.Write(value);

                this.ForegroundColor = originalColor;
            }
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void Write(TerminalColor foregroundColor, string format, params object[] args)
        {
            lock (_syncRoot)
            {
                var originalColor = this.ForegroundColor;
                this.ForegroundColor = foregroundColor;

                Console.Write(format, args);

                this.ForegroundColor = originalColor;
            }
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteLine(object value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine(TerminalColor foregroundColor, string text)
        {
            lock (_syncRoot)
            {
                var originalColor = this.ForegroundColor;
                this.ForegroundColor = foregroundColor;

                Console.WriteLine(text);

                this.ForegroundColor = originalColor;
            }
        }

        public void WriteLine(TerminalColor foregroundColor, object value)
        {
            lock (_syncRoot)
            {
                var originalColor = this.ForegroundColor;
                this.ForegroundColor = foregroundColor;

                Console.WriteLine(value);

                this.ForegroundColor = originalColor;
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void WriteLine(TerminalColor foregroundColor, string format, params object[] args)
        {
            lock (_syncRoot)
            {
                var originalColor = this.ForegroundColor;
                this.ForegroundColor = foregroundColor;

                Console.WriteLine(format, args);

                this.ForegroundColor = originalColor;
            }
        }
        #endregion

        #region 显示实现
        TextReader ITerminal.Input
        {
            get
            {
                return Console.In;
            }
            set
            {
                Console.SetIn(value);
            }
        }

        Stream ITerminal.InputStream
        {
            get
            {
                return Console.OpenStandardInput();
            }
        }

        TextWriter ITerminal.Output
        {
            get
            {
                return Console.Out;
            }
            set
            {
                Console.SetOut(value);
            }
        }

        Stream ITerminal.OutputStream
        {
            get
            {
                return Console.OpenStandardOutput();
            }
        }

        TextWriter ITerminal.Error
        {
            get
            {
                return Console.Error;
            }
            set
            {
                Console.SetError(value);
            }
        }

        Stream ITerminal.ErrorStream
        {
            get
            {
                return Console.OpenStandardError();
            }
        }
        #endregion

        #region 私有方法
        private TerminalColor ConvertColor(ConsoleColor color, TerminalColor defaultColor)
        {
            TerminalColor result;

            if (Enum.TryParse<TerminalColor>(color.ToString(), out result))
                return result;
            else
                return defaultColor;
        }

        private ConsoleColor ConvertColor(TerminalColor color, ConsoleColor defaultColor)
        {
            ConsoleColor result;

            if (Enum.TryParse<ConsoleColor>(color.ToString(), out result))
                return result;
            else
                return defaultColor;
        }
        #endregion
    }
}
