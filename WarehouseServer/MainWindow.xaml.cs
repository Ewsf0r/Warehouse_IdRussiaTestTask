using System.ComponentModel;
using System.Windows;

namespace WarehouseServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebApplication? _app;
        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }

        void StartServer()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            
            _app = builder.Build();
            if (_app.Environment.IsDevelopment())
                _app.MapOpenApi();
            _app.UseHttpsRedirection();
            _app.UseAuthorization();
            _app.MapControllers();
            _app.RunAsync();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            _app?.StopAsync().Wait(100);
            base.OnClosing(e);
        }
    }
}