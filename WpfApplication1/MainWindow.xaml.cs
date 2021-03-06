﻿using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region

        static string filePath = @"%UserProfile%\Desktop";
        string connString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=" + Environment.ExpandEnvironmentVariables(filePath);

        DataTable dtTable = new DataTable();

        Awards drawingAward = Awards.A;

        int allCandiNum, restCandiNum;           // 所有抽奖的总人数，剩余抽奖的人数
        int totalNumToDraw, totalTimesToDraw;    // 当前奖项共抽取多少人，需抽取多少次
        int drawingTime = 0;                    // 当前奖项在抽取第几次
        int lastRound, thisRound = 10;          // 当前奖项最后一次抽取需抽取多少人

        int[] random;                           // 每一次滚动时的随机序列

        DispatcherTimer timer;
        Label[] labels;

        bool isRolling = false;

        bool lastRoundStop = true;              // 当前奖项最后一次抽取是否完成，用于切换至下一奖项时的暂停画面

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += timer_Tick;

            labels = new Label[] { lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6, lbl7, lbl8, lbl9 };
        }

        public void InitializeCandidates()
        {
            string fileName = "Candidates";

            try
            {
                using (OdbcConnection odbcConn = new OdbcConnection(connString))
                {
                    odbcConn.Open();

                    string commandString = "SELECT * FROM [" + fileName + ".csv]";
                    OdbcDataAdapter adapter = new OdbcDataAdapter(commandString, odbcConn);

                    adapter.Fill(dtTable);

                    dtTable.Columns[0].ColumnName = "ID";
                    dtTable.Columns[1].ColumnName = "Name";

                    restCandiNum = allCandiNum = dtTable.Rows.Count;

                    totalNumToDraw = (int)Math.Floor((double)allCandiNum * 0.2);
                    totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
                    if (lastRound == 0) totalTimesToDraw--;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCandidates();

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
                return;


            switch (drawingAward)
            {
                case Awards.A:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\28.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward = Awards.B;

                            totalNumToDraw = (int)Math.Floor((double)allCandiNum * 0.2);
                            totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
                            if (lastRound == 0) totalTimesToDraw--;
                            drawingTime = 0;
                            thisRound = 10;

                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "--------";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;

                            lblAward.Content = "28元档";
                            lblRest.Content = dtTable.Rows.Count;
                            lblPercent.Content = "20%  = ";
                            lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        if (drawingTime == totalTimesToDraw && lastRound != 0)
                        {
                            thisRound = lastRound;
                            for (int i = 9; i >= lastRound; i--)
                            {
                                labels[i].Visibility = Visibility.Collapsed;
                            }
                        }
                        timer.Start();
                        isRolling = true;

                    }
                    break;
                case Awards.B:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\38.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward = Awards.C;

                            totalNumToDraw = (int)Math.Floor((double)allCandiNum * 0.3);
                            totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
                            if (lastRound == 0) totalTimesToDraw--;
                            drawingTime = 0;
                            thisRound = 10;

                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "--------";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;

                            lblAward.Content = "38元档";
                            lblRest.Content = dtTable.Rows.Count;
                            lblPercent.Content = "20%  = ";
                            lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        if (drawingTime == totalTimesToDraw && lastRound != 0)
                        {
                            thisRound = lastRound;
                            for (int i = 9; i >= lastRound; i--)
                            {
                                labels[i].Visibility = Visibility.Collapsed;
                            }
                        }
                        timer.Start();
                        isRolling = true;

                    }
                    break;
                case Awards.C:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\48.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward = Awards.D;

                            totalNumToDraw = allCandiNum - (int)Math.Floor((double)allCandiNum * 0.2) - (int)Math.Floor((double)allCandiNum * 0.2) - (int)Math.Floor((double)allCandiNum * 0.3);
                            totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
                            if (lastRound == 0) totalTimesToDraw--;
                            drawingTime = 0;
                            thisRound = 10;

                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "--------";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;

                            lblAward.Content = "48元档";
                            lblRest.Content = dtTable.Rows.Count;
                            lblPercent.Content = "30%  = ";
                            lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        if (drawingTime == totalTimesToDraw && lastRound != 0)
                        {
                            thisRound = lastRound;
                            for (int i = 9; i >= lastRound; i--)
                            {
                                labels[i].Visibility = Visibility.Collapsed;
                            }
                        }
                        timer.Start();
                        isRolling = true;

                    }
                    break;
                case Awards.D:
                    if (isRolling)
                    {
                        timer.Stop();
                        // 保存数据
                        // 从候选人员中减去
                        SaveToCsv(Environment.ExpandEnvironmentVariables(@"%UserProfile%\Desktop\58.csv"));
                        isRolling = false;
                        if (drawingTime == totalTimesToDraw)    // if(drawingTime==totalTimeToDraw)
                        {
                            drawingAward = Awards.None;

                            //totalNumToDraw = (int)Math.Floor((double)allCandiNum * 0.2);
                            //totalTimesToDraw = Math.DivRem(totalNumToDraw, 10, out lastRound) + 1;
                            //if (lastRound == 0) totalTimesToDraw--;
                            //drawingTime = 0;
                            //thisRound = 10;

                            lastRoundStop = true;
                        }
                    }
                    else
                    {
                        if (lastRoundStop)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                labels[i].Content = "--------";
                                labels[i].Visibility = Visibility.Visible;
                            }
                            lastRoundStop = false;

                            lblAward.Content = "58元档";
                            lblRest.Content = dtTable.Rows.Count;
                            lblPercent.Content = "20%  = ";
                            lblNumToDraw.Content = totalNumToDraw;
                            break;
                        }

                        drawingTime++;
                        if (drawingTime == totalTimesToDraw && lastRound != 0)
                        {
                            thisRound = lastRound;
                            for (int i = 9; i >= lastRound; i--)
                            {
                                labels[i].Visibility = Visibility.Collapsed;
                            }
                        }
                        timer.Start();
                        isRolling = true;

                    }
                    break;
                case Awards.None:
                default:
                    break;
            }


        }

        void timer_Tick(object sender, EventArgs e)
        {
            random = GetRandom(0, restCandiNum - 1, thisRound);

            for (int i = 0; i < thisRound; i++)
            {
                labels[i].Content = dtTable.Rows[random[i]]["ID"].ToString() + "---" + dtTable.Rows[random[i]]["Name"].ToString();
            }
        }

        private static int[] GetRandom(int min, int max, int count)
        {
            int[] maxArray = new int[max - min + 1];
            for (int i = 0; i < max - min + 1; i++)
                maxArray[i] = min + i;

            int[] rArray = new int[count];
            Random rd = new Random();
            int temp = max - min + 1;
            for (int i = 0; i < count; i++)
            {
                int tIndex = rd.Next(0, temp);
                rArray[i] = maxArray[tIndex];
                maxArray[tIndex] = maxArray[--temp];
            }
            return rArray;
        }

        enum Awards
        {
            A, B, C, D, None
        }

        void SaveToCsv(string csvName)
        {
            StreamWriter sw = new StreamWriter(csvName, true, Encoding.Default);
            string data = "";

            for (int i = 0; i < random.Count(); i++)
            {
                data = "";
                for (int j = 0; j < dtTable.Columns.Count; j++)
                {
                    data += dtTable.Rows[random[i]][j].ToString();
                    if (j < dtTable.Columns.Count - 1)
                        data += ",";
                }
                sw.WriteLine(data);
                dtTable.Rows[random[i]].Delete();
            }

            dtTable.AcceptChanges();
            restCandiNum = restCandiNum - random.Count();

            sw.Close();
        }
    }
}
