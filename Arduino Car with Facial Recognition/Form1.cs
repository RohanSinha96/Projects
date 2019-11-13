using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
namespace FacialRecognition
{
public partial class Form1 : Form
{
Capture grabber;
Image<Bgr, byte> currentFrame;
Image<Gray, byte> gray,result,TrainedFace = null;
HaarCascade face;
List<Image<Gray, byte>> trainingImages = new List<Image<Gray,
byte>>();
List<string> labels = new List<string>();
int NumLabels,ContTrain=0;
int t = 0;
public Form1()
{
face = new HaarCascade("haarcascade_frontalface_default.xml");
InitializeComponent();
try
{
string Labelsinfo = File.ReadAllText(Application.StartupPath +
"/TrainedFaces/TrainedLabels.txt");
string[] Labels = Labelsinfo.Split('%');
NumLabels = Convert.ToInt16(Labels[0]);
ContTrain = NumLabels;
string LoadFaces;
for (int tf = 1; tf < NumLabels + 1; tf++)
{
LoadFaces = "face" + tf + ".bmp";
trainingImages.Add(new Image<Gray,
byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
labels.Add(Labels[tf]);
}
}
catch (Exception e)
{
MessageBox.Show("Train item if its first time");
}
}
private void Form1_Load(object sender, EventArgs e)
{
}
private void button1_Click(object sender, EventArgs e)
{
grabber = new Capture();
grabber.QueryFrame();
Application.Idle += new EventHandler(FrameGrabber);
button2.Visible = true;
}
void FrameGrabber(object sender, EventArgs e)
{
currentFrame = grabber.QueryFrame().Resize(320, 240,
Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
gray = currentFrame.Convert<Gray, Byte>();
MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
face,
1.2,
10,
Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
new Size(20, 20));
foreach (MCvAvgComp f in facesDetected[0])
{
t = t + 1;
result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100,
100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);
}
imageBox1.Image = currentFrame;
}
private void imageBox1_Click(object sender, EventArgs e)
{
}
private void button2_Click(object sender, EventArgs e)
{
label1.Visible = true;
textBox1.Visible = true;
button3.Visible = true;
TrainedFace = result.Resize(100, 100,
Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
imageBox2.Image = TrainedFace;
}
private void button3_Click(object sender, EventArgs e)
{
ContTrain=ContTrain+1;
trainingImages.Add(TrainedFace);
labels.Add(textBox1.Text);
File.WriteAllText(Application.StartupPath +
"/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString()
+ "%");//add library to read/write to input file
for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
{
trainingImages.ToArray()[i - 1].Save(Application.StartupPath +
"/TrainedFaces/face" + i + ".bmp");//sav faces to folder with name face(i)i is no.
of face and .bmp extension of detected face image
File.AppendAllText(Application.StartupPath +
"/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");//save names
to text file
}
MessageBox.Show("Image trained and save to database");

}
private void button4_Click(object sender, EventArgs e)
{
StartScreen s = new StartScreen();
s.Show();
this.Hide();
}
}
}