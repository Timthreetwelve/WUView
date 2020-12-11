// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Windows;

namespace WUView
{
    public partial class Excludes : Window
    {
        public Excludes()
        {
            InitializeComponent();
            foreach (var line in ExcludedItems.ExcludedStrings)
            {
                tb1.AppendText(line.ExcludedString);
                tb1.AppendText(Environment.NewLine);
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            List<ExcludedItems> lines = new List<ExcludedItems>();

            for (int line = 0; line < tb1.LineCount; line++)
            {
                ExcludedItems xi = new ExcludedItems();
                // GetLineText takes a zero-based line index.
                string tbline = tb1.GetLineText(line);
                if (!string.IsNullOrWhiteSpace(tbline))
                {
                    xi.ExcludedString = tbline.TrimEnd('\n').TrimEnd('\r');
                    lines.Add(xi);
                }
            }
            ExcludedItems.ExcludedStrings = lines;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
