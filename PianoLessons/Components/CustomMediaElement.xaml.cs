using CommunityToolkit.Maui.Core.Primitives;

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
		media.duration.Text = media.mediaElement.Duration.ToString();
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