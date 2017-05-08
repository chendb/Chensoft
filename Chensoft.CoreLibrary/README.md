此项目是基于Mef 插件框架，适用于Terminal,Web,Wpf等项目。
使用示例如下：
1、程序启动
class Program
{
    static void Main(string[] args)
    {
        Application.Default.Start();
        Console.Read();
    }
}
2、添加项目，并在项目中添加IModule接口实现，并使用Mef导出
    [Export(typeof(IModule))]
    public class TestModule : IModule
    {


        [Import(typeof(ITerminal))]
        public ITerminal Terminal { get; set; }


        public void Initizate(object content)
        {
              Terminal.WriteLine("模块启动");
        }
 
 
    }
}

注意：实现IModule的类，程序在启动时会顺序执行Initizate的操作，启动完成时也将执行完。因此Initizate方法一般是执行该模块启动的先决条件代码。
3、在项目中实现IWorker接口，示例如下：
    [Export(typeof(IWorker))]
    public class TestWorker : WorkerBase
    {
 
        #region 公共属性

        [Import(typeof(ITerminal))]
        public ITerminal Terminal { get; set; }

        #endregion

        #region 接口实现

        protected override void OnStart(string[] args)
        {
            
            this.Terminal.WriteLine(TerminalColor.Blue, "工作线程启动成功");

        }

        protected override void OnStop()
        {
			this.Terminal.WriteLine(TerminalColor.Blue, "工作线程停止");
        }

        #endregion

    }
}

4、添加插件文件，如下：
{
  "Name": "*** Plugin",
  "Manifest": {
    "Assemblies": [
      "*******.dll"
    ],
    "Author": "***",
    "Copyright": "**公司",
    "Title": "**插件",
    "Version": "*.*.*.*"
  },
  "Builtins": [
  ]
}

