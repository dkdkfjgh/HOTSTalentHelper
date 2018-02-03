using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace HOTSTalentHelper
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        HeroAnalyzer HAnalyze = new HeroAnalyzer();
        int CurrentLevel = 1;
        public MainWindow()
        {
            InitializeComponent();





        }
        public void SetImagefromUrl(string uri, Image Img)
        {
            Byte[] buffer;

            string myUri = uri;

            if (myUri.Substring(0, 4).Equals("http"))

            {

                WebClient wc = new WebClient();

                buffer = wc.DownloadData(new Uri(myUri, UriKind.Absolute));

                wc.Dispose();

            }

            else

            {

                buffer = System.IO.File.ReadAllBytes(myUri);

            }



            MemoryStream ms = new MemoryStream(buffer);



            BitmapImage img = new BitmapImage();

            img.BeginInit();

            img.CacheOption = BitmapCacheOption.OnLoad;

            img.StreamSource = ms;

            img.EndInit();

            Img.Source = img;
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.MouseDown += delegate { DragMove(); };
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentLevel == 20) return;
            if (CurrentLevel == 16) { CurrentLevel = 20; UpdateInfo();  return; }
            CurrentLevel += 3;
            UpdateInfo();
        }



        void UpdateInfo()
        {
            LevelText.Content = "Level : " + CurrentLevel.ToString();

            List<Talent> TalentByLevel = new List<Talent>();
            int level = CurrentLevel;
            if(HAnalyze.Analyze(textBox.Text) == false)
            {
                MessageBox.Show("Failed to Search!");
            }

            for(int i = 0; i < HAnalyze.SelectedHero.Tal.Count; i++)
            {
                if(HAnalyze.SelectedHero.Tal[i].Level == "Level: "+level.ToString())
                {
                    TalentByLevel.Add(HAnalyze.SelectedHero.Tal[i].Clone() as Talent);
                }
            }

            switch (HAnalyze.TalentCountbyNum(CurrentLevel))
            {
                case 1:
                    SetImagefromUrl(TalentByLevel[0].SkillImageUrl, image1);
                    TalentName_1.Content = TalentByLevel[0].Name;
                    TalentDesc_1.Content = TalentByLevel[0].Desc;
                    TalentPopularity_1.Content = TalentByLevel[0].Popularity;
                    TalentWinrate_1.Content = TalentByLevel[0].Winrate;

                    image2.Source = null;
                    TalentName_2.Content = null;
                    TalentDesc_2.Content = null;
                    TalentPopularity_2.Content = null;
                    TalentWinrate_2.Content = null;

                    image3.Source = null;
                    TalentName_3.Content = null;
                    TalentDesc_3.Content = null;
                    TalentPopularity_3.Content = null;
                    TalentWinrate_3.Content = null;

                    image4.Source = null;
                    TalentName_4.Content = null;
                    TalentDesc_4.Content = null;
                    TalentPopularity_4.Content = null;
                    TalentWinrate_4.Content = null;

                    break;
                case 2:
                    SetImagefromUrl(TalentByLevel[0].SkillImageUrl, image1);
                    TalentName_1.Content = TalentByLevel[0].Name;
                    TalentDesc_1.Content = TalentByLevel[0].Desc;
                    TalentPopularity_1.Content = TalentByLevel[0].Popularity;
                    TalentWinrate_1.Content = TalentByLevel[0].Winrate;

                    SetImagefromUrl(TalentByLevel[1].SkillImageUrl, image2);
                    TalentName_2.Content = TalentByLevel[1].Name;
                    TalentDesc_2.Content = TalentByLevel[1].Desc;
                    TalentPopularity_2.Content = TalentByLevel[1].Popularity;
                    TalentWinrate_2.Content = TalentByLevel[1].Winrate;

                    image3.Source = null;
                    TalentName_3.Content = null;
                    TalentDesc_3.Content = null;
                    TalentPopularity_3.Content = null;
                    TalentWinrate_3.Content = null;

                    image4.Source = null;
                    TalentName_4.Content = null;
                    TalentDesc_4.Content = null;
                    TalentPopularity_4.Content = null;
                    TalentWinrate_4.Content = null;

                    break;
                case 3:
                    SetImagefromUrl(TalentByLevel[0].SkillImageUrl, image1);
                    TalentName_1.Content = TalentByLevel[0].Name;
                    TalentDesc_1.Content = TalentByLevel[0].Desc;
                    TalentPopularity_1.Content = TalentByLevel[0].Popularity;
                    TalentWinrate_1.Content = TalentByLevel[0].Winrate;

                    SetImagefromUrl(TalentByLevel[1].SkillImageUrl, image2);
                    TalentName_2.Content = TalentByLevel[1].Name;
                    TalentDesc_2.Content = TalentByLevel[1].Desc;
                    TalentPopularity_2.Content = TalentByLevel[1].Popularity;
                    TalentWinrate_2.Content = TalentByLevel[1].Winrate;

                    SetImagefromUrl(TalentByLevel[2].SkillImageUrl, image3);
                    TalentName_3.Content = TalentByLevel[2].Name;
                    TalentDesc_3.Content = TalentByLevel[2].Desc;
                    TalentPopularity_3.Content = TalentByLevel[2].Popularity;
                    TalentWinrate_3.Content = TalentByLevel[2].Winrate;

                    image4.Source = null;
                    TalentName_4.Content = null;
                    TalentDesc_4.Content = null;
                    TalentPopularity_4.Content = null;
                    TalentWinrate_4.Content = null;
                    break;
                case 4:
                    SetImagefromUrl(TalentByLevel[0].SkillImageUrl, image1);
                    TalentName_1.Content = TalentByLevel[0].Name;
                    TalentDesc_1.Content = TalentByLevel[0].Desc;
                    TalentPopularity_1.Content = TalentByLevel[0].Popularity;
                    TalentWinrate_1.Content = TalentByLevel[0].Winrate;

                    SetImagefromUrl(TalentByLevel[1].SkillImageUrl, image2);
                    TalentName_2.Content = TalentByLevel[1].Name;
                    TalentDesc_2.Content = TalentByLevel[1].Desc;
                    TalentPopularity_2.Content = TalentByLevel[1].Popularity;
                    TalentWinrate_2.Content = TalentByLevel[1].Winrate;

                    SetImagefromUrl(TalentByLevel[2].SkillImageUrl, image3);
                    TalentName_3.Content = TalentByLevel[2].Name;
                    TalentDesc_3.Content = TalentByLevel[2].Desc;
                    TalentPopularity_3.Content = TalentByLevel[2].Popularity;
                    TalentWinrate_3.Content = TalentByLevel[2].Winrate;

                    SetImagefromUrl(TalentByLevel[3].SkillImageUrl, image4);
                    TalentName_4.Content = TalentByLevel[3].Name;
                    TalentDesc_4.Content = TalentByLevel[3].Desc;
                    TalentPopularity_4.Content = TalentByLevel[3].Popularity;
                    TalentWinrate_4.Content = TalentByLevel[3].Winrate;
                    break;
            }
            TalentByLevel.Clear();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentLevel = 1;
            UpdateInfo();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentLevel == 1) return;
            if (CurrentLevel == 20) { CurrentLevel = 16; UpdateInfo();  return; }
            CurrentLevel -= 3;
            UpdateInfo();
        }
    }
}
