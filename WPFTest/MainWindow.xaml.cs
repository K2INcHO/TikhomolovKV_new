using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPFTest
{
    public partial class MainWindow
    {
        public MainWindow() => InitializeComponent();

        private async void OnOpenFileClick(object sender, RoutedEventArgs e)
        {
            //Мы находились в ThreadID == 1
            await Task.Yield(); //Даем время на обработку сообщений пользовательского интерфейса (разрываем метод и даем возможность вклиниться чему-то, использовавшему этот поток)
            //Мы снова в ThreadID == 1

            var dialog = new OpenFileDialog
            {
                Title = "Выбор файла для чтения",
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != true) return;

            //var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            //using(var reader = new StreamReader(dialog.FileName))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        var line = await reader.ReadLineAsync().ConfigureAwait(false);
            //        var words = line.Split(' ');
            //        //Thread.Sleep(100);
            //        //await Task.Delay(1);

            //        foreach (var word in words)
            //            if (dict.ContainsKey(word))
            //                dict[word]++;
            //            else
            //                dict.Add(word, 1);

            //        //ProgressInfo.Value = reader.BaseStream.Position / (double)reader.BaseStream.Length;
            //        ProgressInfo.Dispatcher.Invoke(() =>
            //            ProgressInfo.Value = reader.BaseStream.Position / (double)reader.BaseStream.Length);
            //    }
            //}

            //var count = Dictionary.Count;
            //Result.Text = $"Число слов {Count}";
            //Result.Dispatcher.Invoke(() => Result.Text = $"Число слов {Count}");

            StartAction.IsEnabled = false;
            CancelAction.IsEnabled = true;

            _ReadingFileCancellation = new CancellationTokenSource();

            var cancel = _ReadingFileCancellation.Token;
            IProgress<double> progress = new Progress<double>(p => ProgressInfo.Value = p);

            try
            {
                var count = await GetWordsCountAsync(dialog.FileName, progress, cancel).ConfigureAwait(true);
                Result.Text = $"Число слов {count}";
            }
            catch(OperationCanceledException)
            {
                Debug.WriteLine("Операция чтения файла была отменена");
                Result.Text = "---";
                progress.Report(0);
            }

            CancelAction.IsEnabled = false;
            StartAction.IsEnabled = true;
        }

        private static async Task<int> GetWordsCountAsync(string FileName, IProgress<double> Progress = null, CancellationToken Cancel = default)
        {
            // Мы находимся в ThreadId == 7 (например)
            await Task.Yield();
            // Теперь мы в ThreadId == 12 (например) (а, возможно и обратно в 7)

            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            Cancel.ThrowIfCancellationRequested();
            using (var reader = new StreamReader(FileName))
            {
                while (!reader.EndOfStream)
                {
                    Cancel.ThrowIfCancellationRequested();
                    var line = await reader.ReadLineAsync().ConfigureAwait(false);
                    // .ConfigureAwait(false); - требование "вернуться" в произвольный поток из пула потоков
                    var words = line.Split(' ');
                    //Thread.Sleep(100);
                    await Task.Delay(1);

                    foreach (var word in words)
                        if (dict.ContainsKey(word))
                            dict[word]++;
                        else
                            dict.Add(word, 1);

                    Progress?.Report(reader.BaseStream.Position / (double)reader.BaseStream.Length);
                }
            }
            return dict.Count;
        }

        private CancellationTokenSource _ReadingFileCancellation;

        private void OnCancelReadingClick(object sender, RoutedEventArgs e)
        {
            _ReadingFileCancellation?.Cancel();
        }
    }
}
