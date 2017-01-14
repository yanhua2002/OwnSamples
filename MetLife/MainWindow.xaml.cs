using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MetLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Label[] labels;

        int[] randomArray;
        List<string> candiList;

        string csvFileName = "sample.csv";
        int numToDraw=10;

        bool rolling = false;
        bool nextFlag = false;

        Queue<string> awardNameQue;
        Queue<int> awardNumQue;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            randomArray = GetRandoms(0, candiList.Count, numToDraw);

            for (int i = 0; i < numToDraw; i++)
            {
                labels[i].Content = candiList[randomArray[i]];
            }
        }

        private int[] GetRandoms(int startNum,int endNum,int count)
        {
            int number, j, i = 0;
            Random random = new Random();
            int[] tempArray = new int[count];

            while (i<count)
            {
                number = random.Next(startNum, endNum);
                for (j = 0; j < i; j++)
                {
                    if (number == tempArray[j]) break;
                }

                if (j==i)
                {
                    tempArray[i] = number;
                    i++;
                }
            }

            return tempArray;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.None;

            var candidates = File.ReadAllLines(csvFileName);
            candiList = new List<string>();
            foreach (var name in candidates)
            {
                candiList.Add(name);
            }

            candiList.Remove("XXX");

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.05);
            timer.Tick += Timer_Tick;

            labels = new Label[] { lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, lbl7, lbl8, lbl9 };

            awardNameQue = new Queue<string>();
            awardNameQue.Enqueue("三等奖");
            awardNameQue.Enqueue("二等奖");
            awardNameQue.Enqueue("一等奖");
            awardNumQue = new Queue<int>();
            awardNumQue.Enqueue(10);
            awardNumQue.Enqueue(6);
            awardNumQue.Enqueue(3);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
                return;

            if(rolling)
            {
                // stop roll
                timer.Stop();

                // replace xxx
                if(numToDraw==3)
                {
                    lbl2.Content = "XXX";
                }

                // remove candidates
                randomArray = randomArray.OrderBy(i => i).ToArray();
                for (int i = randomArray.Length; i >0 ; i--)
                {
                    candiList.RemoveAt(randomArray[i-1]);
                }

                rolling = false;
            }
            else
            {
                if(nextFlag)
                {
                    // start roll
                    timer.Start();

                    rolling = true;
                    nextFlag = false;
                }
                else
                {
                    if(awardNameQue.Count==0)
                    {
                        midWarpPanel.Visibility = Visibility.Collapsed;
                        bottomStackPanel.Visibility = Visibility.Collapsed;

                        return;
                    }

                    // set some vars, restore the UI...
                    lblAwards2.Content = awardNameQue.Dequeue();
                    numToDraw = awardNumQue.Dequeue();
                    lblNum.Content = numToDraw;
                    foreach (var label in labels)
                    {
                        label.Content = "*****";
                    }
                    for (int i = 9; i > numToDraw-1; i--)
                    {
                        labels[i].Visibility = Visibility.Collapsed;
                    }
                    if (numToDraw < 6)  // when less than 6, make the labels one column
                    {
                        midWarpPanel.Width = 500.0;
                    }
                    midWarpPanel.Visibility = Visibility.Visible;
                    bottomStackPanel.Visibility = Visibility.Visible;

                    nextFlag = true;
                }
            }
        }
    }
}
