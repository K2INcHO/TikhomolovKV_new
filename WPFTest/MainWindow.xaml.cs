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
            // Мы находимся в ThreadId == 1

            var thread_id = Thread.CurrentThread.ManagedThreadId;

            await Task.Yield().ConfigureAwait(false);

            var thread_id2 = Thread.CurrentThread.ManagedThreadId;

            // Мы находимся в ThreadId == 1

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
                    //await Task.Delay(1, Cancel).ConfigureAwait(false);

                    foreach (var word in words)
                        if (dict.ContainsKey(word))
                            dict[word]++;
                        else
                            dict.Add(word, 1);

                    Progress?.Report(reader.BaseStream.Position / (double)reader.BaseStream.Length);
                }
            }

            var thread_id3 = Thread.CurrentThread.ManagedThreadId;
            
            await Application.Current.Dispatcher();

            var thread_id4 = Thread.CurrentThread.ManagedThreadId;

            return dict.Count;
        }

        private CancellationTokenSource _ReadingFileCancellation;

        private void OnCancelReadingClick(object Sender, RoutedEventArgs e)
        {
            _ReadingFileCancellation?.Cancel();
        }
    }
}
