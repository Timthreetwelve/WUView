// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Dialogs
{
    /// <summary>
    /// Dialog to edit the exclude list
    /// </summary>
    public partial class ExcludesEditor : UserControl
    {
        public ExcludesEditor()
        {
            InitializeComponent();

            PopulateTextBox();
        }

        #region Load existing exclude items into the textbox
        private void PopulateTextBox()
        {
            foreach (ExcludedItems line in ExcludedItems.ExcludedStrings)
            {
                tb1.AppendText(line.ExcludedString);
                tb1.AppendText(Environment.NewLine);
            }
        }
        #endregion Load existing exclude items into the textbox
    }
}
