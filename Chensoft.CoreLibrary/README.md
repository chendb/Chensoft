����Ŀ�ǻ���Mef �����ܣ�������Terminal,Web,Wpf����Ŀ��
ʹ��ʾ�����£�
1����������
class Program
{
    static void Main(string[] args)
    {
        Application.Default.Start();
        Console.Read();
    }
}
2�������Ŀ��������Ŀ�����IModule�ӿ�ʵ�֣���ʹ��Mef����
    [Export(typeof(IModule))]
    public class TestModule : IModule
    {


        [Import(typeof(ITerminal))]
        public ITerminal Terminal { get; set; }


        public void Initizate(object content)
        {
              Terminal.WriteLine("ģ������");
        }
 
 
    }
}

ע�⣺ʵ��IModule���࣬����������ʱ��˳��ִ��Initizate�Ĳ������������ʱҲ��ִ���ꡣ���Initizate����һ����ִ�и�ģ���������Ⱦ��������롣
3������Ŀ��ʵ��IWorker�ӿڣ�ʾ�����£�
    [Export(typeof(IWorker))]
    public class TestWorker : WorkerBase
    {
 
        #region ��������

        [Import(typeof(ITerminal))]
        public ITerminal Terminal { get; set; }

        #endregion

        #region �ӿ�ʵ��

        protected override void OnStart(string[] args)
        {
            
            this.Terminal.WriteLine(TerminalColor.Blue, "�����߳������ɹ�");

        }

        protected override void OnStop()
        {
			this.Terminal.WriteLine(TerminalColor.Blue, "�����߳�ֹͣ");
        }

        #endregion

    }
}

4����Ӳ���ļ������£�
{
  "Name": "*** Plugin",
  "Manifest": {
    "Assemblies": [
      "*******.dll"
    ],
    "Author": "***",
    "Copyright": "**��˾",
    "Title": "**���",
    "Version": "*.*.*.*"
  },
  "Builtins": [
  ]
}

