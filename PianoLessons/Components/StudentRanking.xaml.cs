namespace PianoLessons.Components;

public partial class StudentRanking : ContentView
{
	public static readonly BindableProperty RankProperty =
		BindableProperty.Create(nameof(Rank), typeof(int), typeof(StudentRanking), propertyChanged: OnRankChanged);

    private static void OnRankChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var rank = (int)newValue;
        var ranking = (StudentRanking)bindable;

        if (rank > 3)
        {
            ranking.TrophyImage.IsVisible = false;
            ranking.PlaceLabel.IsVisible = true;
            ranking.PlaceLabel.Text = rank.ToString();
        }
        else
        {
            ranking.PlaceLabel.IsVisible = false;
            ranking.TrophyImage.IsVisible = true;
            ranking.TrophyImage.Source = GetTrophyImageSource(rank);
        }
    }

    private static ImageSource GetTrophyImageSource(int rank)
    {
        switch (rank)
        {
            case 1:
                return "gold_cup.png";
            case 2:
                return "silver_cup.png";
            case 3:
                return "bronze_cup.png";
            default:
                return string.Empty;
        }
    }

    public int Rank
	{
		get => (int)GetValue(RankProperty);
		set => SetValue(RankProperty, value);
    }

    public bool FirstPlace => Rank == 1;
    public bool SecondPlace => Rank == 2;
    public bool ThirdPlace => Rank == 3;
    public bool NoTrophy => FirstPlace && SecondPlace && ThirdPlace;

    public StudentRanking()
	{
        InitializeComponent();
	}

}