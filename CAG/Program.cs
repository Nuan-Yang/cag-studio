using CAG.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CAG
{
    public static class Program
    {
        /// <summary>
        /// �����ļ�����
        /// </summary>
        public static readonly string ConfigFileName = "CAGStudio.json";

        /// <summary>
        /// �������ļ��ı��Ƿ��������
        /// </summary>
        public static bool ConfigReloadOnChange = true;

        /// <summary>
        /// ����
        /// </summary>
        public static IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// ����Ӧ��
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitConfiguration();
            InitServiceProvider();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetService<MainForm>());
        }

        /// <summary>
        /// ��ʼ������Ӧ��
        /// </summary>
        static void InitServiceProvider()
        {
            var Service = new ServiceCollection();
            var forms = typeof(Program).Assembly.GetTypes().Where(c => c.IsAssignableTo(typeof(Form)));
            foreach (var item in forms)
            {
                Service.AddScoped(item);
            }
            IEnumerable<Type> types = typeof(Program).Assembly.GetTypes().Where(c => c.CustomAttributes.Any(d => d.AttributeType == typeof(Dotnet2JSAttribute)));
            foreach (Type item in types)
            {
                Service.AddSingleton(item);
            }
            Service.AddSingleton<IConfiguration>(Configuration);
            Service.AddLogging(c =>
            {
                c.AddConfiguration(Configuration.GetSection("Logging"));
                c.AddFile();
                c.AddDebug();
            });
            ServiceProvider = Service.BuildServiceProvider();
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        static void InitConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigFileName, optional: true, reloadOnChange: ConfigReloadOnChange)
                .Build();

        }
    }
}