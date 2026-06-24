using System;
using System.Drawing;
using System.Windows.Forms;

namespace TranslatorUI
{
    public partial class Form1 : Form
    {
        private TextBox txtInput, txtOutput;
        private Label lblDetectedLang;
        private Button btnTranslateToUk, btnTranslateToEn;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Step Lab - Global Support Communication";
            this.Size = new Size(800, 550);
            this.BackColor = Color.WhiteSmoke;

            // Вхідне повідомлення
            this.Controls.Add(new Label { Text = "Вхідне повідомлення від клієнта / постачальника:", Location = new Point(20, 15), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            txtInput = new TextBox { Multiline = true, Location = new Point(20, 40), Size = new Size(740, 120), Font = new Font("Arial", 11) };
            txtInput.Text = "Hello! We are ready to ship the new batch of CAD/CAM 3D printing materials for your orthopedic insoles production. Please confirm the delivery address.";

            // Панель управління
            lblDetectedLang = new Label { Text = "Визначена мова: очікування...", Location = new Point(20, 175), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Italic), ForeColor = Color.DarkGray };

            btnTranslateToUk = new Button { Text = "Перекласти на Українську (для менеджера)", Location = new Point(20, 200), Size = new Size(360, 40), BackColor = Color.LightBlue, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnTranslateToUk.Click += (s, e) => RunTranslationSimulation("uk");

            btnTranslateToEn = new Button { Text = "Перекласти на Англійську (відповідь B2)", Location = new Point(400, 200), Size = new Size(360, 40), BackColor = Color.LightGreen, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnTranslateToEn.Click += (s, e) => RunTranslationSimulation("en");

            // Вихідне повідомлення
            this.Controls.Add(new Label { Text = "Результат перекладу:", Location = new Point(20, 260), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            txtOutput = new TextBox { Multiline = true, Location = new Point(20, 285), Size = new Size(740, 180), Font = new Font("Arial", 11), ReadOnly = true, BackColor = Color.White };

            this.Controls.Add(txtInput); this.Controls.Add(lblDetectedLang);
            this.Controls.Add(btnTranslateToUk); this.Controls.Add(btnTranslateToEn);
            this.Controls.Add(txtOutput);
        }

        private void RunTranslationSimulation(string targetLang)
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text)) return;

            if (targetLang == "uk")
            {
                lblDetectedLang.Text = "Автоматично визначена мова: Англійська (en) | Точність: 100%";
                lblDetectedLang.ForeColor = Color.DarkGreen;
                txtOutput.Text = "Вітаю! Ми готові відправити нову партію матеріалів для 3D-друку CAD/CAM для вашого виробництва ортопедичних устілок. Будь ласка, підтвердіть адресу доставки.";
            }
            else if (targetLang == "en")
            {
                lblDetectedLang.Text = "Автоматично визначена мова: Українська (uk) | Точність: 100%";
                lblDetectedLang.ForeColor = Color.DarkGreen;
                txtOutput.Text = "Good afternoon. We confirm the readiness to receive the materials. The delivery address remains the same. Thank you for your promptness.";
            }
        }
    }
}