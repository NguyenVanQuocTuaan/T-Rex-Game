using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace T_Rex_Game
{
    public partial class Form1 : Form
    {
        bool jumping = false;
        int jumpSpeed;
        int force = 0;
        int obstacleSpeed = 10;
        Random rand = new Random();
        int position;
        int score;
        bool isGameOver = false;
        int count = 0;
        int HighScore = 0;
        public Form1()
        {
            InitializeComponent();
            GameReset();
        }

         //Phan Timer
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            Trex.Top += jumpSpeed;
            txtScore.Text = "Score: " + score;

            if (jumping == true && force < 0)
            {
                jumping = false;
            }
            if (jumping == true)
            {
                jumpSpeed = -20;
                force -= 2;
            }
            else
            {
                jumpSpeed = 10;
            }

            if (Trex.Top > 348 && jumping == false)
            {
                force = 12;
                Trex.Top = 349;
                jumpSpeed = 0;
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle1")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left < 0)
                    {
                        
                        x.Left = this.ClientSize.Width + rand.Next(200, 250) + (x.Width * 15);
                        score++;
                        count++;
                    }
                    if (Trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        SoundPlayer deadSound = new SoundPlayer(@"C:\Users\ACER\Desktop\Lập trình Window\T-Rex Game\T-Rex Game\SoundResources\dead.wav");
                        deadSound.Play();
                        gameTimer.Stop();
                        Trex.Image = Properties.Resources.dead;
                        txtScore.Text += "  Press R to restart the game!";
                        isGameOver = true;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "obstacle2")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left < 0)
                    {
                      
                        x.Left = this.ClientSize.Width + rand.Next(300, 400) + (x.Width * 15);
                        score++;
                        count++;
                    }
                    if (Trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        SoundPlayer deadSound = new SoundPlayer(@"C:\Users\ACER\Desktop\Lập trình Window\T-Rex Game\T-Rex Game\SoundResources\dead.wav");
                        deadSound.Play();
                        gameTimer.Stop();
                        Trex.Image = Properties.Resources.dead;
                        txtScore.Text += "  Press R to restart the game!";
                        isGameOver = true;
                        
                    }
                }
            }

            if (count == 5)
            {
                obstacleSpeed += 1;
                count = 0;
                SoundPlayer scoreSound = new SoundPlayer(@"C:\Users\ACER\Desktop\Lập trình Window\T-Rex Game\T-Rex Game\SoundResources\score.wav");
                scoreSound.Play();
            }
        }
        //Nhan phim
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && jumping == false && Trex.Top == 349)
            {
                SoundPlayer jumpSound = new SoundPlayer(@"C:\Users\ACER\Desktop\Lập trình Window\T-Rex Game\T-Rex Game\SoundResources\jump.wav");
                jumpSound.Play();
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                count = 0;
                if(score > HighScore)
                {
                    txtHighScore.Text = "High Score: " + score;
                    HighScore = score;
                }
                GameReset();
                
            }
        }
        //Reset game
        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Score: " + score;
            Trex.Image = Properties.Resources.running;
            isGameOver = false;
            Trex.Top = 348;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle1")
                {
                    position = this.ClientSize.Width + rand.Next(500, 600) + (x.Width * 10);
                    x.Left = position;
                }
                if (x is PictureBox && (string)x.Tag == "obstacle2")
                {
                    position = this.ClientSize.Width + rand.Next(700, 800) + (x.Width * 10);
                    x.Left = position;
                }
            }
            gameTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
