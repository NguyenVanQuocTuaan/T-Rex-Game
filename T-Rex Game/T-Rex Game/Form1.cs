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
        int force = 0;//Do roi
        int obstacleSpeed = 10;//Toc do chuong ngai vat
        Random rand = new Random();
        int position;
        int score;
        bool isGameOver = false;
        int flag = 0;
        int highScore = 0;
        public Form1()
        {
            InitializeComponent();
            GameReset();

        }
        // Trex dead----------------------------------------------------
        private void deadTrex()
        {
            SoundPlayer deadSound = new SoundPlayer(Properties.SoundResources.dead);
            deadSound.Play();
            gameTimer.Stop();
            BirdTimer1.Stop();
            BirdTimer2.Stop();
            TrexDTimer1.Stop();
            TrexDTimer2.Stop();
            TrexUpTimer.Stop();
            Trex.Image = Properties.Resources.dead;
            refresh.Visible = true;
            lbGameOver.Visible = true;
            isGameOver = true;
            TrexDown.Visible = false;

        }
        // Phan HighScore lay so diem cao nhat---------------------------
        private void HighScore()
        {
            if (score > highScore)
            {
                txtHighScore.Text = "High Score: " + score;
                highScore = score;
            }
        }
        //Phan khoi tao--------------------------------------------------
        private void GameReset()
        {
            HighScore();
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            flag = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Score: " + score;
            lbGameOver.Visible = false;
            refresh.Visible = false;
            TrexDown.Visible = false;
            isGameOver = false;
            Trex.Top = 348;
            Trex.Image = Properties.Resources.running;
            //Tao vi tri bat dau cho cac doi tuong-
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
                if (x is PictureBox && (string)x.Tag == "bird")
                {
                    x.Top = this.ClientSize.Height - rand.Next(130,200);
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);
                    x.Left = position;
                }

            }

            gameTimer.Start();
        }
        //Phan Keys dieu khien hanh dong cua T-rex------------------------
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up && jumping == false && Trex.Top == 349)
            {
                SoundPlayer jumpSound = new SoundPlayer(Properties.SoundResources.jump);
                jumpSound.Play();
                jumping = true;
            }
            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();
            }
            if (e.KeyCode == Keys.Down && jumping == false && Trex.Top >= 300)
            {
                TrexDown.Visible = true;
                Trex.Visible = false;
                TrexDTimer1.Start();
                TrexDTimer2.Start();
                TrexUpTimer.Stop();

            }
            else
            {
                TrexDTimer1.Stop();
                TrexDTimer1.Stop();
            }
        }
        private void keyisup(object sender, KeyEventArgs e)
        {

            Trex.Visible = true;
            TrexDTimer1.Stop();
            TrexDTimer1.Stop();
            TrexUpTimer.Start();
            TrexDown.Visible = false;
        }
        //Nut refresh game------------------------------------------------ 
        private void refresh_MouseClick(object sender, MouseEventArgs e)
        {
            GameReset();
        }
        //Phan Timer cua Obstacle va TrexUp--------------------------------
        private void MainGameTimerEvent(object sender, EventArgs e)
        {

            Trex.Top += jumpSpeed;
            //Hieu ung nhay len
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
            txtScore.Text = "Score: " + score;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle1")
                {
                    x.Left -= obstacleSpeed; //Thay doi position lam vat chuyen dong
                    if (x.Left < 0)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(100, 200) + (x.Width * 15);//Tao lai vi tri vat khi vat di het man hinh
                        score++; // Tang diem
                        flag++;
                    }

                }
                if (x is PictureBox && (string)x.Tag == "obstacle2")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left < 0)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(300, 400) + (x.Width * 15);
                        score++;
                        flag++;
                    }

                }
            }
           
            if (score == 2)  // So diem can de cho Bird xuat hien
            {
                BirdTimer1.Start();
                BirdTimer2.Start();
            }
            if (flag == 5)// So diem de phat am thanh score 
            {
                obstacleSpeed += 1;
                flag = 0;
                SoundPlayer scoreSound = new SoundPlayer(Properties.SoundResources.score);
                scoreSound.Play();
            }
        }
        //Timer of Bird----------------------------------------------------
        private void BirdTimer1_Tick(object sender, EventArgs e)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "bird")
                {
                    x.Left -= obstacleSpeed;
                    Bird.Image = Properties.Resources.birdup;
                    if (x.Left < 0)
                    {
                        x.Top = this.ClientSize.Height - rand.Next(130, 200);
                        x.Left = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 15);
                    }
                   
                }

            }

        }
        private void BirdTimer2_Tick(object sender, EventArgs e)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "bird")
                {
                    x.Left -= obstacleSpeed;
                    Bird.Image = Properties.Resources.birddown;
                    if (x.Left < 0)
                    {
                        x.Top = this.ClientSize.Height - rand.Next(130, 200);
                        x.Left = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 15);
                    }
                   
                }

            }

        }
        //Timer of TrexDown------------------------------------------------
        private void TrexDTimer1_Tick(object sender, EventArgs e)
        {
            TrexDown.Image = Properties.Resources.trexdown1;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "obstacle1" || (string)x.Tag == "obstacle2" || (string)x.Tag == "bird"))
                {
                    if (TrexDown.Bounds.IntersectsWith(x.Bounds) && TrexDown.Visible == true)
                    {
                        deadTrex();    //Neu Trex cham vat se chet
                    }
                }
            }

        }

        private void TrexDTimer2_Tick(object sender, EventArgs e)
        {
            TrexDown.Image = Properties.Resources.trexdown2;
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "obstacle1" || (string)x.Tag == "obstacle2" || (string)x.Tag == "bird"))
                {
                    if (TrexDown.Bounds.IntersectsWith(x.Bounds) && TrexDown.Visible == true)
                    {
                        deadTrex();    //Neu Trex cham vat se chet
                    }
                }
            }

        }
        //Timer of TrexUp---------------------------------------------------
        private void TrexUpTimer_Tick(object sender, EventArgs e)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "obstacle1" || (string)x.Tag == "obstacle2" || (string)x.Tag == "bird"))
                {
                    if (Trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        deadTrex();    //Neu Trex cham vat se chet
                    }
                }
            }
        }
    }
}
