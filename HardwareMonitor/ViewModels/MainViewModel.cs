using HardwareMonitor.Models;
using HardwareMonitor.Services;
using HardwareMonitor.Utils;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;


namespace HardwareMonitor.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly CpuMonitor _cpu = new CpuMonitor();
        private readonly MemoryMonitor _memory = new MemoryMonitor();
        private readonly DiskMonitor _disk = new DiskMonitor();
        private readonly ExportService _export = new ExportService();
        private readonly GpuMonitor _gpu = new GpuMonitor();
        private readonly NetworkMonitor _network = new NetworkMonitor();
        private readonly SystemMonitor _system = new SystemMonitor();

        private readonly ExportService _exportService = new ExportService();

        private GpuInfo _gpuInfo = new GpuInfo();
        public GpuInfo GpuInfo
        {
            get => _gpuInfo;
            set => SetProperty(ref _gpuInfo, value);
        }

        public List<NetworkAdapterInfo> NetworkAdapters { get; set; } = new List<NetworkAdapterInfo>();

        private SystemInfo _systemInfo = new SystemInfo();
        public SystemInfo SystemInfo
        {
            get => _systemInfo;
            set => SetProperty(ref _systemInfo, value);
        }


        private CpuInfo _cpuInfo = new CpuInfo();
        public CpuInfo CpuInfo
        {
            get => _cpuInfo;
            set => SetProperty(ref _cpuInfo, value);
        }

        private MemoryInfo _memoryInfo = new MemoryInfo();
        public MemoryInfo MemoryInfo
        {
            get => _memoryInfo;
            set => SetProperty(ref _memoryInfo, value);
        }

        private DiskInfo _diskInfo = new DiskInfo();
        public DiskInfo DiskInfo
        {
            get => _diskInfo;
            set => SetProperty(ref _diskInfo, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand HelpCommand { get; }

        private DispatcherTimer _timer;

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await RefreshAsync());
            ExportCommand = new RelayCommand(Export);
            HelpCommand = new RelayCommand(OpenHelp);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += async (s, e) => await RefreshAsync();
            _timer.Start();
        }

        private async Task RefreshAsync()
        {
            CpuInfo = await Task.Run(() => _cpu.GetCpuInfo());
            MemoryInfo = await Task.Run(() => _memory.GetMemoryInfo());
            DiskInfo = await Task.Run(() => _disk.GetDiskInfo());
            GpuInfo = await Task.Run(() => _gpu.GetGpuInfo());
            NetworkAdapters = await Task.Run(() => _network.GetAdapters());
            SystemInfo = await Task.Run(() => _system.GetSystemInfo());

            OnPropertyChanged(nameof(NetworkAdapters));
        }


        private void Export()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Экспорт данных",
                Filter = "Text file (*.txt)|*.txt|JSON file (*.json)|*.json|CSV file (*.csv)|*.csv",
                FileName = "hardware_info"
            };

            bool? result = dialog.ShowDialog();

            if (result != true)
                return; // пользователь нажал Отмена

            try
            {
                if (dialog.FileName.EndsWith(".json"))
                    _exportService.ExportToJson(dialog.FileName, this);
                else if (dialog.FileName.EndsWith(".csv"))
                    _exportService.ExportToCsv(dialog.FileName, this);
                else
                    _exportService.ExportToTxt(dialog.FileName, this);

                System.Windows.MessageBox.Show("Экспорт выполнен успешно!",
                    "Экспорт", System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка экспорта:\n{ex.Message}");
            }
        }

        private void OpenHelp()
        {
            var helpWindow = new HelpWindow();
            helpWindow.Owner = Application.Current.MainWindow;
            helpWindow.ShowDialog();
        }
    }
}
