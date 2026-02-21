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
        private readonly GpuMonitor _gpu = new GpuMonitor();
        private readonly NetworkMonitor _network = new NetworkMonitor();
        private readonly SystemMonitor _system = new SystemMonitor();
        private readonly ExportService _exportService = new ExportService();

        private bool _isRefreshing;

        #region Properties

        private GpuInfo _gpuInfo = new GpuInfo();
        public GpuInfo GpuInfo
        {
            get => _gpuInfo;
            set => SetProperty(ref _gpuInfo, value);
        }

        private List<NetworkAdapterInfo> _networkAdapters = new List<NetworkAdapterInfo>();
        public List<NetworkAdapterInfo> NetworkAdapters
        {
            get => _networkAdapters;
            set => SetProperty(ref _networkAdapters, value);
        }

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

        #endregion

        #region Commands

        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand HelpCommand { get; }

        #endregion

        private readonly DispatcherTimer _timer;

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await RefreshAsync());
            ExportCommand = new RelayCommand(Export);
            HelpCommand = new RelayCommand(OpenHelp);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _timer.Tick += async (s, e) => await RefreshAsync();
            _timer.Start();

            /*try
            {
                HardwareMonitor.Utils.PerformanceHelper.RunAllTests();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка бенчмарка: {ex.Message}");
            }*/
        }

        private async Task RefreshAsync()
        {
            if (_isRefreshing) return;
            _isRefreshing = true;

            try
            {
                var cpuTask = Task.Run(() => _cpu.GetCpuInfo());
                var memoryTask = Task.Run(() => _memory.GetMemoryInfo());
                var diskTask = Task.Run(() => _disk.GetDiskInfo());
                var gpuTask = Task.Run(() => _gpu.GetGpuInfo());
                var networkTask = Task.Run(() => _network.GetAdapters());
                var systemTask = Task.Run(() => _system.GetSystemInfo());

                CpuInfo = await cpuTask;
                MemoryInfo = await memoryTask;
                DiskInfo = await diskTask;
                GpuInfo = await gpuTask;
                NetworkAdapters = await networkTask;
                SystemInfo = await systemTask;
            }
            finally
            {
                _isRefreshing = false;
            }
        }


        private void Export()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Экспорт данных",
                Filter = "Text file (*.txt)|*.txt|JSON file (*.json)|*.json|CSV file (*.csv)|*.csv",
                FileName = "hardware_info"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                if (dialog.FileName.EndsWith(".json"))
                    _exportService.ExportToJson(dialog.FileName, this);
                else if (dialog.FileName.EndsWith(".csv"))
                    _exportService.ExportToCsv(dialog.FileName, this);
                else
                    _exportService.ExportToTxt(dialog.FileName, this);

                MessageBox.Show("Экспорт выполнен успешно!",
                    "Экспорт", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта:\n{ex.Message}");
            }
        }

        private void OpenHelp()
        {
            var helpWindow = new HelpWindow
            {
                Owner = Application.Current.MainWindow
            };
            helpWindow.ShowDialog();
        }

    }
}