using HardwareMonitor.Models;
using HardwareMonitor.Services;
using HardwareMonitor.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using HardwareMonitor.Utils;


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

        private DispatcherTimer _timer;

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await RefreshAsync());
            ExportCommand = new RelayCommand(Export);

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
            var dialog = new SaveFileDialog();
            dialog.Filter = "TXT|*.txt|CSV|*.csv|JSON|*.json";

            if (dialog.ShowDialog() != true) return;

            var data = new { CpuInfo, MemoryInfo, DiskInfo, Date = DateTime.Now };

            if (dialog.FileName.EndsWith(".txt"))
                _export.ExportToTxt(dialog.FileName, data);
            else if (dialog.FileName.EndsWith(".csv"))
            {
                _exportService.ExportToTxt("report.txt", data);
                _exportService.ExportToJson("report.json", data);
            }
            else
                _export.ExportToJson(dialog.FileName, data);
        }
    }
}
