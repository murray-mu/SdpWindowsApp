/*
	Copyright NetFoundry Inc.

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	https://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Media.Animation;
using C8Edge.Models;
using C8Edge.ServiceClient;
using C8Edge.DataStructures;

using NLog;
using QRCoder;
using System.Text.RegularExpressions;

namespace C8Edge {
    /// <summary>
    /// Interaction logic for MFA.xaml
    /// </summary>
    public partial class MFAScreen : UserControl {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public delegate void LoadEvent(bool isComplete, string title, string message);
        public event LoadEvent OnLoad;
        public event CommonDelegates.CloseAction OnClose;
        private string _url = "";
        public delegate void ErrorOccurred(string message);
        public event ErrorOccurred OnError;
        private string[] _codes = new string[0];
        private ZitiIdentity zid;
        private bool _executing = false;
        public int Type { get; set; }

        public ZitiIdentity Identity {
            get {
                return this.zid;
            }
            set {
                this.zid = value;
            }
        }

        public MFAScreen() {
            InitializeComponent();
        }

        private void ExecuteClose(object sender, MouseButtonEventArgs e) {
            this.OnClose?.Invoke(false, this);
        }

        private void ShowError(string message) {
            this.OnError?.Invoke(message);
        }

        private BitmapImage CreateQRFromUrl(string url) {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);

            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)qrCodeImage).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public void ShowSetup(ZitiIdentity identity, string url, string secret) {
            SetupCode.Text = "";
            this.zid = identity;
            MFAImage.Visibility = Visibility.Visible;
            SecretCode.Visibility = Visibility.Collapsed;
            CloseBlack.Visibility = Visibility.Visible;
            CloseWhite.Visibility = Visibility.Collapsed;
            SecretButton.Content = "Show Secret";
            IdName.Content = identity.Name;
            Logger.Debug($"MFA Url: {url}");
            AuthBrush.Visibility = Visibility.Collapsed;
            MainBrush.Visibility = Visibility.Visible;

            MFAImage.Source = CreateQRFromUrl(url);
            SecretCode.Text = secret;

            _url = url;
            MFAArea.Height = 515;
            MFAAuthArea.Visibility = Visibility.Collapsed;
            MFASetupArea.Visibility = Visibility.Visible;
            MFARecoveryArea.Visibility = Visibility.Collapsed;
            SeperationColor.Visibility = Visibility.Visible;
            SetupCode.Focus();
        }

        public void ShowRecovery(string[] codes, ZitiIdentity identity) {
            this.zid = identity;
            MFASetupArea.Visibility = Visibility.Collapsed;
            MFAAuthArea.Visibility = Visibility.Collapsed;
            SeperationColor.Visibility = Visibility.Collapsed;
            MFARecoveryArea.Visibility = Visibility.Visible;
            RecoveryList.Children.Clear();
            _codes = codes;
            AuthBrush.Visibility = Visibility.Collapsed;
            MainBrush.Visibility = Visibility.Visible;
            CloseBlack.Visibility = Visibility.Visible;
            CloseWhite.Visibility = Visibility.Collapsed;
            MFAArea.Height = 380;
            if (codes.Length > 0) {
                for (int i = 0; i < codes.Length; i++) {
                    TextBox label = new TextBox();
                    label.Text = codes[i];
                    label.BorderThickness = new Thickness(0);
                    label.Background = new SolidColorBrush();
                    label.Background.Opacity = 0;
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    RecoveryList.Children.Add(label);
                }
                RecoveryList.Visibility = Visibility.Visible;
                NoRecovery.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Visible;
            } else {

                ShowMFA(this.zid, 2);
                SaveButton.Visibility = Visibility.Collapsed;
                RecoveryList.Visibility = Visibility.Collapsed;
                NoRecovery.Visibility = Visibility.Visible;
            }
        }

        public void ShowMFA(ZitiIdentity identity, int type) {
            this.Type = type;
            AuthCode.Text = "";
            AuthBrush.Visibility = Visibility.Visible;
            MainBrush.Visibility = Visibility.Collapsed;
            CloseBlack.Visibility = Visibility.Collapsed;
            CloseWhite.Visibility = Visibility.Visible;
            this.zid = identity;
            MFASetupArea.Visibility = Visibility.Collapsed;
            MFARecoveryArea.Visibility = Visibility.Collapsed;
            SeperationColor.Visibility = Visibility.Collapsed;
            MFAAuthArea.Visibility = Visibility.Visible;
            MFAArea.Height = 220;
            AuthCode.Focusable = true;
            AuthCode.Focus();
        }

        private BitmapImage LoadImage(string url) {
            var imgUrl = new Uri(url);
            var imageData = new WebClient().DownloadData(imgUrl);
            var bitmapImage = new BitmapImage { CacheOption = BitmapCacheOption.OnLoad };
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private void GoTo(object sender, MouseButtonEventArgs e) {
            if (_url != null && _url.Length > 0) {
                Process.Start(new ProcessStartInfo(_url) { UseShellExecute = true });
            } else {
                this.OnError?.Invoke("Invalid MFA Url");
            }
        }

        async private void GetMFACodes(object sender, MouseButtonEventArgs e) {
            DataClient serviceClient = serviceClient = (DataClient)Application.Current.Properties["ServiceClient"];
            string code = AuthCode.Text;
            Logger.Debug("AuthMFA successful.");
            MfaRecoveryCodesResponse getcodes = await serviceClient.GetMFACodes(this.zid.Identifier, code);
            if (getcodes.Code != 0) {
                Logger.Error("AuthMFA failed. " + getcodes.Error);
            }
            Logger.Error("DATA: {0}", getcodes.Data);
        }
        async private void GenerateMFACodes(object sender, MouseButtonEventArgs e) {
            DataClient serviceClient = serviceClient = (DataClient)Application.Current.Properties["ServiceClient"];
            string code = AuthCode.Text;
            MfaRecoveryCodesResponse gencodes = await serviceClient.GenerateMFACodes(this.zid.Identifier, code);
            if (gencodes.Code != 0) {
                Logger.Error("AuthMFA failed. " + gencodes.Error);
            }
            Logger.Error("DATA: {0}", gencodes.Data);
        }

        private void SaveCodes(object sender, MouseButtonEventArgs e) {
            string fileText = string.Join("\n", _codes);
            string name = Regex.Replace(this.zid.Name, "[^a-zA-Z0-9]", String.Empty);

            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Text Files(*.txt)|*.txt|All(*.*)|*";
            dialog.Title = "Save Recovery Codes";
            dialog.FileName = name + "RecoveryCodes.txt";

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel) {
                File.WriteAllText(dialog.FileName, fileText);
            }
        }

        async private void DoSetupAuthenticate(object sender, MouseButtonEventArgs e) {
            string code = SetupCode.Text;

            DataClient serviceClient = serviceClient = (DataClient)Application.Current.Properties["ServiceClient"];
            SvcResponse resp = await serviceClient.VerifyMFA(this.zid.Identifier, code);
            if (resp.Code != 0) {
                this.OnClose?.Invoke(false, this);
            } else {
                this.OnClose?.Invoke(false, this);
            }
        }

        /// <summary>
        /// Call the MFA Functions
        /// 
        /// Types:
        /// 1 = Normal MFA Authentication
        /// 2 = Get Recovery Codes
        /// 3 = Remove MFA
        /// 4 = Generate New MFA Codes
        /// </summary>
        async private void DoAuthenticate(object sender, MouseButtonEventArgs e) {
            if (!this._executing) {

                this._executing = true;
                string code = AuthCode.Text;

                if (code.Trim().Length > 0) {

                    DataClient serviceClient = (DataClient)Application.Current.Properties["ServiceClient"];
                    this.OnLoad?.Invoke(false, "Authentication", "One Moment Please...");
                    if (this.Type == 1) {
                        SvcResponse authResult = await serviceClient.AuthMFA(this.zid.Identifier, code);
                        if (authResult?.Code != 0) {
                            Logger.Error("AuthMFA failed. " + authResult.Error);
                            this.OnError?.Invoke("Authentication Failed");
                            this._executing = false;
                        } else {
                            this.zid.IsMFANeeded = true;
                            this.OnClose?.Invoke(true, this);
                            this._executing = false;
                        }
                        this.OnLoad?.Invoke(true, "", "");
                    } else if (this.Type == 2) {
                        MfaRecoveryCodesResponse codeResponse = await serviceClient.GetMFACodes(this.zid.Identifier, code);
                        if (codeResponse?.Code != 0) {
                            Logger.Error("AuthMFA failed. " + codeResponse.Error);
                            AuthCode.Text = "";
                            this.OnError?.Invoke("Authentication Failed");
                            this._executing = false;
                        } else {
                            this.zid.RecoveryCodes = codeResponse.Data.RecoveryCodes;
                            this.OnClose?.Invoke(true, this);
                            this._executing = false;
                        }
                        this.OnLoad?.Invoke(true, "", "");
                    } else if (this.Type == 3) {
                        SvcResponse authResult = await serviceClient.RemoveMFA(this.zid.Identifier, code);
                        if (authResult?.Code != 0) {
                            Logger.Error("AuthMFA failed. " + authResult.Error);
                            AuthCode.Text = "";
                            this.OnError?.Invoke("Authentication Failed");
                            this._executing = false;
                        } else {
                            this.OnClose?.Invoke(true, this);
                            this._executing = false;
                        }
                        this.OnLoad?.Invoke(true, "", "");
                    } else if (this.Type == 4) {
                        MfaRecoveryCodesResponse codeResponse = await serviceClient.GenerateMFACodes(this.zid.Identifier, code);
                        if (codeResponse?.Code != 0) {
                            Logger.Error("AuthMFA failed. " + codeResponse?.Error);
                            AuthCode.Text = "";
                            this.OnError?.Invoke("Authentication Failed");
                        } else {
                            this.zid.RecoveryCodes = codeResponse.Data.RecoveryCodes;
                            this.OnClose?.Invoke(true, this);
                        }
                        this._executing = false;
                        this.OnLoad?.Invoke(true, "", "");
                    }
                }
            }
        }

        private void RegenerateCodes(object sender, MouseButtonEventArgs e) {
            ShowMFA(this.zid, 4);
        }

        private void ShowSecret(object sender, MouseButtonEventArgs e) {
            if (SecretCode.Visibility == Visibility.Visible) {
                MFAImage.Visibility = Visibility.Visible;
                SecretCode.Visibility = Visibility.Collapsed;
                SecretButton.Content = "Show Secret";
            } else {
                MFAImage.Visibility = Visibility.Collapsed;
                SecretCode.Visibility = Visibility.Visible;
                SecretButton.Content = "Show QR Code";
            }
        }

        private void HandleKey(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                DoSetupAuthenticate(sender, null);
            }
        }

        private void AuthCode_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                DoAuthenticate(sender, null);
            }
        }
    }
}
