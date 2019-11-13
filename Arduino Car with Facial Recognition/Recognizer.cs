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
public partial class Recognizer : Form
{
Image<Bgr, Byte> currentFrame;
Capture grabber;
HaarCascade face;
Image<Gray, byte> result, TrainedFace = null;
Image<Gray, byte> gray = null;
List<Image<Gray, byte>> trainingImages = new List<Image<Gray,
byte>>();
List<string> labels = new List<string>();
MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX,
0.5d, 0.5d);
List<string> NamePersons = new List<string>();
string name = null;
int t, ContTrain, NumLabels;
public Recognizer()
{
InitializeComponent();
face = new HaarCascade("haarcascade_frontalface_default.xml");
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
MessageBox.Show("no image trained");
}
}
private void Recognizer_Load(object sender, EventArgs e)
{
}
private void button1_Click(object sender, EventArgs e)
{
grabber = new Capture();
grabber.QueryFrame();
Application.Idle += new EventHandler(FrameGrabber);
}
void FrameGrabber(object sender, EventArgs e)
{
NamePersons.Add("");
label2.Text = "0";
currentFrame = grabber.QueryFrame().Resize(320, 240,
Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
gray = currentFrame.Convert<Gray, Byte>();
MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
face,
1.3,
10,
Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
new Size(20, 20));
foreach (MCvAvgComp f in facesDetected[0])
{
t = t + 1;
result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100,
100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);
//initialize result,t and gray if (trainingImages.ToArray().Length != 0)
{
MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain,
0.001);
EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
trainingImages.ToArray(),
labels.ToArray(),
1000,
ref termCrit);
name = recognizer.Recognize(result);
currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y -
2), new Bgr(Color.LightGreen));
}
NamePersons[t - 1] = name;
NamePersons.Add("");
label2.Text = facesDetected[0].Length.ToString();
}
imageBox1.Image = currentFrame;
}
private void button4_Click(object sender, EventArgs e)
{
StartScreen s = new StartScreen();
s.Show();
this.Hide();
}
}
}