using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Linq;
using System;

public class VideoManager : Singleton<VideoManager>
{
    public VideoPlayer[] movies;
    // Start is called before the first frame update

    // Update is called once per frame
    public void PlayMovie(string movieName)
    {
        var movie = FindMovie(movieName);
        movie.Play();
    }

    public void StopMovie(string movieName)
    {
        var movie = FindMovie(movieName);
        movie.Stop();
        
        //movie.Stop();
    }

    private VideoPlayer FindMovie(string movieName)
    {
        VideoPlayer result = movies.FirstOrDefault(x => x.name.Equals(movieName, StringComparison.CurrentCultureIgnoreCase));
        if (result == null)
        {
            throw new Exception("Cannot find movie " + movieName);
        }
        return result;
    }
}
