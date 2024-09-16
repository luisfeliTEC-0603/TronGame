
namespace TronGame
{
    partial class TronGame
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TronGame));
            GameField = new PictureBox();
            GameTimer = new System.Windows.Forms.Timer(components);
            GameGPX = new ImageList(components);
            energyLabel = new Label();
            energyBar = new ProgressBar();
            speedInfo = new Label();
            PowerUps = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)GameField).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PowerUps).BeginInit();
            SuspendLayout();
            // 
            // GameField
            // 
            GameField.BackColor = Color.Black;
            GameField.BorderStyle = BorderStyle.FixedSingle;
            GameField.Enabled = false;
            GameField.Location = new Point(0, 0);
            GameField.Name = "GameField";
            GameField.Size = new Size(1024, 608);
            GameField.TabIndex = 0;
            GameField.TabStop = false;
            // 
            // GameTimer
            // 
            GameTimer.Enabled = true;
            GameTimer.Interval = 175;
            GameTimer.Tick += GameLoop;
            // 
            // GameGPX
            // 
            GameGPX.ColorDepth = ColorDepth.Depth32Bit;
            GameGPX.ImageStream = (ImageListStreamer)resources.GetObject("GameGPX.ImageStream");
            GameGPX.TransparentColor = Color.Transparent;
            GameGPX.Images.SetKeyName(0, "Void.png");
            GameGPX.Images.SetKeyName(1, "Bomb.png");
            GameGPX.Images.SetKeyName(2, "Energy.png");
            GameGPX.Images.SetKeyName(3, "HyperVelocity.png");
            GameGPX.Images.SetKeyName(4, "NewJet.png");
            GameGPX.Images.SetKeyName(5, "Shield.png");
            GameGPX.Images.SetKeyName(6, "Inv-JetWall.png");
            GameGPX.Images.SetKeyName(7, "Player-JetWall.png");
            GameGPX.Images.SetKeyName(8, "Bot-JetWall.png");
            GameGPX.Images.SetKeyName(9, "Invencible.png");
            GameGPX.Images.SetKeyName(10, "Player.png");
            GameGPX.Images.SetKeyName(11, "Bot.png");
            // 
            // energyLabel
            // 
            energyLabel.AutoSize = true;
            energyLabel.Font = new Font("Pixel-Art", 16F, FontStyle.Italic);
            energyLabel.ForeColor = SystemColors.Window;
            energyLabel.Location = new Point(12, 628);
            energyLabel.Name = "energyLabel";
            energyLabel.Size = new Size(174, 19);
            energyLabel.TabIndex = 1;
            energyLabel.Text = "Energy Lvl.";
            // 
            // energyBar
            // 
            energyBar.ForeColor = SystemColors.HotTrack;
            energyBar.Location = new Point(12, 659);
            energyBar.Maximum = 500;
            energyBar.Name = "energyBar";
            energyBar.Size = new Size(266, 3);
            energyBar.Step = -1;
            energyBar.TabIndex = 2;
            // 
            // speedInfo
            // 
            speedInfo.AutoSize = true;
            speedInfo.Font = new Font("Pixel-Art", 16F, FontStyle.Italic);
            speedInfo.ForeColor = SystemColors.Window;
            speedInfo.Location = new Point(800, 643);
            speedInfo.Name = "speedInfo";
            speedInfo.Size = new Size(185, 19);
            speedInfo.TabIndex = 3;
            speedInfo.Text = "Speed | MARC ";
            // 
            // PowerUps
            // 
            PowerUps.Location = new Point(297, 614);
            PowerUps.Name = "PowerUps";
            PowerUps.Size = new Size(488, 50);
            PowerUps.TabIndex = 4;
            PowerUps.TabStop = false;
            // 
            // TronGame
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(1024, 676);
            Controls.Add(PowerUps);
            Controls.Add(speedInfo);
            Controls.Add(energyBar);
            Controls.Add(energyLabel);
            Controls.Add(GameField);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "TronGame";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TRON GAME";
            Load += TronGameLoad;
            KeyDown += TronGameKeyDown;
            ((System.ComponentModel.ISupportInitialize)GameField).EndInit();
            ((System.ComponentModel.ISupportInitialize)PowerUps).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox GameField;
        private System.Windows.Forms.Timer GameTimer;
        private ImageList GameGPX;
        private Label energyLabel;
        public ProgressBar energyBar;
        private Label speedInfo;
        private PictureBox PowerUps;
    }
}
