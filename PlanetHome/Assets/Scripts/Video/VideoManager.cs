using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class VideoManager : Singleton<VideoManager>
{
    public RawImage movieScreen;
    public MovieTexture[] movies;
    // Start is called before the first frame update

    // Update is called once per frame
    public void PlayMovie(string movieName)
    {
        movieScreen.enabled = true;
        var movie = FindMovie(movieName);
        movieScreen.texture = movie;
        movie.Play();
    }

    public void StopMovie(string movieName)
    {
        movieScreen.enabled = false;
        MovieTexture movie = movieScreen.texture as MovieTexture;
        movie.Stop();
    }

    private MovieTexture FindMovie(string movieName)
    {
        MovieTexture result = movies.FirstOrDefault(x => x.name.Equals(movieName, StringComparison.CurrentCultureIgnoreCase));
        if (result == null)
        {
            throw new Exception("Cannot find movie " + movieName);
        }
        return result;
    }
}
