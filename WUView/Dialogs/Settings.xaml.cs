// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Dialogs
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        #region Toggle an increased shadow effect when mouse is over Card
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            Card card = sender as Card;
            ShadowAssist.SetShadowDepth(card, ShadowDepth.Depth4);
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Card card = sender as Card;
            ShadowAssist.SetShadowDepth(card, ShadowDepth.Depth2);
        }
        #endregion Toggle an increased shadow effect when mouse is over Card
    }
}
