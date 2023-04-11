using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;

namespace PianoLessons.Components;

public partial class CustomMediaElement : ContentView
{
	public static readonly BindableProperty SourceProperty =
		BindableProperty.Create(nameof(Source), typeof(string), typeof(CustomMediaElement), propertyChanged: OnSourceChanged);

	private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var source = (string)newValue;
		var media = (CustomMediaElement)bindable;
		media.mediaElement.Source = source;
	}

	public string Source
	{
		get => (string)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public CustomMediaElement()
	{
		InitializeComponent();
		mediaElement.MediaEnded += MediaElement_MediaEnded;
        mediaElement.PropertyChanged += MediaElement_PropertyChanged;
		position.Text = TimeSpan.Zero.ToString("hh\\:mm\\:ss");
	}


    private void Scrubber_DragStarted(object sender, EventArgs e)
    {
        //mediaElement.Pause();
        Play.IsVisible = true;
        Stop.IsVisible = false;
    }

    private void MediaElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
		{
			duration.Text = mediaElement.Duration.ToString("hh\\:mm\\:ss");
		}

		if (e.PropertyName == MediaElement.PositionProperty.PropertyName)
		{
			position.Text = mediaElement.Position.ToString("hh\\:mm\\:ss");
		}
    }

    private void MediaElement_MediaEnded(object sender, EventArgs e)
	{
		mediaElement.Stop();
		Play.IsVisible = true;
		Stop.IsVisible = false;
	}


	void PlayRecording(object sender, EventArgs args)
	{
		if (mediaElement.CurrentState == MediaElementState.Stopped ||
			mediaElement.CurrentState == MediaElementState.Paused)
		{
			mediaElement.Play();
			Stop.IsVisible = true;
			Play.IsVisible = false;
		}
	}

	void StopPlayingRecording(object sender, EventArgs args)
	{
		mediaElement.Pause();
		Play.IsVisible = true;
		Stop.IsVisible = false;
	}
}