using CAG.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CAG
{
    public static class Program
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public static readonly string ConfigFileName = "CAGStudio.json";

        /// <summary>
        /// 当配置文件改变是否进行重载
        /// </summary>
        public static bool ConfigReloadOnChange = true;

        /// <summary>
        /// 配置
        /// </summary>
        public static IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// 服务供应商
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
        /// 初始化服务供应商
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
        /// 初始化配置
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