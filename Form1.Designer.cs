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
            ((System.ComponentModel.ISupportInitialize)GameField).BeginInit();
            SuspendLayout();
            // 
            // GameField
            // 
            GameField.BackColor = SystemColors.AppWorkspace;
            GameField.Location = new Point(0, 0);
            GameField.Name = "GameField";
            GameField.Size = new Size(1024, 608);
            GameField.TabIndex = 0;
            GameField.TabStop = false;
            // 
            // GameTimer
            // 
            GameTimer.Enabled = true;
            GameTimer.Interval = 300;
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
            // TronGame
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 676);
            Controls.Add(GameField);
            Name = "TronGame";
            Text = "TRON GAME";
            Load += TronGameLoad;
            KeyDown += TronGameKeyDown;
            ((System.ComponentModel.ISupportInitialize)GameField).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox GameField;
        private System.Windows.Forms.Timer GameTimer;
        private ImageList GameGPX;
    }
}
