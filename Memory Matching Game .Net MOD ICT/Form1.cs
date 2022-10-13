using Microsoft.VisualBasic;

namespace Memory_Matching_Game_.Net_MOD_ICT

//16:50
// From: https://www.youtube.com/watch?v=EbEm5kohPfE&t=85s


{
public partial class Form1 : Form
{

    List<int> numbers = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
    String firstChoice;
    String secondChoice;
    int tries;
    List<PictureBox> Boxes = new List<PictureBox>();
    PictureBox picA;
    PictureBox picB;
    int totalTime = 30;
    int countDownTime;
    bool gameOver = false;

    public Form1()
    {
        InitializeComponent();
        LoadPictures();
    }

    private void TimerEvent(object sender, EventArgs e)
    {
        countDownTime--;
        lblTimeLeft.Text = "Time Left: " + countDownTime;

        if (countDownTime < 1)
        {
            GameOver("Times up, You Lose");

            foreach (PictureBox x in Boxes.ToList())
            {
                if (x.Tag != null)
                {
                    x.Image = Image.FromFile("pics/" + (string)x.Tag + ".png");
                }
            }
        }
    }

    private void RestartGameEvent(object sender, EventArgs e)
    {
        RestartGame();
    }

    private void LoadPictures()
    {
        int leftPos = 20;
        int topPos = 20;
        int rows = 0;

        for (int i = 0; i < 12; i++)
        {
            PictureBox newPic = new PictureBox();
            newPic.Height = 50;
            newPic.Width = 50;
            newPic.BackColor = Color.LightGray;
            newPic.SizeMode = PictureBoxSizeMode.StretchImage;
            newPic.Click += NewPic_Click;
            Boxes.Add(newPic);

            if (rows < 3)
            {
                rows++;
                newPic.Left = leftPos;
                newPic.Top = topPos;
                this.Controls.Add(newPic);
                leftPos = leftPos + 60;
            }

            if (rows == 3)
            {
                leftPos = 20;
                topPos += 60;
                rows = 0;
            }

        }

        RestartGame();
    }

    private void NewPic_Click(object sender, EventArgs e)
    {
        if (gameOver)
        {
            // dont register a click if the game is over
            return;
        }

        if (firstChoice == null)
        {
            picA = sender as PictureBox;
            if (picA.Tag != null && picA.Image == null)
            {
                picA.Image = Image.FromFile("pics/" + (string)picA.Tag + ".png");
                firstChoice = (string)picA.Tag;
            }
        }
        else if (secondChoice == null)
        {
            picB = sender as PictureBox;
            if (picB.Tag != null && picB.Image == null)
            {
                picB.Image = Image.FromFile("pics/" + (string)picB.Tag + ".png");
                secondChoice = (string)picB.Tag;
            }
        }

        else
        {
            checkPictures(picA, picB);
        }

    }


    private void RestartGame()
    {
        //randomise the original list
        var randomList = numbers.OrderBy(x => Guid.NewGuid()).ToList();
        //assign the random list to the original
        numbers = randomList;

        for (int i = 0; i < Boxes.Count; i++)
        {
            Boxes[i].Image = null;
            Boxes[i].Tag = numbers[i].ToString();
        }

        tries = 0;
        lblStatus.Text = "Mismatched: " + tries + " times.";
        lblTimeLeft.Text = "Time Left: " + totalTime;
        gameOver = false;
        GameTimer.Start();
        countDownTime = totalTime;
    }

        private void checkPictures(PictureBox A, PictureBox B)
        {
            if (firstChoice == secondChoice)
            {
                A.Tag = null;
                B.Tag = null;
            }
            else
            {
                tries++;
                lblStatus.Text = "Mismatched " + tries + " times.";
            }

            firstChoice = null;
            secondChoice = null;

            foreach (PictureBox pics in Boxes.ToList())
            {
                if (pics.Tag != null)
                {
                    pics.Image = null;
                }
            }

            // now lets check if all of the items have been solved

            if (Boxes.All(o => o.Tag == Boxes[0].Tag))
            {
                GameOver("Great Work, You Win!!!!");
            }

        }

        private void GameOver(string msg)
    {
        GameTimer.Stop();
        gameOver = true;
        MessageBox.Show(msg + "Click Restart to Play Again.", "Moo Says: ");
    }
}
}