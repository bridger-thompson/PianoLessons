using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;

namespace PianoLessons.Components;

public partial class CustomMediaElement : ContentView
{
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(string), typeof(CustomMediaElement), propertyChanged: OnSourceChanged);

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

    void PlayRecording(object sender, EventArgs args)
    {
        if (IsNotPlaying())
        {
            mediaElement.Play();
            HidePlayButtonShowStopButton();
        }
    }

    void StopPlayingRecording(object sender, EventArgs args)
    {
        mediaElement.Pause();
        HideStopButtonShowPlayButton();
    }

    void RestartRecording(object sender, EventArgs args)
    {
        mediaElement.Stop();
        mediaElement.Play();
        HidePlayButtonShowStopButton();
	}

    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var source = (string)newValue;
        var media = (CustomMediaElement)bindable;
        media.mediaElement.Source = source;

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
        HideStopButtonShowPlayButton();
    }

    private bool IsNotPlaying()
    {
        return mediaElement.CurrentState == MediaElementState.Stopped ||
                    mediaElement.CurrentState == MediaElementState.Paused;
    }
    private void HidePlayButtonShowStopButton()
    {
        StopButton.IsVisible = true;
        PlayButton.IsVisible = false;
    }

    private void HideStopButtonShowPlayButton()
    {
        PlayButton.IsVisible = true;
        StopButton.IsVisible = false;
    }
}